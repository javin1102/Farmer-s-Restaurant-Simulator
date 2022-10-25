using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRecipeCard : MonoBehaviour
{
    public KeyValuePair<FoodData, FoodConfig> Food { get => m_Food; set => m_Food = value; }

    private KeyValuePair<FoodData, FoodConfig> m_Food;
    [SerializeField] private GameObject m_IngredientPrefab;
    [SerializeField] private GameObject m_IngredientContent, m_LockedOverlay;
    [SerializeField] private Button m_UnlockedButton, m_SellButton;
    [SerializeField] private Image m_FoodIcon;
    [SerializeField] private TMP_Text m_CookDurText, m_DishPriceText, m_UnlockedPriceText, m_FoodNameText;

    private TMP_Text m_SellButtonText;
    private Color m_RedColor, m_GreenColor;
    private Image m_SellButtonImage;

    private void Start()
    {
        foreach ( var ingredient in m_Food.Key.ingredients )
        {
            UIRecipeIngredient uiIngredient = Instantiate( m_IngredientPrefab, m_IngredientContent.transform ).GetComponent<UIRecipeIngredient>();
            uiIngredient.FoodIngredient = ingredient;
        }
        m_RedColor = new( 0.772549f, 0.2588235f, 0.2980392f );
        m_GreenColor = new( 0.4941176f, 0.8039216f, 0.4862745f );
        m_SellButtonText = m_SellButton.transform.GetChild(0).GetComponent<TMP_Text>();
        m_SellButtonImage = m_SellButton.GetComponent<Image>();
        m_SellButton.onClick.AddListener( SellHandler );
    }

    private void SellHandler()
    {
        m_Food.Value.IsSelling = !m_Food.Value.IsSelling;
    }

    private void Update() => UpdateUI();
    public void UpdateUI()
    {
        UpdateOverlayUI();
        UpdateSellButtonUI();
        m_FoodIcon.sprite = m_Food.Key.icon;
        m_CookDurText.text = $"{m_Food.Key.cookDuration} Seconds";
        m_DishPriceText.text = $"{m_Food.Key.dishPrice}/Dish";
        m_UnlockedPriceText.text = $"Unlock For <color=yellow>${m_Food.Key.unlockPrice}</color>";
        m_FoodNameText.text = m_Food.Key.name;
    }

    private void UpdateOverlayUI()
    {
        if ( m_Food.Value.IsUnlock ) m_LockedOverlay.SetActive( false );
        else m_LockedOverlay.SetActive( true );
    }

    private void UpdateSellButtonUI()
    {
        if ( m_Food.Value.IsSelling )
        {
            m_SellButtonImage.color = m_RedColor;
            m_SellButtonText.text = "Batal";
        }
        else
        {
            m_SellButtonImage.color = m_GreenColor;
            m_SellButtonText.text = "Jual";
        }
    }
}
