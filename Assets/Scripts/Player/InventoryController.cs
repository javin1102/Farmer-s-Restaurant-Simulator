using UnityEngine;
using UnityEngine.Events;
public class InventoryController : ItemSlotsController
{
    [SerializeField] private ItemData s;
    [SerializeField] private ItemData x;

    public event UnityAction OnEnableInventoryUI;
    public event UnityAction OnDisableInventoryUI;

    private void Awake()
    {
        Store( s );
        Store( s );
        Store( x );
        Store( x );
        Store( s );
    }

    public void InvokeEnableInventoryUIEvent() => OnEnableInventoryUI?.Invoke();
    public void InvokeDisableInventoryUIEvent() => OnDisableInventoryUI?.Invoke();

    [ContextMenu( "Store S" )]
    public void StoreS() => Store( s );
    [ContextMenu( "Store X" )]
    public void StoreX() => Store( x );
}
