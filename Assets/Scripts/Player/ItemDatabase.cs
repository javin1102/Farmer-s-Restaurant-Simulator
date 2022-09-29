using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ItemDatabase : MonoBehaviour
{
    public UnityAction<ItemSlot> OnStoreItem { get => m_OnStoreItem; set => m_OnStoreItem = value; }
    public Dictionary<string, ItemSlot> InventorySlots => m_InventoryDB;
    [SerializeReference] private readonly Dictionary<string, ItemSlot> m_InventoryDB = new();
    private event UnityAction<ItemSlot> m_OnStoreItem;
    private ActionSlotsController m_ActionSlots;
    private InventorySlotsController m_InventorySlots;
    private void Start()
    {
        m_ActionSlots = GetComponent<ActionSlotsController>();
        m_InventorySlots = GetComponent<InventorySlotsController>();
        m_ActionSlots.DBRemoveAction += RemoveFromDB;
        ItemData[] x = Resources.LoadAll<ItemData>( "Data" );
        foreach ( var item in x )
        {
            Debug.Log( item.name );
            Store( item );
            Store( item );
            Store( item );
            Store( item );
        }
    }
    private void OnDestroy()
    {
        m_ActionSlots.DBRemoveAction -= RemoveFromDB;
    }
    private void RemoveFromDB( string id ) => m_InventoryDB.Remove( id );
    public bool Store( ItemData itemData )
    {
        if ( m_InventoryDB.Count >= 48 ) return false;
        if ( m_InventoryDB.TryGetValue( itemData.id, out ItemSlot slot ) )
        {
            slot.quantity += 1;
            m_OnStoreItem?.Invoke( slot );
            return true;
        }
        else
        {
            ItemSlot itemSlot = new( itemData );
            m_InventoryDB.Add( itemData.id, itemSlot );
            m_OnStoreItem?.Invoke( itemSlot );
            if ( m_ActionSlots.TrySetSlot( itemSlot ) ) return true;
            if ( m_InventorySlots.TrySetSlot( itemSlot ) ) return true;
            return false;
        }
    }
    public void Drop( ItemData itemData, int quantity )
    {
        if ( m_InventoryDB.TryGetValue( itemData.id, out ItemSlot slot ) )
        {
            slot.quantity -= quantity;
            for ( int i = 0; i < quantity; i++ )
            {
                Item droppedItem = Instantiate( itemData.prefab, transform.position, transform.rotation ).GetComponent<Item>();
                droppedItem.DropState();
            }

            if ( slot.quantity <= 0 ) m_InventoryDB.Remove( itemData.id );
        }
    }
}


