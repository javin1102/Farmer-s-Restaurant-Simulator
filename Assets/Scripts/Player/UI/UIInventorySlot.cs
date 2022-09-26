using System;
using UnityEngine.UI;

public class UIInventorySlot : UIItemSlot
{
    protected ActionSlotsController m_ActionSlotsController;
    private InventoryController m_InventoryController;
    private Button m_Button;
    private UIInventoryController m_UIInventoryController;
    protected override void Awake()
    {
        m_UIInventoryController = transform.parent.parent.GetComponent<UIInventoryController>(); 
        m_ItemsController = transform.root.GetComponent<InventoryController>();
        m_ActionSlotsController = transform.root.GetComponent<ActionSlotsController>();
        m_InventoryController = m_ItemsController as InventoryController;
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener( Select );
        base.Awake();
        m_InventoryController.OnStoreExistingItem += UpdateUI;
        m_InventoryController.OnDropItem += UpdateUI;
    }

    private void Select()
    {
        m_UIInventoryController.SelectedItem = m_ItemSlot;
        if ( m_ItemSlot == null )
        {
            m_UIInventoryController.DisableDropUI();
            return;
        }
        m_UIInventoryController.EnableDropUI();
    }

    private void OnDestroy()
    {
        m_InventoryController.OnStoreExistingItem -= UpdateUI;
        m_InventoryController.OnDropItem -= UpdateUI;
    }

    protected override void OnDropAction( UIItemSlot originItemSlot )
    {
        if ( ( originItemSlot.GetType() == typeof( UIActionSlot ) ) )
        {
            if ( m_ItemSlot == null )
            {
                //From Action Slot to Inventory Slot, Add back to inventory, set action slot null
                m_InventoryController.ItemSlotsDictionary.Add( originItemSlot.ItemSlot.data.id, originItemSlot.ItemSlot );
                m_ActionSlotsController.ItemSlotsDictionary.Remove( originItemSlot.ItemSlot.data.id );
            }
            else
            {
                string key1 = m_ItemSlot.data.id;
                string key2 = originItemSlot.ItemSlot.data.id;

                (m_InventoryController.ItemSlotsDictionary[key1], m_ActionSlotsController.ItemSlotsDictionary[key2]) = (m_InventoryController.ItemSlotsDictionary[key2], m_ActionSlotsController.ItemSlotsDictionary[key1]);
            }
        }
        SwapUIItemSlot( originItemSlot );
        m_ActionSlotsController.InvokeUIDropItemEvent();
        m_ActionSlotsController.CheckEquippedItem();
    }

}
