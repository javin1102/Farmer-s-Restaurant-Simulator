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

    [Header("UI FOR CLOCK")]
    [SerializeField] private TMP_Text m_clockText;

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
            yield return new WaitForSeconds(1 / m_timeScale);
            Tick();
        }
    }

    public void Tick()
    {
        // update clock
        m_timeStamp.UpdateClock();

        // calculate sun angle
        int timeInMinutes = GameTimeStamp.HourToMinutes(m_timeStamp.hour) + m_timeStamp.minute;
        float sunAngle = .25f * timeInMinutes - 90;

        // change the directional light angle
        m_sun.eulerAngles = new Vector3(sunAngle, 0, 0);


        // update the clock UI
        m_clockText.text = m_timeStamp.hour.ToString("00") + " : " + m_timeStamp.minute.ToString("00");


    }

}
