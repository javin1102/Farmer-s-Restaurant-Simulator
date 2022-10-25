using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Upgrades
{
    public class InventorySlot : Upgrade
    {
        private InventorySlotsController m_InventorySlotsController;
        [SerializeField] private GameObject m_UIInventoryLayout;
        private new void Start()
        {
            base.Start();
            m_InventorySlotsController = transform.root.GetComponent<InventorySlotsController>();
        }


        protected override void UpgradeFeature()
        {
            int initSize = m_InventorySlotsController.SlotSize;
            if ( m_CurrentLevel != m_MaxLevel ) m_InventorySlotsController.SlotSize += 7;
            else m_InventorySlotsController.SlotSize += 8;

            for ( int i = initSize; i < m_InventorySlotsController.SlotSize; i++ )
            {
                m_UIInventoryLayout.transform.GetChild( i ).gameObject.SetActive( true );
            }
        }

        protected override void SetMaxLevel()
        {
            m_MaxLevel = 3;
        }

        protected override void SetCurrentLevel()
        {
            m_CurrentLevel = 1;
        }
    }
}
