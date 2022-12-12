using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public int ChefQuantityLevel { get; set; }
    public int RestaurantExpandLevel { get; set; }
    public int InventoryLevel { get; set; }
    public int INVENTORY_MAX_LEVEL { get => 3; }
    private InventorySlotsController m_InventorySlotsController;
    void Start()
    {
        m_InventorySlotsController = GetComponent<InventorySlotsController>();
    }
    public void Reset()
    {
        ChefQuantityLevel = 1;
        RestaurantExpandLevel = 1;
        InventoryLevel = 1;
    }
    public void Set(int chefQuantityLevel, int restaurantLevel, int inventoryLevel)
    {
        ChefQuantityLevel = chefQuantityLevel;
        RestaurantExpandLevel = restaurantLevel;
        InventoryLevel = inventoryLevel;
    }

    public int GetSlotSize()
    {
        float tValue = Mathf.InverseLerp(1, INVENTORY_MAX_LEVEL, InventoryLevel);
        int slotSize = Mathf.RoundToInt(Mathf.Lerp(20, 42, tValue));
        m_InventorySlotsController.SlotSize = slotSize;
        return slotSize;
    }


}
