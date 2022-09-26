using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName ="Custom Data/New Food")]
public class FoodData : ScriptableObject
{
    public GameObject foodPrefab;
    public int price;
    public float cookDuration;
    public List<FoodIngredient> ingredients = new();
}

[Serializable]
public class FoodIngredient
{
    public ItemData ingredient;
    public int quantity;
}