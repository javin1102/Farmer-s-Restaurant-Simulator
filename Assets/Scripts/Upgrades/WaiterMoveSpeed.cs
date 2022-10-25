using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Upgrades
{
    public class WaiterMoveSpeed : Upgrade
    {
        [SerializeField] private RestaurantUpgradesChannel m_RestaurantUpgradesChannel;

        protected override void UpgradeFeature()
        {
            m_RestaurantUpgradesChannel.RaiseIncreaseWaiterSpeedEvent( .5f );
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

