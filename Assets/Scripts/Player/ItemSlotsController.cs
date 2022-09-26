using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ItemSlotsController : MonoBehaviour
{
    public ItemSlot[] Slots { get => m_Slots; }
    [SerializeReference] protected ItemSlot[] m_Slots;
    public abstract bool TrySetSlot( ItemSlot slot );
}


