using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class UIActionSlot : UIItemSlot
    {
        private void Awake()
        {
            m_SlotsController = transform.root.GetComponent<ActionSlotsController>();
        }
    }


