using UnityEngine;

[CreateAssetMenu(fileName ="Seed", menuName ="New Seed")]
public class SeedData : ScriptableObject
{
    public ItemData itemData;
    public new string cropId;
    public GameObject cropPrefab;

}
