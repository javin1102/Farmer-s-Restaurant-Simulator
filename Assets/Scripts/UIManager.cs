using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour,ITimeTracker
{
    public static UIManager Instance { get; set; }
    [Header("=== CLOCK UI ===")]
    [SerializeField] private TMP_Text m_clockText;

    [Header("=== Action Helper UI ===")]
    [SerializeField] private GameObject m_ActionHelperPrimaryGO;
    [SerializeField] private GameObject m_ActionHelperSecondaryGO;
    [SerializeField] private Image m_PrimaryActionImage;
    [SerializeField] private TMP_Text m_PrimaryActionText;
    [SerializeField] private Image m_SecondaryActionImage;
    [SerializeField] private TMP_Text m_SecondaryActionText;

    private SceneLoader m_SceneLoader;
    private TimeManager m_TimeManager;
    private void Awake()
    {
        if ( Instance == null ) Instance = this;
        else Destroy( gameObject );

        DontDestroyOnLoad( this );
    }
    
    private void Start()
    {
        // add listener
        m_TimeManager = TimeManager.Instance;
        m_TimeManager.RegisterListener(this);
        m_SceneLoader = SceneLoader.Instance;
        m_SceneLoader.OnFinishLoading += HideActionHelper;
    }

    private void OnDestroy()
    {
        m_SceneLoader.OnFinishLoading -= HideActionHelper;
    }


    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        m_clockText.text = timeStamp.hour.ToString("00") + " : " + timeStamp.minute.ToString("00");
    }

    public void ShowActionHelperPrimary(string imageName, string actionText)
    {
        if ( m_PrimaryActionImage == null || m_PrimaryActionText.text == null ) return;
        // active action helper container
        m_ActionHelperPrimaryGO.SetActive(true);

        // load sprite for action helper
        m_PrimaryActionImage.sprite = Resources.Load<Sprite>("ActionHelper/" + imageName);

        // set action helper text
        m_PrimaryActionText.text = actionText;
    }

    public void ShowActionHelperSecondary( string imageName, string actionText )
    {
        if ( m_PrimaryActionImage == null || m_PrimaryActionText.text == null ) return;
        // active action helper container
        m_ActionHelperSecondaryGO.SetActive( true );

        // load sprite for action helper
        m_SecondaryActionImage.sprite = Resources.Load<Sprite>( "ActionHelper/" + imageName );

        // set action helper text
        m_SecondaryActionText.text = actionText;
    }

    public void HideActionHelper()
    {
        m_ActionHelperPrimaryGO.SetActive(false);
        m_ActionHelperSecondaryGO.SetActive( false );
    }

}
