using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Upgrades
{
    public class ChefQuantity : Upgrade
    {
        [SerializeField] private RestaurantUpgradesChannel m_RestaurantUpgradesChannel;

        protected override int m_CurrentLevel { get => m_PlayerUpgrades.ChefQuantityLevel; set => m_PlayerUpgrades.ChefQuantityLevel = value; }

        protected override void UpgradeFeature()
        {
            m_RestaurantUpgradesChannel.RaiseAddChefEvent();
        }

        protected override void SetMaxLevel()
        {
            m_MaxLevel = 3;
        }

    }

}
