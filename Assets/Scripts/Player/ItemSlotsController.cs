using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ItemSlotsController : MonoBehaviour
{
    public ItemSlot[] Slots { get => m_Slots; }
    public int SlotSize { get => m_SlotSize; set => m_SlotSize =  value ; }

    [SerializeReference] protected ItemSlot[] m_Slots;
    [SerializeField] protected int m_SlotSize;
    protected int m_MaxSlotSize;
    public abstract bool TrySetSlot( ItemSlot slot );

}


