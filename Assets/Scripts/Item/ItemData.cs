using UnityEngine;
[CreateAssetMenu( fileName = "Item", menuName = "New Item" )]
public class ItemData : ScriptableObject
{
    public new string name;
    public GameObject prefab;
    public Sprite icon;
    public ItemType type;
}
