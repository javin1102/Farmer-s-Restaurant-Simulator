using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class SerializablePlayerUpgradesData : ISerializable
{
    public int chefQuantityLevel;
    public int restaurantExpandLevel;
    public int inventoryLevel;

    public SerializablePlayerUpgradesData(int chefQuantityLevel, int restaurantExpandLevel, int inventoryLevel)
    {
        this.chefQuantityLevel = chefQuantityLevel;
        this.restaurantExpandLevel = restaurantExpandLevel;
        this.inventoryLevel = inventoryLevel;
    }

    public SerializablePlayerUpgradesData(PlayerUpgrades playerUpgrades)
    {
        this.chefQuantityLevel = playerUpgrades.ChefQuantityLevel;
        this.restaurantExpandLevel = playerUpgrades.RestaurantExpandLevel;
        this.inventoryLevel = playerUpgrades.InventoryLevel;
    }

    public SerializablePlayerUpgradesData(JSONNode jsonNode)
    {
        this.chefQuantityLevel = jsonNode["chefQuantityLevel"];
        this.restaurantExpandLevel = jsonNode["restaurantExpandLevel"];
        this.inventoryLevel = jsonNode["inventoryLevel"];
    }

    public JSONObject Serialize()
    {
        JSONObject jsonObject = new();
        jsonObject.Add("chefQuantityLevel", this.chefQuantityLevel);
        jsonObject.Add("restaurantExpandLevel", this.restaurantExpandLevel);
        jsonObject.Add("inventoryLevel", this.inventoryLevel);
        return jsonObject;
    }
}
