using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName ="Custom Data/New Recipe")]
public class RecipeData : ScriptableObject
{
    public GameObject foodPrefab;
    public int price;
    public float cookDuration;
    public List<RecipeIngredient> ingredients = new();
}

[Serializable]
public class RecipeIngredient
{
    public ItemData ingredient;
    public int quantity;
}