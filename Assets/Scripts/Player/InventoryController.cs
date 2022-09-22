using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
public class InventoryController : ItemSlotsController
{
    public event UnityAction OnEnableInventoryUI;
    public event UnityAction OnDisableInventoryUI;


    [SerializeField] private ItemData s;
    [SerializeField] private ItemData x;


    public void InvokeEnableInventoryUIEvent() => OnEnableInventoryUI?.Invoke();
    public void InvokeDisableInventoryUIEvent() => OnDisableInventoryUI?.Invoke();


    [ContextMenu( "Store S" )]
    public void StoreS() => Store( s );
    [ContextMenu( "Store X" )]
    public void StoreX() => Store( x );

    private void Start()
    {
        StoreS();
        StoreS();
        StoreS();
        StoreX();
        StoreX();
    }

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
}
