using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour,ITimeTracker
{
    [SerializeField] private TMP_Text m_clockText;

    private void Start()
    {
        // add listener
        TimeManager.Instance.RegisterListener(this);
    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        m_clockText.text = timeStamp.hour.ToString("00") + " : " + timeStamp.minute.ToString("00");
    }
}
