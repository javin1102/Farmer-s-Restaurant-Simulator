using UnityEngine;
using UnityEngine.Events;

public abstract class ItemSlotsController : MonoBehaviour
{
    public ItemSlot[] ItemSlots { get => m_ItemSlots; }

    [SerializeReference] protected ItemSlot[] m_ItemSlots;
    public event UnityAction OnStoreItem;

    public void SetSlotItem( ItemSlot itemSlotData, int index ) => m_ItemSlots[index] = itemSlotData;
    public void SwapSlotItem( int index1, int index2 ) => (m_ItemSlots[index1], m_ItemSlots[index2]) = (m_ItemSlots[index2], m_ItemSlots[index1]);

    /**
     *<summary>Return true if store is successfull</summary>
     **/

    public bool Store(Item item )
    {
        if ( Store( item.Data ) )
        {
            Destroy( item.gameObject );
            return true;
        }

        return false;
    }


    protected bool Store( ItemData itemData )
    {
        int length = m_ItemSlots.Length;
        for ( int i = 0; i < length; i++ )
        {
            if ( m_ItemSlots[i] == null )
            {
                ItemSlot itemSlotData = new( itemData );
                m_ItemSlots[i] = itemSlotData;
                OnStoreItem?.Invoke();
                return true;
            }
            else
            {
                if ( m_ItemSlots[i].data == itemData )
                {
                    m_ItemSlots[i].quantity += 1;
                    OnStoreItem?.Invoke();
                    return true;
                }
            }

        }

        return false;
    }
}
