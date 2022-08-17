using UnityEngine;
using UnityEngine.InputSystem;
public class ItemSlotsController : MonoBehaviour
{
    public Old.ItemSlot CurrentSelectedSlot { get => m_ItemSlots[m_SelectedSlotIndex]; }

    [SerializeField] private Old.ItemSlot[] m_ItemSlots = new Old.ItemSlot[4];
    [SerializeField] private Transform m_Hand;
    [SerializeField] private ItemMainActionChannel m_DeacreasableItemChannel;

    private int m_SelectedSlotIndex;

    private void OnEnable()
    {
        m_DeacreasableItemChannel.OnMainAction += OnSelectedItemMainAction;
    }

    private void OnDisable()
    {
        m_DeacreasableItemChannel.OnMainAction -= OnSelectedItemMainAction;
    }

    /**
    * <summary>
    * Store item to slot with specific rule
    * </summary>
    */
    public void Store( Item item )
    {
        GameObject prefab = item.Data.prefab;

        // If Current selected slot is null -> Store to Current slot
        if ( CurrentSelectedSlot.prefab == null )
        {
            CurrentSelectedSlot.prefab = prefab;
            CurrentSelectedSlot.quantity += 1;
            AssignAndUseCurrentSlot( item );
        }

        // Current selected slot is not null
        else
        {
            //Current selected slot prefab equals raycasted prefab
            if ( prefab == CurrentSelectedSlot.prefab )
            {
                CurrentSelectedSlot.quantity += 1;
                //CurrentSelectedSlot.ItemRef.Select();
            }
            else
            {
                for ( int i = 0; i < m_ItemSlots.Length; i++ )
                {
                    //Find if have the same prefab
                    if ( m_ItemSlots[i].prefab == prefab )
                    {
                        m_ItemSlots[i].quantity += 1;
                        break;
                    }

                    //Find empty slot
                    else if ( m_ItemSlots[i].prefab == null )
                    {
                        m_ItemSlots[i].quantity += 1;
                        m_ItemSlots[i].prefab = prefab;
                        AssignItemToSlot( item, i );
                        InstantiateAndAssignItemRefToSlot( i );
                        break;
                    }
                }
            }

            Destroy( item.gameObject );
        }

    }

    public void ItemSlotSelection( InputAction.CallbackContext context )
    {
        Vector2 value = context.ReadValue<Vector2>();
        if ( value == Vector2.left )
        {
            SelectSlot( 0 );
        }
        else if ( value == Vector2.right )
        {
            SelectSlot( 1 );
        }
        else if ( value == Vector2.down )
        {
            SelectSlot( 2 );
        }
        else
        {
            SelectSlot( 3 );
        }
    }
    public void SelectSlot( int index )
    {
        //Null check Unuse old copy
        TryUnselectOldItemSlot();

        //Select new slot 
        m_SelectedSlotIndex = index;

        //Null check and use new copy
        TrySelectCurrentItemSlot();


    }
    /**
    * <summary>
    * Instantiate all assigned slot prefab to Hand Tranform and assign all slot item reference to instantiated object
    * </summary>
    */
    public void InstantiateAndAssignItemRefToSlot()
    {
        for ( int i = 0; i < m_ItemSlots.Length; i++ )
        {
            InstantiateAndAssignItemRefToSlot( i );
        }
    }

    /**
    * <summary>
    * Instantiate selected item slot prefab to Hand Tranform and assign slot item reference to instantiated object
    * </summary>
    */
    public void InstantiateAndAssignItemRefToSlot( int index )
    {
        GameObject prefab = m_ItemSlots[index].prefab;
        if ( prefab != null )
        {
            GameObject go = Instantiate( prefab );
            ResetItemTf( go.transform );
            go.SetActive( false );
            m_ItemSlots[index].ItemRef = go.GetComponent<Item>();
        }
    }

    private void TrySelectCurrentItemSlot()
    {
        if ( CurrentSelectedSlot.ItemRef == null )
        {
            return;
        }

        CurrentSelectedSlot.ItemRef.Select();
    }

    private void TryUnselectOldItemSlot()
    {
        if ( CurrentSelectedSlot.ItemRef == null )
        {
            return;
        }

        CurrentSelectedSlot.ItemRef.Unselect();
    }

    private void AssignAndUseCurrentSlot( Item raycastedItem )
    {
        AssignItemToSlot( raycastedItem, m_SelectedSlotIndex );
        CurrentSelectedSlot.ItemRef.Select();
    }

    private void AssignItemToSlot( Item raycastedItem, int index )
    {
        m_ItemSlots[index].prefab = raycastedItem.Data.prefab;
        m_ItemSlots[index].ItemRef = raycastedItem;
        ResetItemTf( raycastedItem.transform );
    }

    private void ResetItemTf( Transform raycastedItemTf )
    {
        raycastedItemTf.parent = m_Hand;
        raycastedItemTf.localPosition = Vector3.zero;
        raycastedItemTf.localScale = Vector3.one;
        raycastedItemTf.localRotation = Quaternion.identity;
        raycastedItemTf.gameObject.layer = 11;
    }

    private void OnSelectedItemMainAction()
    {
        DecreaseSlotQuantity();
        CheckQuantityAndInstatiateCopyToSlot();
    }
    private void DecreaseSlotQuantity()
    {
        CurrentSelectedSlot.quantity -= 1;
        if ( CurrentSelectedSlot.quantity <= 0 )
        {
            CurrentSelectedSlot.prefab = null;
            CurrentSelectedSlot.ItemRef = null;
        }

    }

    /**
     * <summary>
     * Instantiate new object from current selected slot if current slot quantity is > 0
     * </summary>
     */
    private void CheckQuantityAndInstatiateCopyToSlot()
    {
        if ( CurrentSelectedSlot.ItemRef == null ) return;
        if ( CurrentSelectedSlot.quantity > 0 )
        {
            CurrentSelectedSlot.ItemRef.Unselect();
            GameObject go = Instantiate( CurrentSelectedSlot.prefab );
            ResetItemTf( go.transform );
            CurrentSelectedSlot.ItemRef = go.GetComponent<Item>();
            CurrentSelectedSlot.ItemRef.Select();

        }
    }

}

namespace Old {
    [System.Serializable]
    public class ItemSlot
    {
        public GameObject prefab;
        public Item ItemRef;
        public int quantity;
    }
}


