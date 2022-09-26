using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventorySlotsController : ItemSlotsController
{
    private void Start()
    {
        m_Slots = new ItemSlot[42];
    }

    public override bool TrySetSlot( ItemSlot slot )
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


