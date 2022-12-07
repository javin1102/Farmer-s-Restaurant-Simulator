using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SimpleJSON;
public enum ItemDatabaseAction
{
    DROP, SELL
}
public class ItemDatabase : MonoBehaviour
{
    public UnityAction<ItemSlot> OnStoreItem { get => m_OnStoreItem; set => m_OnStoreItem = value; }
    public Dictionary<string, ItemSlot> ItemDB => m_ItemDB;
    [SerializeReference] private readonly Dictionary<string, ItemSlot> m_ItemDB = new();
    private event UnityAction<ItemSlot> m_OnStoreItem;
    private ActionSlotsController m_ActionSlots;
    private InventorySlotsController m_InventorySlots;
    private int MaxSize { get => m_ActionSlots.SlotSize + m_InventorySlots.SlotSize; }
    private ResourcesLoader m_ResourcesLoader;
    public enum SET_SLOT_TYPE
    {
        AUTO,
        INVENTORY,
        ACTION_SLOTS
    }


    private void Start()
    {
        m_ActionSlots = GetComponent<ActionSlotsController>();
        m_InventorySlots = GetComponent<InventorySlotsController>();
        m_ActionSlots.DBRemoveAction += RemoveFromDB;
        m_ResourcesLoader = ResourcesLoader.Instance;
    }


    private void OnDestroy()
    {
        m_ActionSlots.DBRemoveAction -= RemoveFromDB;
    }
    private void RemoveFromDB(string id) => m_ItemDB.Remove(id);
    public bool Store(ItemData itemData, int quantity = 1, int slotIndex = -1, SET_SLOT_TYPE setSlotType = SET_SLOT_TYPE.AUTO)
    {
        if (m_ItemDB.Count >= MaxSize) return false;

        if (m_ItemDB.TryGetValue(itemData.ID, out ItemSlot slot))
        {
            if (!itemData.decreaseable) quantity = 0;
            slot.quantity += quantity;
            m_OnStoreItem?.Invoke(slot);
            return true;
        }
        else
        {
            if (!itemData.decreaseable) quantity = 1;
            ItemSlot itemSlot = new(itemData, quantity);
            m_ItemDB.Add(itemData.ID, itemSlot);
            m_OnStoreItem?.Invoke(itemSlot);
            switch (setSlotType)
            {
                case SET_SLOT_TYPE.AUTO:
                    if (m_ActionSlots.TrySetSlot(itemSlot)) return true;
                    if (m_InventorySlots.TrySetSlot(itemSlot)) return true;
                    break;
                case SET_SLOT_TYPE.INVENTORY:
                    m_InventorySlots.Slots[slotIndex] = itemSlot;
                    break;
                case SET_SLOT_TYPE.ACTION_SLOTS:
                    m_ActionSlots.Slots[slotIndex] = itemSlot;
                    break;
            }
            return true;
        }
    }

    public void Decrease(ItemData itemData, int quantity, ItemDatabaseAction action)
    {
        if (action == ItemDatabaseAction.DROP && !itemData.decreaseable) return;
        if (m_ItemDB.TryGetValue(itemData.ID, out ItemSlot slot))
        {
            slot.quantity -= quantity;
            OnAction(itemData, quantity, action);

            if (slot.quantity <= 0)
            {
                m_ItemDB.Remove(itemData.ID);
            }

        }
    }

    private void OnAction(ItemData itemData, int quantity, ItemDatabaseAction action)
    {
        switch (action)
        {
            case ItemDatabaseAction.DROP:
                Drop(itemData, quantity);
                break;
            case ItemDatabaseAction.SELL:
                PlayerAction.Coins += itemData.sellPrice;
                break;
            default:
                break;
        }
    }

    private void Drop(ItemData itemData, int quantity)
    {
        Item droppedItem = Instantiate(itemData.prefab, transform.position, transform.rotation).GetComponent<Item>();
        droppedItem.DropState(quantity);
    }

    public (JSONArray, JSONArray) ToJSON()
    {
        JSONArray inventoryJsonArray = new();
        JSONArray actionSlotsJsonArray = new();

        for (int i = 0; i < m_InventorySlots.SlotSize; i++)
        {
            ItemSlot slot = m_InventorySlots.Slots[i];
            if (slot == null) continue;
            inventoryJsonArray.Add(new SerializableItemSlotData(slot, i).Serialize());
        }

        for (int i = 0; i < m_ActionSlots.SlotSize; i++)
        {
            ItemSlot slot = m_ActionSlots.Slots[i];
            if (slot == null) continue;
            actionSlotsJsonArray.Add(new SerializableItemSlotData(slot, i).Serialize());
        }

        return (inventoryJsonArray, actionSlotsJsonArray);
    }

    public void OnLoadSucceded(JSONNode jsonNode)
    {
        JSONNode inventoryNode = jsonNode["inventory"];
        JSONNode actionSlotsNode = jsonNode["actionSlots"];
        foreach (var node in inventoryNode)
        {
            SerializableItemSlotData serializableItemSlotData = new(node);
            StoreItemFromSaveData(serializableItemSlotData, SET_SLOT_TYPE.INVENTORY);
        }

        foreach (var node in actionSlotsNode)
        {
            SerializableItemSlotData serializableItemSlotData = new(node);
            StoreItemFromSaveData(serializableItemSlotData, SET_SLOT_TYPE.ACTION_SLOTS);
        }
    }

    public void OnLoadFailed()
    {
        m_ResourcesLoader.StarterPackData.ForEach(item => Store(item, 5));
    }

    private void StoreItemFromSaveData(SerializableItemSlotData serializableItemSlotData, SET_SLOT_TYPE setSlotType)
    {
        ItemType itemType = (ItemType)serializableItemSlotData.type;
        ItemData itemData = null;
        switch (itemType)
        {
            case ItemType.EQUIPMENT:
                itemData = m_ResourcesLoader.GetEquipmentDataByID(serializableItemSlotData.ID);
                break;
            case ItemType.INGREDIENT:
                itemData = m_ResourcesLoader.GetIngredientDataByID(serializableItemSlotData.ID);
                break;
            case ItemType.FURNITURE:
                itemData = m_ResourcesLoader.GetFurnitureDataByID(serializableItemSlotData.ID);
                break;
            case ItemType.SEED:
                itemData = m_ResourcesLoader.GetSeedDataByID(serializableItemSlotData.ID);
                break;
            case ItemType.CROP:
                itemData = m_ResourcesLoader.GetCropDataByID(serializableItemSlotData.ID);
                break;
        }
        Store(itemData, serializableItemSlotData.quantity, serializableItemSlotData.slotIndex, setSlotType);
    }
}


