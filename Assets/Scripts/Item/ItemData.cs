using UnityEngine;
[CreateAssetMenu( fileName = "Item", menuName = "Custom Data/New Item" )]
public class ItemData : ScriptableObject
{
    public string id;
    public GameObject prefab;
    public Sprite icon;
    public ItemType type;
}
