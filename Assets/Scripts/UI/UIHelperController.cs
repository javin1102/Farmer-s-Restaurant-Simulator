using UnityEngine;
using UnityEngine.UI;

public class UIHelperController : MonoBehaviour
{
    private PlayerAction m_PlayerAction;
    [SerializeField] private Button m_CaraTanamButton, m_RestoranButton, m_MenuUIButton, m_TokoButton, m_CloseButton;
    [SerializeField] private GameObject m_CaraTanamPanel, m_RestoranPanel, m_MenuUIPanel, m_TokoPanel;

    private void Awake()
    {
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
        m_CloseButton.onClick.AddListener(CloseUI);
        m_CaraTanamButton.onClick.AddListener(OpenCaraTanamPanel);
        m_RestoranButton.onClick.AddListener(OpenRestoranPanel);
        m_MenuUIButton.onClick.AddListener(OpenMenuUIPanel);
        m_TokoButton.onClick.AddListener(OpenCaraTokoPanel);
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
        m_PlayerAction.IsOtherUIOpen = false;
        m_PlayerAction.OnDisableOtherUI?.Invoke();
        m_PlayerAction.PlayAudio(Utils.BUTTON_SFX);
    }

    public void ToggleUI()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        m_PlayerAction.IsOtherUIOpen = gameObject.activeInHierarchy;
        if (gameObject.activeInHierarchy)
        {
            m_PlayerAction.OnEnableOtherUI?.Invoke();
        }
        else
        {
            m_PlayerAction.OnDisableOtherUI?.Invoke();
        }

    }

    private void OpenCaraTanamPanel()
    {
        m_PlayerAction.PlayAudio(Utils.BUTTON_SFX);
        m_CaraTanamPanel.SetActive(true);
    }
    private void CloseCaraTanamPanel()
    {
        m_PlayerAction.PlayAudio(Utils.BUTTON_SFX);
        m_CaraTanamPanel.SetActive(false);
    }
    private void OpenRestoranPanel()
    {
        m_PlayerAction.PlayAudio(Utils.BUTTON_SFX);
        m_RestoranPanel.SetActive(true);
    }
    private void CloseRestoranPanel()
    {
        m_PlayerAction.PlayAudio(Utils.BUTTON_SFX);
        m_RestoranPanel.SetActive(false);
    }
    private void OpenMenuUIPanel()
    {
        m_PlayerAction.PlayAudio(Utils.BUTTON_SFX);
        m_MenuUIPanel.SetActive(true);
    }
    private void CloseMenuUIPanel()
    {
        m_PlayerAction.PlayAudio(Utils.BUTTON_SFX);
        m_MenuUIPanel.SetActive(false);
    }
    private void OpenCaraTokoPanel()
    {
        m_PlayerAction.PlayAudio(Utils.BUTTON_SFX);
        m_TokoPanel.SetActive(true);
    }

}
