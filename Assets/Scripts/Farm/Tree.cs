using System;
using UnityEngine;

[RequireComponent(typeof(Hoverable))]
public class Tree : BaseFarmObject, IActionTime
{

    public float ActionTime { get; set; }
    public float DefaultActionTime => 3.5f;
    private PlayerAction m_PlayerAction;
    private Hoverable m_Hoverable;
    private UIManager m_UIManager;

    private new void Start()
    {
        base.Start();
        m_Hoverable = GetComponent<Hoverable>();
        ActionTime = DefaultActionTime;
        m_UIManager = UIManager.Instance;
        m_Hoverable.OnHoverEnter += ShowHelper;
        m_Hoverable.OnHoverExit += OnHoverExit;
    }

    private void ShowHelper()
    {
        m_UIManager.ShowActionHelperPrimary("Left", "Tahan untuk menebang");
    }

    public void OnHoldMainAction(PlayerAction playerAction)
    {
        m_PlayerAction = playerAction;
        Debug.Log(m_PlayerAction);
        float multiplier = 1;
        if (playerAction.CurrEquippedItem != null && playerAction.CurrEquippedItem.Data.ID.Equals("Kapak")) multiplier = 3;
        ActionTime -= Time.deltaTime * multiplier;
        playerAction.DefaultActionTime = DefaultActionTime;
        playerAction.ActionTime = ActionTime;
    }

    public void OnReleaseMainAction(PlayerAction playerAction)
    {
        ActionTime = DefaultActionTime;
        playerAction.DefaultActionTime = 0;
        playerAction.ActionTime = 0;
    }

    private void OnHoverExit()
    {
        ActionTime = DefaultActionTime;
        m_UIManager.HideActionHelper();
    }

    private void Update()
    {
        if (ActionTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        if (m_PlayerAction != null)
            m_PlayerAction.PlayAudio("thump_sfx");
        m_Hoverable.OnHoverExit -= OnHoverExit;
        m_Hoverable.OnHoverEnter -= ShowHelper;
    }
}
