using UnityEngine;
using UnityEngine.Events;


public class ActionSlotsController : ItemSlotsController
{
    public Item CurrEquippedItem { get => m_CurrEquippedItem; }
    public UnityAction<string> DBRemoveAction { get => m_DBRemoveAction; set => m_DBRemoveAction = value; }
    public UnityAction<int> OnSelectSlot { get; set; }
    [SerializeField] private Transform m_Hand;
    [SerializeField] private GameObject m_HandModel;
    [SerializeField] private ItemMainActionChannel m_DeacreasableItemChannel;
    private Item m_CurrEquippedItem;
    private int m_SelectedSlotIndex;
    private UnityAction<string> m_DBRemoveAction;

    private void Awake()
    {
        m_MaxSlotSize = 6;
        m_SlotSize = 6;
        m_Slots = new ItemSlot[m_MaxSlotSize];
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
        UIManager.Instance.HideActionHelper();
        Item instantiatedItem = Instantiate( item.data.prefab, m_Hand ).GetComponent<Item>();
        ResetItemTf( instantiatedItem );
        instantiatedItem.gameObject.SetActive( true );
        return instantiatedItem;
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


    private void ResetItemTf( Item item )
    {
        item.transform.parent = m_Hand;
        item.SetHandTf();
    }

    public void SelectActionSlot( int index )
    {
        m_SelectedSlotIndex = index;
        OnSelectSlot?.Invoke( index );
    }


    public void SelectActionSlot_1() => SelectActionSlot( 0 );
    public void SelectActionSlot_2() => SelectActionSlot( 1 );
    public void SelectActionSlot_3() => SelectActionSlot( 2 );
    public void SelectActionSlot_4() => SelectActionSlot( 3 );
    public void SelectActionSlot_5() => SelectActionSlot( 4 );
    public void SelectActionSlot_6() => SelectActionSlot( 5 );

    public void CheckEquippedItem()
    {
        if ( m_Slots[m_SelectedSlotIndex] == null || m_Slots[m_SelectedSlotIndex].quantity <= 0 )
        {
            if ( m_CurrEquippedItem == null ) return;
            m_CurrEquippedItem = null;
            m_Slots[m_SelectedSlotIndex] = null;
            m_HandModel.SetActive( true );
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
            m_HandModel.SetActive( false );
        }

    }
}


