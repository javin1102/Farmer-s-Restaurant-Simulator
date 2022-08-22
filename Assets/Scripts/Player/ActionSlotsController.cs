using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ActionSlotsController : ItemSlotsController
{
    public event UnityAction OnDropItem;
    public Item CurrEquippedItem { get => m_CurrEquippedItem; }
    public ItemSlot[] ActionSlots { get => m_ActionSlots; }
    public int SelectedSlotIndex { get => m_SelectedSlotIndex; }

    [SerializeField] private Transform m_Hand;
    [SerializeField] private ItemMainActionChannel m_DeacreasableItemChannel;
    [SerializeField] private ItemData s;
    [SerializeField] private Item m_CurrEquippedItem;
    [SerializeReference] private ItemSlot[] m_ActionSlots;

    private int m_SelectedSlotIndex;
    public void InvokeUIDropItemEvent() => OnDropItem?.Invoke();

    private void Awake()
    {
        m_ActionSlots = new ItemSlot[6];
    }
    private void OnEnable()
    {
        OnStoreNewItem += _ => CheckEquippedItem();
        m_DeacreasableItemChannel.OnMainAction += CheckQuantity;
    }

    private void OnDisable()
    {
        m_DeacreasableItemChannel.OnMainAction -= CheckQuantity;
        OnStoreNewItem -= _ => CheckEquippedItem();
    }

    public void SelectActionSlot( int index )
    {
        m_SelectedSlotIndex = index;
        DestroyAllItemsInHand();
        m_CurrEquippedItem = null;
        if ( m_ActionSlots[index] == null ) return;
        m_CurrEquippedItem = InstantiateItemToHand( m_ActionSlots[index] );
    }

    [ContextMenu( "Store S" )]
    public void StoreS() => Store( s );
    public override bool Store( ItemData itemData )
    {
        if ( m_ItemSlotsDictionary.Count >= 6 ) return false;
        if ( m_ItemSlotsDictionary.TryGetValue( itemData.id, out ItemSlot slot ) )
        {
            slot.quantity += 1;
            InvokeStoreExistingItemEvent();
            return true;
        }
        else
        {
            ItemSlot itemSlotData = new( itemData );
            m_ItemSlotsDictionary.Add( itemData.id, itemSlotData );

            //Todo::Drop curr item
            m_ActionSlots[m_SelectedSlotIndex] = itemSlotData;
            InvokeStoreNewItemEvent( itemSlotData );
            return true;
        }

    }

    public void CheckEquippedItem()
    {
        DestroyAllItemsInHand();
        if ( m_ActionSlots[m_SelectedSlotIndex] == null ) return;

        //if ( m_CurrEquippedItem != null && m_CurrEquippedItem.Data == m_ItemSlots[m_SelectedSlotIndex].data ) return;
        m_CurrEquippedItem = InstantiateItemToHand( m_ActionSlots[m_SelectedSlotIndex] );
    }

    private Item InstantiateItemToHand( ItemSlot item )
    {
        GameObject go = Instantiate( item.data.prefab, m_Hand );
        ResetItemTf( go.transform );
        go.SetActive( true );
        return go.GetComponent<Item>();
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
        m_ActionSlots[m_SelectedSlotIndex].quantity -= 1;
        if ( m_ActionSlots[m_SelectedSlotIndex].quantity <= 0 )
        {

            m_ItemSlotsDictionary.Remove( m_ActionSlots[m_SelectedSlotIndex].data.id );
            m_ActionSlots[m_SelectedSlotIndex] = null;
            m_CurrEquippedItem = null;
            DestroyAllItemsInHand();
        }
    }


}
