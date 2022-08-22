using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIActionSlotController : UIItemSlotsController
{
    private ActionSlotsController m_ActionSlotsController;
    protected override void Awake()
    {
        base.Awake();
        m_ActionSlotsController = transform.root.GetComponent<ActionSlotsController>();
        m_ActionSlotsController.OnStoreNewItem += SetUISlotReference;
        m_ActionSlotsController.OnDropItem += UpdateActionSlots;
    }

    protected override void SetUISlotReference( ItemSlot itemSlot )
    {
        m_UIItemSlots[m_ActionSlotsController.SelectedSlotIndex].ItemSlot = itemSlot;
        m_UIItemSlots[m_ActionSlotsController.SelectedSlotIndex].UpdateUI();
    }

    private void OnEnable()
    {
        for ( int i = 0; i < m_ActionSlotsController.ActionSlots.Length; i++ )
        {
            m_UIItemSlots[i].ItemSlot = m_ActionSlotsController.ActionSlots[i];
            m_UIItemSlots[i].UpdateUI();
        }

    }
    private void UpdateActionSlots()
    {
        for ( int i = 0; i < m_ActionSlotsController.ActionSlots.Length; i++ )
        {
            m_ActionSlotsController.ActionSlots[i] = m_UIItemSlots[i].ItemSlot;
        }
    }

    
}
