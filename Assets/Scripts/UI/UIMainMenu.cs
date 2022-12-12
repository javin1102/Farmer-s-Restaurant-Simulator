using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button m_PlayButton, m_CreditsButton, m_ExitButton;
    [SerializeField] private GameObject m_LoadingUI;
    void Start()
    {
        m_PlayButton.onClick.AddListener(Play);
        m_ExitButton.onClick.AddListener(Exit);
    }

    private void Exit()
    {

    }

    private void Play() => StartCoroutine(Play_Coroutine());

    private IEnumerator Play_Coroutine()
    {
        m_LoadingUI.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        while (!asyncOperation.isDone) yield return null;
        m_LoadingUI.SetActive(false);
    }
}
