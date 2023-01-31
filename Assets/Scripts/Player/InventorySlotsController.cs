public class InventorySlotsController : ItemSlotsController
{
    private PlayerAction m_PlayerAction;
    private void Awake()
    {
        m_MaxSlotSize = Utils.MAX_INVENTORYSLOT_SIZE;
        m_Slots = new ItemSlot[m_MaxSlotSize];
    }

    public void CheckItem()
    {
        for (int i = 0; i < m_SlotSize; i++)
        {
            if (m_Slots[i] == null) continue;
            if (m_Slots[i].quantity <= 0) m_Slots[i] = null;
        }
    }

    public override bool TrySetSlot(ItemSlot slot)
    {
        for (int i = 0; i < m_SlotSize; i++)
        {
            if (m_Slots[i] == null)
            {
                m_Slots[i] = slot;
                return true;
            }
        }

        return false;
    }

}


