public class InventorySlotsController : ItemSlotsController
{
    private PlayerAction m_PlayerAction;
    private void Awake()
    {
        m_MaxSlotSize = 42;
        m_Slots = new ItemSlot[m_MaxSlotSize];
        m_SlotSize = 20;
    }

    public void CheckItem()
    {
        for ( int i = 0; i < m_SlotSize; i++ )
        {
            if ( m_Slots[i] == null ) continue;
            if ( m_Slots[i].quantity <= 0 ) m_Slots[i] = null;
        }
    }

    public override bool TrySetSlot( ItemSlot slot )
    {
        for ( int i = 0; i < m_SlotSize; i++ )
        {
            if ( m_Slots[i] == null )
            {
                m_Slots[i] = slot;
                return true;
            }
        }

        return false;
    }
}


