using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Upgrades
{
    public class ExpandRestaurant : Upgrade
    {
        protected override int m_CurrentLevel { get => m_PlayerUpgrades.RestaurantExpandLevel; set => m_PlayerUpgrades.RestaurantExpandLevel = value; }

        protected override void SetMaxLevel()
        {
            m_MaxLevel = 3;
        }

        protected override void UpgradeFeature()
        {
        }
    }
}

