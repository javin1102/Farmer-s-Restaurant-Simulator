using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOtherController : MonoBehaviour
{
    [SerializeField] private GameObject m_InventoryUI;
    private PlayerAction m_PlayerAction;

    private void Awake()
    {
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
    }
    private void OnEnable()
    {
        m_PlayerAction.ToggleInventoryUI += ToggleUI;
    }
    private void OnDisable()
    {
        m_PlayerAction.ToggleInventoryUI -= ToggleUI;
    }
    private void ToggleUI()
    {
        m_InventoryUI.SetActive( !m_InventoryUI.activeInHierarchy );
        if ( m_InventoryUI.activeInHierarchy ) m_PlayerAction.OnEnableUI?.Invoke();
        else m_PlayerAction.OnDisableUI?.Invoke();
    }
}
