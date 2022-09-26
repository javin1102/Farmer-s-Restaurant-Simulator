using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameController : MonoBehaviour
{
    [SerializeField] private GameObject m_ActionSlots;
    [SerializeField] private GameObject m_Crosshair;
    private PlayerAction m_PlayerAction;
    private void Awake()
    {
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
    }

    private void OnEnable()
    {
        m_PlayerAction.OnEnableUI += DisableActionSlot;
        m_PlayerAction.OnDisableUI += EnableActionSlot;
        m_PlayerAction.OnEnableUI += DisableCrosshair;
        m_PlayerAction.OnDisableUI += EnableCrosshair;
    }

    private void OnDisable()
    {
        m_PlayerAction.OnEnableUI -= DisableCrosshair;
        m_PlayerAction.OnDisableUI -= EnableCrosshair;
        m_PlayerAction.OnEnableUI -= DisableActionSlot;
        m_PlayerAction.OnDisableUI -= EnableActionSlot;
    }


    private void EnableActionSlot() => m_ActionSlots.SetActive( true );
    private void DisableActionSlot() => m_ActionSlots.SetActive( false );
    private void EnableCrosshair() => m_Crosshair.SetActive( true );
    private void DisableCrosshair() => m_Crosshair.SetActive( false );
}
