using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISellController : UIExtraController
{
    [SerializeField] private TMP_Text m_SellPriceText;
    protected override void ButtonAction()
    {
        m_ItemDatabase.Decrease( m_UIInventoryController.SelectedSlot.Slot.data, m_Quantity, ItemDatabaseAction.SELL );
    }
    private void OnEnable()
    {
        m_SellPriceText.text = "Harga:<indent=40%><sprite=0><color=yellow>0</color>";
    }
    private new void OnDisable()
    {
        base.OnDisable();
        m_SellPriceText.text = "Harga:<indent=40%><sprite=0><color=yellow>0</color>";
    }

    protected override void Validate( string arg0 )
    {
        base.Validate( arg0 );
        if ( m_UIInventoryController.SelectedSlot == null ) return;
        int price = m_UIInventoryController.SelectedSlot.Slot.data.sellPrice * m_Quantity;
        m_SellPriceText.text = $"Harga:<indent=40%><sprite=0><color=yellow>{price}</color>";
    }
}
