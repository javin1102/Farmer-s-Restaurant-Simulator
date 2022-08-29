public class UIActionSlot : UIItemSlot
{
    private InventoryController m_InventoryController;
    private ActionSlotsController m_ActionSlotsController;
    private PlayerAction m_PlayerAction;

    protected override void Awake()
    {
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
        m_ItemsController = transform.root.GetComponent<ActionSlotsController>();
        m_InventoryController = transform.root.GetComponent<InventoryController>();
        m_ActionSlotsController = m_ItemsController as ActionSlotsController;
        base.Awake();
        m_PlayerAction.OnPerformItemMainAction += UpdateUI;
        m_ActionSlotsController.OnStoreExistingItem += UpdateUI;
    }


    private void OnDestroy()
    {
        m_ActionSlotsController.OnStoreExistingItem -= UpdateUI;
        m_PlayerAction.OnPerformItemMainAction -= UpdateUI;
    }

    protected override void OnDropAction( UIItemSlot originItemSlot )
    {
        if ( originItemSlot.GetType() == typeof( UIInventorySlot ) )
        {
            if ( m_ItemSlot == null )
            {
                m_InventoryController.ItemSlotsDictionary.Remove( originItemSlot.ItemSlot.data.id );
                m_ActionSlotsController.ItemSlotsDictionary.Add( originItemSlot.ItemSlot.data.id, originItemSlot.ItemSlot );
            }
            else
            {
                string key1 = m_ItemSlot.data.id;
                string key2 = originItemSlot.ItemSlot.data.id;

                (m_ActionSlotsController.ItemSlotsDictionary[key1], m_InventoryController.ItemSlotsDictionary[key2]) = (m_ActionSlotsController.ItemSlotsDictionary[key2], m_InventoryController.ItemSlotsDictionary[key1]);
            }
        }

        SwapUIItemSlot( originItemSlot );
        m_ActionSlotsController.InvokeUIDropItemEvent();
        m_ActionSlotsController.CheckEquippedItem();
    }
}
