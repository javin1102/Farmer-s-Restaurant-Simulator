using System;
using UnityEngine;
[RequireComponent(typeof(Hoverable))]
public class Bed : MonoBehaviour, IInteractable
{
    private SaveManager m_SaveManager;
    private TimeManager m_TimeManager;
    private UIManager m_UIManager;
    private SceneLoader m_SceneLoader;
    private Hoverable m_Hoverable;
    public void Interact(PlayerAction m_PlayerAction)
    {
        //time skip and save
        GameTimeStamp skippedTimeStamp = new GameTimeStamp(m_TimeManager.CurrentTimeStamp.day + 1, 6, 0);
        m_TimeManager.CurrentTimeStamp = skippedTimeStamp;
        m_SaveManager.OnSave?.Invoke();
        m_SceneLoader.SpawnToScene(SPAWN_TYPE.HOUSE_BED);
    }

    void Start()
    {
        m_SaveManager = SaveManager.Instance;
        m_TimeManager = TimeManager.Instance;
        m_UIManager = UIManager.Instance;
        m_SceneLoader = SceneLoader.Instance;
        m_Hoverable = GetComponent<Hoverable>();
        m_Hoverable.OnHoverEnter += ShowHelperUI;
        m_Hoverable.OnHoverExit += HideHelperUI;
    }

    private void ShowHelperUI() => m_UIManager.ShowActionHelperPrimary("Left", "Tidur dan Save");
    private void HideHelperUI() => m_UIManager.HideActionHelper();
}
