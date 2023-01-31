using UnityEngine;

[CreateAssetMenu(fileName = "Seed", menuName = "New Seed")]
public class SeedData : ItemData
{
    [Header("--- CROP ---")]
    public GameObject cropPrefab;
    public float dropChance;
    public int hourToGrow;
    public int minSeedDropQuantity;
    public int maxSeedDropQuanitty;
    [Header("--- HARVERSTED")]
    public IngredientData harvestedIngredientData;
    public int minIngredientDropQuantity;
    public int maxIngredientDropQuantity;
}
