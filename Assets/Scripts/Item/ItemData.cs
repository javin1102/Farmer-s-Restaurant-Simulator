using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Custom Data/New Item")]
public class ItemData : ScriptableObject
{
    [Header("--- ITEM DATA")]
    public string ID;
    public GameObject prefab;
    public Sprite icon;
    //public ItemType type;
    public int buyPrice;
    public int sellPrice;
    public bool decreaseable;
    public ItemType itemType;
}

public enum ItemType
{
    EQUIPMENT,
    INGREDIENT,
    FURNITURE,
    SEED,
    CROP,
}