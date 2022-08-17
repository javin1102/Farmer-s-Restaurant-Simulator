using UnityEngine;
public abstract class Item : MonoBehaviour
{
    public IRaycastAction ItemRaycastAction { get => m_ItemRaycastAction; }
    public ItemData Data { get => m_Data; }

    [SerializeField] private ItemData m_Data;

    protected IRaycastAction m_ItemRaycastAction;
    protected bool m_IsSelected = false;

    protected void OnEnable()
    {
        TryGetComponent( out m_ItemRaycastAction );
    }
    public abstract void MainAction();
    public void Unselect()
    {
        m_IsSelected = false;
        gameObject.SetActive( false );
    }
    public void Select()
    {
        m_IsSelected = true;
        gameObject.SetActive( true );
        TryGetComponent( out m_ItemRaycastAction );
    }
}

public enum ItemType
{
    Equipment,
    Ingredient,
    Furniture,
    Seed
}