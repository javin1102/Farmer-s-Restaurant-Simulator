using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ActionSlotsController : ItemSlotsController
{
    public Item CurrEquippedItem { get => m_CurrEquippedItem; }
    public UnityAction<string> DBRemoveAction { get => m_DBRemoveAction; set => m_DBRemoveAction = value; }

    [SerializeField] private Transform m_Hand;
    [SerializeField] private ItemMainActionChannel m_DeacreasableItemChannel;
    private Item m_CurrEquippedItem;
    private int m_SelectedSlotIndex;
    private UnityAction<string> m_DBRemoveAction;

    private void Awake()
    {
        m_Slots = new ItemSlot[6];
    }
    private void Start()
    {
        m_DeacreasableItemChannel.OnMainAction += DecreaseQuantity;
    }

    private void OnDestroy()
    {
        m_DeacreasableItemChannel.OnMainAction -= DecreaseQuantity;
    }

    public override bool TrySetSlot( ItemSlot slot )
    {
        if ( m_Slots[m_SelectedSlotIndex] == null )
        {
            m_Slots[m_SelectedSlotIndex] = slot;
            return true;
        }
        else
        {
            for ( int i = 0; i < m_Slots.Length; i++ )
            {
                if ( m_Slots[i] == null )
                {
                    m_Slots[i] = slot;
                    return true;
                }
            }

            return false;
        }

    }
    private void Update()
    {
        CheckEquippedItem();
    }
    private Item InstantiateItemToHand( ItemSlot item )
    {
        GameObject go = Instantiate( item.data.prefab, m_Hand );
        ResetItemTf( go.transform );
        go.SetActive( true );
        return go.GetComponent<Item>();
    }

    private void DestroyAllItemsInHand()
    {
        foreach ( Transform child in m_Hand )
        {
            Destroy( child.gameObject );
        }
    }


    private void DecreaseQuantity()
    {
        m_Slots[m_SelectedSlotIndex].quantity -= 1;
        if ( m_Slots[m_SelectedSlotIndex].quantity <= 0 )
        {

            m_DBRemoveAction?.Invoke( m_Slots[m_SelectedSlotIndex].data.id );
            m_Slots[m_SelectedSlotIndex] = null;
            m_CurrEquippedItem = null;
            DestroyAllItemsInHand();
        }
    }


    private void ResetItemTf( Transform item )
    {
        item.parent = m_Hand;
        item.localPosition = Vector3.zero;
        item.localScale = Vector3.one;
        item.localRotation = Quaternion.identity;
        item.gameObject.layer = Utils.HandLayer;
    }

    public void SelectActionSlot( int index )
    {
        m_SelectedSlotIndex = index;
    }


    public void SelectActionSlot_1() => m_SelectedSlotIndex = 0;
    public void SelectActionSlot_2() => m_SelectedSlotIndex = 1;
    public void SelectActionSlot_3() => m_SelectedSlotIndex = 2;
    public void SelectActionSlot_4() => m_SelectedSlotIndex = 3;
    public void SelectActionSlot_5() => m_SelectedSlotIndex = 4;
    public void SelectActionSlot_6() => m_SelectedSlotIndex = 5;

    public void CheckEquippedItem()
    {
        if ( m_Slots[m_SelectedSlotIndex] == null  )
        {
            if ( m_CurrEquippedItem == null ) return;
            m_CurrEquippedItem = null; 
            DestroyAllItemsInHand();
        }
        else
        {
            if ( m_CurrEquippedItem == null )
            {
                m_CurrEquippedItem = InstantiateItemToHand( m_Slots[m_SelectedSlotIndex] );
            }
            else
            {
                if ( m_CurrEquippedItem.Data.id != m_Slots[m_SelectedSlotIndex].data.id )
                {
                    DestroyAllItemsInHand();
                    m_CurrEquippedItem = InstantiateItemToHand( m_Slots[m_SelectedSlotIndex] );
                }
            }
           
        }

    }
}


