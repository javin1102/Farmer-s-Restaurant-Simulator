using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIPauseController : MonoBehaviour
{
    protected PlayerAction m_PlayerAction;
    [SerializeField] private Button m_ResumeButton;
    [SerializeField] private Button m_MainMenuButton;
    private SceneLoader m_SceneLoader;
    void Awake()
    {
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
        m_ResumeButton.onClick.AddListener(Resume);
        m_MainMenuButton.onClick.AddListener(MainMenu);
    }
    void Start()
    {
        m_SceneLoader = SceneLoader.Instance;
    }

    private void MainMenu()
    {
        m_SceneLoader.LoadSceneAsynchronous("MainMenu", LoadSceneMode.Single);
        Time.timeScale = 1f;
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
        m_PlayerAction.PlayAudio("button_sfx");
        Time.timeScale = 0;
    }

    private void Resume()
    {
        gameObject.SetActive(false);
        m_PlayerAction.IsOtherUIOpen = false;
        m_PlayerAction.ExitCursorMode();
        m_PlayerAction.PlayAudio("button_sfx");
        Time.timeScale = 1f;
    }
}
