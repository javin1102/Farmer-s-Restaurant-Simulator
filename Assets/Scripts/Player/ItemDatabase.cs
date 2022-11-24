using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ItemDatabaseAction
{
    DROP, SELL
}
public class ItemDatabase : MonoBehaviour
{
    public UnityAction<ItemSlot> OnStoreItem { get => m_OnStoreItem; set => m_OnStoreItem = value; }
    public Dictionary<string, ItemSlot> InventorySlots => m_InventoryDB;
    [SerializeReference] private readonly Dictionary<string, ItemSlot> m_InventoryDB = new();
    private event UnityAction<ItemSlot> m_OnStoreItem;
    private ActionSlotsController m_ActionSlots;
    private InventorySlotsController m_InventorySlots;
    private int MaxSize { get => m_ActionSlots.SlotSize + m_InventorySlots.SlotSize; }
    private ResourcesLoader m_ResourcesLoader;
    private void Start()
    {
        m_ActionSlots = GetComponent<ActionSlotsController>();
        m_InventorySlots = GetComponent<InventorySlotsController>();
        m_ActionSlots.DBRemoveAction += RemoveFromDB;
        m_ResourcesLoader = ResourcesLoader.Instance;
        m_ResourcesLoader.StarterPackData.ForEach( item => Store( item, 5 ) );
    }
    private void OnDestroy()
    {
        m_ActionSlots.DBRemoveAction -= RemoveFromDB;
    }
    private void RemoveFromDB( string id ) => m_InventoryDB.Remove( id );
    public bool Store( ItemData itemData, int quantity = 1 )
    {
        if ( m_InventoryDB.Count >= MaxSize ) return false;
        
        if ( m_InventoryDB.TryGetValue( itemData.id, out ItemSlot slot ) )
        {
            if ( !itemData.decreaseable ) quantity = 0;
            slot.quantity += quantity;
            m_OnStoreItem?.Invoke( slot );
            return true;
        }
        else
        {
            if ( !itemData.decreaseable ) quantity = 1;
            ItemSlot itemSlot = new( itemData, quantity );
            m_InventoryDB.Add( itemData.id, itemSlot );
            m_OnStoreItem?.Invoke( itemSlot );
            if ( m_ActionSlots.TrySetSlot( itemSlot ) ) return true;
            if ( m_InventorySlots.TrySetSlot( itemSlot ) ) return true;
            return false;
        }
    }
    public void Decrease( ItemData itemData, int quantity, ItemDatabaseAction action )
    {
        if ( action == ItemDatabaseAction.DROP && !itemData.decreaseable ) return;
        if ( m_InventoryDB.TryGetValue( itemData.id, out ItemSlot slot ) )
        {
            slot.quantity -= quantity;
            DoAction( itemData, quantity, action );

            if ( slot.quantity <= 0 )
            {
                m_InventoryDB.Remove( itemData.id );
            }

        }
    }

    private void DoAction( ItemData itemData, int quantity, ItemDatabaseAction action )
    {
        switch ( action )
        {
            case ItemDatabaseAction.DROP:
                Drop( itemData, quantity );
                break;
            case ItemDatabaseAction.SELL:
                break;
            default:
                break;
        }
    }

    private void Drop( ItemData itemData, int quantity )
    {
        Item droppedItem = Instantiate( itemData.prefab, transform.position, transform.rotation ).GetComponent<Item>();
        droppedItem.DropState( quantity );
    }
}


