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
        // DontDestroyOnLoad(this);
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
        float currentTimeHour = m_CurrentTimeStamp.hour + m_CurrentTimeStamp.minute / 60;
        //time between 14:30 -> 19:30
        if (currentTimeHour <= 19.5f && currentTimeHour >= 14.5f)
        {
            skybox.SetFloat("_CubemapTransition", Mathf.InverseLerp(14.5f, 19.5f, currentTimeHour));
        }
        //time between 03:00 -> 11:59
        else if (currentTimeHour >= 3f && currentTimeHour < 12f)
        {
            skybox.SetFloat("_CubemapTransition", 1 - Mathf.InverseLerp(3, 12f, currentTimeHour));
        }
        //time between 19:30 -> 02:59
        else if ((currentTimeHour > 19.5f && currentTimeHour < 23.59f) || (currentTimeHour < 3f))
        {
            skybox.SetFloat("_CubemapTransition", 1);
        }
        //time between 12:00 -> 14:29
        else if (currentTimeHour >= 12f && currentTimeHour < 14.5f)
        {
            skybox.SetFloat("_CubemapTransition", 0);
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
