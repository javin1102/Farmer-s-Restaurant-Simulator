using System;

public class UIDropController : UIExtraController
{
    protected override void ButtonAction()
    {
        m_ItemDatabase.Decrease( m_UIInventoryController.SelectedSlot.Slot.data, m_Quantity, ItemDatabaseAction.DROP );
    }
}
