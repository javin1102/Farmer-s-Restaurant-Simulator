using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public int ChefQuantityLevel { get; set; }
    public int RestaurantExpandLevel { get; set; }
    public int InventoryLevel { get; set; }
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
}
