using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIActionSlot : UIItemSlot
{
    private InventoryController m_InventoryController;
    private PlayerAction m_PlayerAction;
    protected override void OnEnable()
    {
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
        m_ItemsController = transform.root.GetComponent<ActionSlotsController>();
        m_InventoryController = transform.root.GetComponent<InventoryController>();
        base.OnEnable();
        m_PlayerAction.OnPerformItemMainAction += UpdateUI;
        UpdateUI();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_PlayerAction.OnPerformItemMainAction -= UpdateUI;
    }

    protected override void OnDropAction( UIItemSlot originItemSlot )
    {
        
        if ( originItemSlot.GetType() == typeof( UIActionSlot ) )
            m_ItemsController.SwapSlotItem( m_SlotIndex, originItemSlot.SlotIndex );

        else
        {
            //From inventory slot, remove from inventory and set action slot
            m_ItemsController.SetSlotItem( originItemSlot.ItemSlotData, m_SlotIndex );
            m_InventoryController.SetSlotItem( null, originItemSlot.SlotIndex );
        }

        ActionSlotsController actionSlotsController = m_ItemsController as ActionSlotsController;
        actionSlotsController.CheckEquippedItem();
    }
}
