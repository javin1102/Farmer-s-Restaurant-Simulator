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

        protected override void UpgradeFeature()
        {
            int slotSize = m_PlayerUpgrades.GetSlotSize();
            for (int i = 0; i < slotSize; i++) m_UIInventoryLayout.transform.GetChild(i).gameObject.SetActive(true);
        }

        protected override void SetMaxLevel()
        {
            m_MaxLevel = 4;
        }
    }
}
