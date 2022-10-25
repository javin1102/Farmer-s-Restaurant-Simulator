using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameController : MonoBehaviour
{
    [SerializeField] private GameObject m_ActionSlots;
    [SerializeField] private GameObject m_Crosshair;
    [SerializeField] private Button m_UIButton;
    [SerializeField] private Button m_InventoryButton;
    private PlayerAction m_PlayerAction;
    private void Awake()
    {
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
        m_InventoryButton.onClick.AddListener( m_PlayerAction.InvokeToggleInventoryUI);
        m_UIButton.onClick.AddListener( m_PlayerAction.InvokeToggleMiscUI );
    }

    private void OnEnable()
    {
        m_PlayerAction.OnEnableUI += DisableChilds;
        m_PlayerAction.OnDisableUI += EnableChilds;
    }

    private void OnDisable()
    {
        m_PlayerAction.OnEnableUI -= DisableChilds;
        m_PlayerAction.OnDisableUI -= EnableChilds;
    }


    private void EnableChilds()
    {
        foreach ( Transform child in transform )
        {
            child.gameObject.SetActive( true );
        }
    }

    private void DisableChilds()
    {
        foreach ( Transform child in transform )
        {
            child.gameObject.SetActive( false );
        }
    }


    private void EnableActionSlot() => m_ActionSlots.SetActive( true );
    private void DisableActionSlot() => m_ActionSlots.SetActive( false );
    private void EnableCrosshair() => m_Crosshair.SetActive( true );
    private void DisableCrosshair() => m_Crosshair.SetActive( false );
}
