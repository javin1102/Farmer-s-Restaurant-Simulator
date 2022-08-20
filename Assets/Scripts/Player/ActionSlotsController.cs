using UnityEngine;
using UnityEngine.Events;
public class ActionSlotsController : ItemSlotsController
{
    public Item CurrEquippedItem { get => m_CurrEquippedItem; }
    [SerializeField] private Transform m_Hand;
    [SerializeField] private ItemMainActionChannel m_DeacreasableItemChannel;
    [SerializeField] private ItemData s;

    private int m_SelectedSlotIndex;
    private Item m_CurrEquippedItem;
    private InventoryController m_InventoryController;


    private void OnEnable()
    {
        m_InventoryController = GetComponent<InventoryController>();
        m_InventoryController.OnDisableInventoryUI += CheckEquippedItem;
        OnStoreItem += CheckEquippedItem;
        m_DeacreasableItemChannel.OnMainAction += CheckQuantity;
    }

    private void OnDisable()
    {
        m_InventoryController.OnDisableInventoryUI -= CheckEquippedItem;
        m_DeacreasableItemChannel.OnMainAction -= CheckQuantity;
        OnStoreItem -= CheckEquippedItem;
    }

    public void SelectActionSlot( int index )
    {
        m_SelectedSlotIndex = index;
        DestroyAllItemsInHand();
        m_CurrEquippedItem = null;
        if ( m_ItemSlots[index] == null ) return;
        m_CurrEquippedItem = InstantiateItemToHand( m_ItemSlots[index] );
    }


    private Item InstantiateItemToHand( ItemSlot item )
    {
        GameObject go = Instantiate( item.data.prefab, m_Hand );
        ResetItemTf( go.transform );
        go.SetActive( true );
        return go.GetComponent<Item>();
    }

    private void CheckEquippedItem()
    {
        if ( m_ItemSlots[m_SelectedSlotIndex] == null ) return;

        //if ( m_CurrEquippedItem != null && m_CurrEquippedItem.Data == m_ItemSlots[m_SelectedSlotIndex].data ) return;
        DestroyAllItemsInHand();
        m_CurrEquippedItem = InstantiateItemToHand( m_ItemSlots[m_SelectedSlotIndex] );
    }

    private void ResetItemTf( Transform item )
    {
        item.parent = m_Hand;
        item.localPosition = Vector3.zero;
        item.localScale = Vector3.one;
        item.localRotation = Quaternion.identity;
        item.gameObject.layer = Utils.HandLayer;
    }


    private void DestroyAllItemsInHand()
    {
        foreach ( Transform child in m_Hand )
        {
            Destroy( child.gameObject );
        }
    }

    private void CheckQuantity()
    {
        m_ItemSlots[m_SelectedSlotIndex].quantity -= 1;
        if ( m_ItemSlots[m_SelectedSlotIndex].quantity <= 0 )
        {
            m_ItemSlots[m_SelectedSlotIndex] = null;
            m_CurrEquippedItem = null;
            DestroyAllItemsInHand();
        }
    }
    [ContextMenu( "Store S" )]
    public void StoreS() => Store( s );
}
