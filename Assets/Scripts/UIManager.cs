using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour,ITimeTracker
{
    [Header("=== CLOCK UI ===")]
    [SerializeField] private TMP_Text m_clockText;

    [Header("=== Action Helper UI ===")]
    public Image m_actionImage;
    public TMP_Text m_actionText;
    
    public static UIManager Instance { get; set; }
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        // add listener
        TimeManager.Instance.RegisterListener(this);
    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        m_clockText.text = timeStamp.hour.ToString("00") + " : " + timeStamp.minute.ToString("00");
    }

    public void ShowActionHelper(string imageName, string actionText)
    {
        // active action helper container
        m_actionImage.transform.parent.gameObject.SetActive(true);

        // load sprite for action helper
        m_actionImage.sprite = Resources.Load<Sprite>("ActionHelper/" + imageName);

        // set action helper text
        m_actionText.text = actionText;
    }

    public void HideActionHelper()
    {
        Debug.Log("setactive false");
        // deactive action helper container
        m_actionImage.transform.parent.gameObject.SetActive(false);
    }

}
