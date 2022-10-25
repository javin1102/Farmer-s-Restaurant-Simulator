using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISellController : UIExtraController
{
    protected override void Action()
    {
        m_ItemDatabase.Decrease( m_UIInventoryController.SelectedSlot.Slot.data, m_Quantity, ItemDatabaseAction.SELL );
    }
}
