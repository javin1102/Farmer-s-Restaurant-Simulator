using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public abstract class ItemSlotsController : MonoBehaviour
{
    public Dictionary<string, ItemSlot> ItemSlotsDictionary => m_ItemSlotsDictionary;
    public event UnityAction<ItemSlot> OnStoreNewItem;
    public event UnityAction OnStoreExistingItem;
    [SerializeReference] protected readonly Dictionary<string, ItemSlot> m_ItemSlotsDictionary = new( );



    public void InvokeStoreNewItemEvent( ItemSlot itemSlot ) => OnStoreNewItem?.Invoke( itemSlot );
    public void InvokeStoreExistingItemEvent( ) => OnStoreExistingItem?.Invoke();
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



    //protected abstract bool Store(ItemData itemData);
    public abstract bool Store( ItemData itemData );

}
