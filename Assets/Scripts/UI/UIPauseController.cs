using UnityEngine;
using UnityEngine.UI;
public class UIPauseController : MonoBehaviour
{
    protected PlayerAction m_PlayerAction;
    [SerializeField] private Button m_ResumeButton;
    void Awake()
    {
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
        m_ResumeButton.onClick.AddListener(Resume);
    }
    public void ToggleUI()
    {
        if (!gameObject.activeInHierarchy) Pause();
        else Resume();
    }

    private void Pause()
    {
        gameObject.SetActive(true);
        m_PlayerAction.IsOtherUIOpen = true;
        m_PlayerAction.EnterCursorMode();
        // m_PlayerAction.OnEnableOtherUI?.Invoke();
        Time.timeScale = 0;
    }

    private void Resume()
    {
        gameObject.SetActive(false);
        m_PlayerAction.IsOtherUIOpen = false;
        m_PlayerAction.ExitCursorMode();
        // m_PlayerAction.OnDisableOtherUI?.Invoke();
        Time.timeScale = 1f;
    }
}
