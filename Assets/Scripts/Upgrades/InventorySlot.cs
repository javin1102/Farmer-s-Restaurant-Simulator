using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Upgrades
{
    public class InventorySlot : Upgrade
    {
        private InventorySlotsController m_InventorySlotsController;
        [SerializeField] private GameObject m_UIInventoryLayout;

        protected override int m_CurrentLevel { get => m_PlayerUpgrades.InventoryLevel; set => m_PlayerUpgrades.InventoryLevel = value; }
        private void Start()
        {
            m_InventorySlotsController = transform.root.GetComponent<InventorySlotsController>();
            UpgradeFeature();
        }
        private int GetSlotSize()
        {
            float tValue = Mathf.InverseLerp(1, m_MaxLevel, m_PlayerUpgrades.InventoryLevel);
            int slotSize = Mathf.RoundToInt(Mathf.Lerp(20, 42, tValue));
            return slotSize;
        }

        protected override void UpgradeFeature()
        {
            int slotSize = GetSlotSize();
            m_InventorySlotsController.SlotSize = slotSize;
            for (int i = 0; i < slotSize; i++) m_UIInventoryLayout.transform.GetChild(i).gameObject.SetActive(true);
        }

        protected override void SetMaxLevel()
        {
            m_MaxLevel = 4;
        }
    }
}
