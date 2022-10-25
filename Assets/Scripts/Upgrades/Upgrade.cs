using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    public abstract class Upgrade : MonoBehaviour
    {
        [SerializeField] protected int m_Cost;
        [SerializeField] protected Button m_Button;
        [SerializeField] protected TMP_Text m_LevelText, m_CostText;
        protected int m_CurrentLevel;
        protected int m_MaxLevel;
        protected void Start()
        {
            m_Button.onClick.AddListener( DoUpgrade );
            SetMaxLevel();
            SetCurrentLevel();
        }
        protected void DoUpgrade()
        {
            if ( m_CurrentLevel >= m_MaxLevel ) return;
            UpgradeFeature();
            m_CurrentLevel += 1;
            m_LevelText.text = $"{m_CurrentLevel}/{m_MaxLevel}";
            m_CostText.text = $"Cost: <sprite index=0>{m_Cost}";
        }

        protected abstract void UpgradeFeature();
        protected abstract void SetMaxLevel();
        protected abstract void SetCurrentLevel();
    }

}
