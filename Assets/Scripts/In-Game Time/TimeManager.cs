using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField]
    GameTimeStamp m_timeStamp;

    public float m_timeScale = 1f;

    public Transform m_sun;


    public List<ITimeTracker> m_listenner = new List<ITimeTracker>();

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
    }

    private void Start()
    {
        // initialize GameTimeStamp day1, 6 : 00 AM
        m_timeStamp = new GameTimeStamp(1,6,0);

        // start update time 
        StartCoroutine(TimeUpdate());
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            // m_timeScale to define how fast the in game time
            Tick();
            yield return new WaitForSeconds(1 / m_timeScale);
        }
    }

    public void Tick()
    {
        // update clock
        m_timeStamp.UpdateClock();

        for (int i = 0; i < m_listenner.Count; i++)
        {
            m_listenner[i].ClockUpdate(m_timeStamp);
        }

        // calculate sun angle
        int timeInMinutes = GameTimeStamp.HourToMinutes(m_timeStamp.hour) + m_timeStamp.minute;
        float sunAngle = .25f * timeInMinutes - 90;

        // change the directional light angle
        m_sun.eulerAngles = new Vector3(sunAngle, 0, 0);

    }

    public GameTimeStamp GetGameTimeStamp()
    {
        return new GameTimeStamp(m_timeStamp);
    }


    public void RegisterListener(ITimeTracker listener)
    {
        m_listenner.Add(listener);
    }

    public void UnRegisterListener(ITimeTracker listner)
    {
        m_listenner.Remove(listner);
    }

}
