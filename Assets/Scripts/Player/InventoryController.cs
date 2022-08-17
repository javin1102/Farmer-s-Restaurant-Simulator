using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class InventoryController : MonoBehaviour
{
    private readonly Dictionary<string, ItemSlotData> m_InventoryDictionary = new();
    public Dictionary<string, ItemSlotData> InventoryDictionary => m_InventoryDictionary;
    public List<ItemSlotData> InventoryItemList => m_InventoryDictionary.Values.ToList();
    private int m_MaxItem = 42;

    public event UnityAction<ItemSlotData> OnStoreNewItem;
    public event UnityAction OnStoreExistingItem;
    [SerializeField] private ItemData s;
    [SerializeField] private ItemData x;
    private void Start()
    {
        Store( s );
        Store( s );
        Store( x );
        Store( x );
        Store( s );
    }
    public void Store( Item item )
    {
        string itemKey = item.Data.name;

        //If get key successful, simply increase quantity
        if ( m_InventoryDictionary.TryGetValue( itemKey, out ItemSlotData existingSlot ) )
        {
            existingSlot.quantity += 1;
            OnStoreExistingItem?.Invoke();
        }
        else
        {
            if ( InventoryItemList.Count >= m_MaxItem ) return;
            //There's no key in dictionary, add new slot to dictionary
            ItemSlotData itemSlotData = new( item.Data );
            m_InventoryDictionary.Add( itemKey, itemSlotData );
            OnStoreNewItem?.Invoke( itemSlotData );
        }
    }
    public void Store( ItemData item )
    {
        string itemKey = item.name;

        //If get key successful, simply increase quantity
        if ( m_InventoryDictionary.TryGetValue( itemKey, out ItemSlotData existingSlot ) )
        {
            existingSlot.quantity += 1;
            OnStoreExistingItem?.Invoke();
            print( "Store Existing" );
        }
        else
        {
            if ( InventoryItemList.Count >= m_MaxItem ) return;
            //There's no key in dictionary, add new slot to dictionary
            ItemSlotData itemSlotData = new( item );
            m_InventoryDictionary.Add( itemKey, itemSlotData );
            OnStoreNewItem?.Invoke( itemSlotData );
            print( "Store New" );
        }
    }

    [ContextMenu("Store S")]
    public void StoreS() => Store( s );
}
