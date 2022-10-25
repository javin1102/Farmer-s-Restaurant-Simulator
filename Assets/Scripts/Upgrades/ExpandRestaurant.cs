using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Upgrades
{
    public class ExpandRestaurant : Upgrade
    {
        protected override void SetCurrentLevel()
        {
            m_CurrentLevel = 1;
        }

        protected override void SetMaxLevel()
        {
            m_MaxLevel = 3;
        }

        protected override void UpgradeFeature()
        {
        }
    }
}

