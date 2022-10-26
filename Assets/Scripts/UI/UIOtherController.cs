using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOtherController : MonoBehaviour
{

    private PlayerAction m_PlayerAction;
    [SerializeField] private UIMiscController m_UIMiscController;
    [SerializeField] private UIFurnitureStoreController m_FurnitureStoreController;
    [SerializeField] private UIFarmStoreController m_SeedStoreController;
    private void Awake()
    {
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
    }
    private void OnEnable()
    {
        m_PlayerAction.ToggleMiscUI += m_UIMiscController.ToggleUI;
        m_PlayerAction.ToggleInventoryUI += m_UIMiscController.ToggleInventoryUI;
        m_PlayerAction.ToggleFurnitureStoreUI += m_FurnitureStoreController.ToggleUI;
        m_PlayerAction.ToggleSeedStoreUI += m_SeedStoreController.ToggleUI;

    }
    private void OnDisable()
    {

        m_PlayerAction.ToggleInventoryUI -= m_UIMiscController.ToggleInventoryUI;
        m_PlayerAction.ToggleMiscUI -= m_UIMiscController.ToggleUI;
        m_PlayerAction.ToggleFurnitureStoreUI -= m_FurnitureStoreController.ToggleUI;
        m_PlayerAction.ToggleSeedStoreUI -= m_SeedStoreController.ToggleUI;

    }

}
