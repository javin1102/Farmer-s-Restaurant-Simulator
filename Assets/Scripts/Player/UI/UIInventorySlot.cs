public class UIInventorySlot : UIItemSlot
{
    protected ActionSlotsController m_ActionSlotsController;
    protected override void OnEnable()
    {
        m_ItemsController = transform.root.GetComponent<InventoryController>();
        m_ActionSlotsController = transform.root.GetComponent<ActionSlotsController>();
        base.OnEnable();
        UpdateUI();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnDropAction( UIItemSlot originItemSlot )
    {
        if ( ( originItemSlot.GetType() == typeof( UIInventorySlot ) ) )
            m_ItemsController.SwapSlotItem( m_SlotIndex, originItemSlot.SlotIndex );

        else
        {
            //From Action Slot to Inventory Slot, Add back to inventory, set action slot null
            m_ItemsController.SetSlotItem( originItemSlot.ItemSlotData, m_SlotIndex );
            m_ActionSlotsController.SetSlotItem( null, originItemSlot.SlotIndex );
        }

    }

}
