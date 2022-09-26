using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
public class InventoryController : ItemSlotsController
{
    public UnityAction OnDropItem { get => m_OnDropItem; set => m_OnDropItem = value; }
    public UnityAction ToggleUIAction { get => m_ToggleUIAction; set => m_ToggleUIAction = value; }
    private UnityAction m_ToggleUIAction;

    private event UnityAction m_OnDropItem;
    public void ToggleUI() => m_ToggleUIAction?.Invoke();
    public override bool Store( ItemData itemData )
    {
        if ( m_ItemSlotsDictionary.TryGetValue( itemData.id, out ItemSlot slot ) )
        {
            slot.quantity += 1;
            InvokeStoreExistingItemEvent();
            return true;
        }
        if ( m_ItemSlotsDictionary.Count >= 42 ) return false;
        else
        {
            ItemSlot itemSlotData = new( itemData );
            m_ItemSlotsDictionary.Add( itemData.id, itemSlotData );
            InvokeStoreNewItemEvent( -1, itemSlotData );
            return true;
        }
    }

    public void Drop( ItemData itemData, int quantity )
    {
        if ( m_ItemSlotsDictionary.TryGetValue( itemData.id, out ItemSlot slot ) )
        {
            if ( slot.quantity < quantity ) return;
            slot.quantity -= quantity;

            for ( int i = 0; i < quantity; i++ )
            {
                Item droppedItem = Instantiate( itemData.prefab, transform.position, transform.rotation ).GetComponent<Item>();
                droppedItem.DropState();
            }

            if ( slot.quantity <= 0 ) m_ItemSlotsDictionary.Remove( itemData.id );
        }
    }
}
