using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SettingManager : MonoBehaviour
{
    [Header("=== AUDIO MIXER ===")]
    public AudioMixer m_audioMixer;
    private const string MASTER = "Master_Volume";
    private const string BGM = "BGM_Volume";
    private const string SFX = "SFX_Volume";

    [Header("=== AUDIO UI ===")]
    public Slider m_masterSlider;
    public Slider m_bgmSlider;
    public Slider m_sfxSlider;

    private void Start()
    {
        LoadAudioSetting();
    }

    private void LoadAudioSetting()
    {
        // assign float to audio mixer
        m_audioMixer.SetFloat(MASTER, Mathf.Log10(PlayerPrefs.GetFloat(MASTER , 1)) * 20);
        m_audioMixer.SetFloat(BGM, Mathf.Log10(PlayerPrefs.GetFloat(BGM , 1)) * 20);
        m_audioMixer.SetFloat(SFX, Mathf.Log10(PlayerPrefs.GetFloat(SFX , 1)) * 20);

        // assign float to slider
        m_masterSlider.value = PlayerPrefs.GetFloat(MASTER, 1);
        m_bgmSlider.value = PlayerPrefs.GetFloat(BGM, 1);
        m_sfxSlider.value = PlayerPrefs.GetFloat(SFX, 1);
    }

    public void SetMasterVolume(float sliderValue)
    {
        Debug.Log("master");
        Debug.Log(m_audioMixer.GetFloat(MASTER, out float value));
        m_audioMixer.SetFloat("Master_Volume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetBGMVolume(float sliderValue)
    {
        m_audioMixer.SetFloat(BGM, Mathf.Log10(sliderValue) * 20);
    }
    public void SetSFXVolume(float sliderValue)
    {
        m_audioMixer.SetFloat(SFX, Mathf.Log10(sliderValue) * 20);
    }

    public void SaveAudioSetting()
    {
        // save to playerpref after close setting page
        PlayerPrefs.SetFloat(MASTER, m_masterSlider.value);
        PlayerPrefs.SetFloat(BGM, m_bgmSlider.value);
        PlayerPrefs.SetFloat(SFX, m_sfxSlider.value);
    }


}
