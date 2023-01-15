using System;
using UnityEngine;


public class PesticideSystem : MonoBehaviour,ITimeTracker
{
    public static PesticideSystem Instance { get; set; }
    public int pesticideDay { get => m_pesticideDay; }
    public bool isAlreadyPesticide { get => m_isAlreadyPesticide; }

    private int m_pesticideDay;
    private bool m_isAlreadyPesticide;
    private int m_tempDay;
    private bool m_isAlreadySet = false;

    // const string for playerpref
    private const string PESTICEDAY_KEYWORD = "PesticideDay";
    private const string ISALREADYPESTICIDE_KEYWORD = "IsAlreadyPesticide";

    private void Awake()
    {
        Instance = this;

        TimeManager.Instance.RegisterListener(this);

        m_pesticideDay = PlayerPrefs.GetInt(PESTICEDAY_KEYWORD, 0);
        m_isAlreadyPesticide = Convert.ToBoolean(PlayerPrefs.GetString(ISALREADYPESTICIDE_KEYWORD,"false"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && m_isAlreadyPesticide == false)
        {
            Debug.Log("enter ontrigger enter");
            
            m_pesticideDay = 3;
            PlayerPrefs.SetInt(PESTICEDAY_KEYWORD, m_pesticideDay);

            m_isAlreadyPesticide = true;
            PlayerPrefs.SetString(ISALREADYPESTICIDE_KEYWORD, "true");

            m_isAlreadySet = false;
            Debug.Log("Success Pesticide Plant ~");
        }
    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        if(timeStamp.hour == 6 && timeStamp.minute == 1 && m_isAlreadyPesticide == true)
        {
            if(m_pesticideDay <= 0)
            {
                Debug.Log("END of pesticide duration");
                m_isAlreadyPesticide = false;
                PlayerPrefs.SetString(ISALREADYPESTICIDE_KEYWORD, "false");
                m_isAlreadySet = false;
                return;
            }
            m_pesticideDay--;
            PlayerPrefs.SetInt(PESTICEDAY_KEYWORD, m_pesticideDay);
        }
    }
}
