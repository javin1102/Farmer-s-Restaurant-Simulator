using System;
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
        abstract protected int m_MaxLevel { get; }
        protected PlayerUpgrades m_PlayerUpgrades;
        protected PlayerAction m_PlayerAction;
        void Awake()
        {
            m_PlayerUpgrades = transform.root.GetComponent<PlayerUpgrades>();
            m_Button.onClick.AddListener(DoUpgrade);
            m_PlayerAction = PlayerAction.Instance;
        }
        void OnEnable()
        {
            UpdateUI();
        }


        private int CalculateCost()
        {
            return Mathf.RoundToInt(Mathf.Lerp(m_Cost, m_Cost * m_CurrentLevel, Mathf.InverseLerp(1, m_MaxLevel, m_CurrentLevel)));
        }

        private void UpdateUI()
        {
            if (m_CurrentLevel >= m_MaxLevel)
            {
                m_LevelText.text = $"{m_CurrentLevel}/{m_MaxLevel}";
                m_CostText.text = $"Max";
                m_Button.interactable = false;
            }
            else
            {
                m_LevelText.text = $"{m_CurrentLevel}/{m_MaxLevel}";
                m_CostText.text = $"Harga: <sprite index=0>{CalculateCost()}";
            }



        }


        protected void DoUpgrade()
        {
            int cost = CalculateCost();
            if (m_CurrentLevel >= m_MaxLevel || m_PlayerAction.Coins < cost) return;
            m_CurrentLevel += 1;
            m_PlayerAction.Coins -= cost;
            UpgradeFeature();
            UpdateUI();
            m_PlayerAction.PlayAudio("button_sfx");
        }

        protected abstract void UpgradeFeature();
    }

}
