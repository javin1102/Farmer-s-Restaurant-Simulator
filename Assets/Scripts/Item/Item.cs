using UnityEngine;
public abstract class Item : MonoBehaviour
{
    public IRaycastAction ItemRaycastAction { get => m_ItemRaycastAction; }
    public ItemData Data { get => m_Data; }

    [SerializeField] private ItemData m_Data;

    protected IRaycastAction m_ItemRaycastAction;

    protected void OnEnable()
    {
        TryGetComponent( out m_ItemRaycastAction );
    }
    public abstract void MainAction();
}

public enum ItemType
{
    Equipment,
    Ingredient,
    Furniture,
    Seed
}