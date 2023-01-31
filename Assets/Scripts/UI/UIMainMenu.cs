using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button m_PlayButton, m_CreditsButton, m_ExitButton, m_CloseCreditButton;
    [SerializeField] private GameObject m_LoadingUI, m_CreditsUI;
    void Start()
    {
        m_PlayButton.onClick.AddListener(Play);
        m_CreditsButton.onClick.AddListener(Credits);
        m_CloseCreditButton.onClick.AddListener(CloseCredits);
        m_ExitButton.onClick.AddListener(Exit);
    }

    private void CloseCredits()
    {
        m_CreditsUI.SetActive(false);
    }

    private void Credits()
    {
        m_CreditsUI.SetActive(true);
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void Play() => StartCoroutine(Play_Coroutine());

    private IEnumerator Play_Coroutine()
    {
        m_LoadingUI.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        while (!asyncOperation.isDone) yield return null;
        m_LoadingUI.SetActive(false);
    }
}
