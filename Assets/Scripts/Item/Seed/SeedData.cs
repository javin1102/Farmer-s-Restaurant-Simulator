using UnityEngine;

[CreateAssetMenu(fileName ="Seed", menuName ="New Seed")]
public class SeedData : ItemData
{
    [Header("--- CROP ---")]
    public GameObject cropPrefab;
    public float dropChance;

    [Header("--- HARVERSTED")]
    public ItemData harverstedCropData;
}
