using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : UIItemSlot
{
    private Button m_Button;
    private UIInventoryController m_UIInventoryController;
    private void Awake()
    {
        m_SlotsController = transform.root.GetComponent<InventorySlotsController>();
        m_UIInventoryController  = transform.parent.parent.GetComponent<UIInventoryController>();
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener( Select );
    }

    private void Select()
    {
        m_UIInventoryController.SelectedItem = m_SlotsController.Slots[m_SlotIndex];
        if ( m_SlotsController.Slots[m_SlotIndex] == null )
        {
            m_UIInventoryController.DisableDropUI();
            return;
        }
        m_UIInventoryController.EnableDropUI();
    }
}




