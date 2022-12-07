using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    public GameTimeStamp CurrentTimeStamp { get => m_CurrentTimeStamp; set => m_CurrentTimeStamp = value; }

    [SerializeField]
    GameTimeStamp m_CurrentTimeStamp;

    public float TimeScale = 1f;

    public Material skybox;

    public List<ITimeTracker> m_Listener = new List<ITimeTracker>();

    private SaveManager m_SaveManager;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        m_SaveManager = SaveManager.Instance;
        m_SaveManager.LoadData(Utils.GAME_TIME_FILENAME, OnLoadSucceeded, OnLoadFailed);


        // initialize Cubemap Transition to 0 
        skybox.SetFloat("_CubemapTransition", 0);

        // start update time 
        StartCoroutine(TimeUpdate());

        m_SaveManager.OnSave += SaveGameTime;
    }
    private void OnDestroy()
    {
        m_SaveManager.OnSave -= SaveGameTime;
    }

    private async void SaveGameTime()
    {
        JSONObject timeObj = m_CurrentTimeStamp.Serialize();
        await m_SaveManager.SaveData(timeObj.ToString(), Utils.GAME_TIME_FILENAME);
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            // m_timeScale to define how fast the in game time
            Tick();
            yield return new WaitForSeconds(1 / TimeScale);
        }
    }

    public void Tick()
    {

        // check time for plus or minus
        if (m_CurrentTimeStamp.hour > 6)
        {
            if (m_CurrentTimeStamp.minute >= 59)
            {
                if (skybox.GetFloat("_CubemapTransition") >= 1f) skybox.SetFloat("_CubemapTransition", 1f);
                else skybox.SetFloat("_CubemapTransition", (skybox.GetFloat("_CubemapTransition") + 0.065f));
            }
        }
        else
        {
            if (m_CurrentTimeStamp.minute >= 59)
            {
                if (skybox.GetFloat("_CubemapTransition") <= 0f) skybox.SetFloat("_CubemapTransition", 0f);
                else skybox.SetFloat("_CubemapTransition", (skybox.GetFloat("_CubemapTransition") - .19f));
            }
        }

        // update clock
        m_CurrentTimeStamp.UpdateClock();

        for (int i = 0; i < m_Listener.Count; i++)
        {
            m_Listener[i].ClockUpdate(m_CurrentTimeStamp);
        }
    }

    public GameTimeStamp GetCurrentTimeStamp()
    {
        return new GameTimeStamp(m_CurrentTimeStamp);
    }


    public void RegisterListener(ITimeTracker listener)
    {
        m_Listener.Add(listener);
    }

    public void UnRegisterListener(ITimeTracker listner)
    {
        m_Listener.Remove(listner);
    }


    private void OnLoadSucceeded(JSONNode jsonNode) => m_CurrentTimeStamp = new(jsonNode);

    private void OnLoadFailed() => m_CurrentTimeStamp = new GameTimeStamp(1, 6, 0);
}
