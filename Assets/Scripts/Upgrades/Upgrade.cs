using System;
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
        abstract protected int m_CurrentLevel { get; set; }
        protected int m_MaxLevel;
        protected PlayerUpgrades m_PlayerUpgrades;
        void Awake()
        {
            m_PlayerUpgrades = transform.root.GetComponent<PlayerUpgrades>();
            m_Button.onClick.AddListener(DoUpgrade);
            SetMaxLevel();
        }
        void OnEnable()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            m_LevelText.text = $"{m_CurrentLevel}/{m_MaxLevel}";
            m_CostText.text = $"Cost: <sprite index=0>{m_Cost}";
        }


        protected void DoUpgrade()
        {
            if (m_CurrentLevel >= m_MaxLevel) return;
            m_CurrentLevel += 1;
            UpgradeFeature();
            UpdateUI();
        }

        protected abstract void UpgradeFeature();
        protected abstract void SetMaxLevel();
    }

}
