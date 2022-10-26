using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIStoreItem : MonoBehaviour
{
    public ItemData ItemData { get => m_ItemData; set => m_ItemData = value; }
    protected ItemData m_ItemData;
    [SerializeField] protected TMP_Text m_NameText, m_PriceText;
    [SerializeField] protected Image m_Icon;
    [SerializeField] protected Button m_BuyButton;


    protected void Start()
    {
        UpdateUI();
        m_BuyButton.onClick.AddListener( BuyItem );
    }

    protected abstract void BuyItem();
    protected void UpdateUI()
    {
        m_NameText.text = m_ItemData.id;
        m_PriceText.text = $"<sprite=0> {m_ItemData.buyPrice}";
        m_Icon.sprite = m_ItemData.icon;
    }

}
