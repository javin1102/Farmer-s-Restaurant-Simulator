using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Upgrades
{
    public class WaiterQuantity : Upgrade
    {
        [SerializeField] private RestaurantUpgradesChannel m_RestaurantUpgrades;
        protected override void UpgradeFeature()
        {
            m_RestaurantUpgrades.RaiseAddWaiterEvent();
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

