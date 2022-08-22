public class UIInventoryController : UIItemSlotsController
{
    private InventoryController m_InventoryController;
    protected override void Awake()
    {
        base.Awake();
        m_InventoryController = transform.root.GetComponent<InventoryController>();
        m_InventoryController.OnStoreNewItem += SetUISlotReference;
    }

    protected override void SetUISlotReference( ItemSlot itemSlot )
    {
        for ( int i = 0; i < m_UIItemSlots.Length; i++ )
        {
            if ( m_UIItemSlots[i].ItemSlot == null )
            {
                m_UIItemSlots[i].ItemSlot = itemSlot;
                m_UIItemSlots[i].UpdateUI();
                break;
            }
        }
    }

}
