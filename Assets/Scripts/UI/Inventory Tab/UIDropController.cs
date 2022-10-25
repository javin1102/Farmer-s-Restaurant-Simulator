using System;

public class UIDropController : UIExtraController
{
    protected override void Action()
    {
        m_ItemDatabase.Decrease( m_UIInventoryController.SelectedSlot.Slot.data, m_Quantity, ItemDatabaseAction.DROP );
    }
}
