using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameController : MonoBehaviour
{
    [SerializeField] private GameObject m_ActionSlots;
    [SerializeField] private GameObject m_Crosshair;
    [SerializeField] private Button m_UIButton;
    [SerializeField] private TMP_Text m_CoinText;
    private PlayerAction m_PlayerAction;
    private void Awake()
    {
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
    }

    private void Start()
    {
        m_UIButton.onClick.AddListener(m_PlayerAction.InvokeToggleMiscUI);
        m_UIButton.onClick.AddListener(() => m_PlayerAction.PlayAudio(Utils.BUTTON_SFX));
    }

    void Update()
    {
        m_CoinText.text = $"<indent=40%><sprite=0><color=yellow>{PlayerAction.Coins}</color>";
    }
    private void OnEnable()
    {
        m_PlayerAction.OnEnableOtherUI += DisableChilds;
        m_PlayerAction.OnDisableOtherUI += EnableChilds;
    }

    private void OnDisable()
    {
        m_PlayerAction.OnEnableOtherUI -= DisableChilds;
        m_PlayerAction.OnDisableOtherUI -= EnableChilds;
    }


    private void EnableChilds()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void DisableChilds()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
