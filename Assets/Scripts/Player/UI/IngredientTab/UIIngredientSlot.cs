using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIngredientSlot : MonoBehaviour
{
    public ItemData IngredientData { get => m_IngredientData; set => m_IngredientData = value; }
    [SerializeField] private Image m_Icon;
    [SerializeField] private TMP_Text m_QuantityText, m_IngredientNameText;
    [SerializeField] private GameObject m_OverlayGO;
    private ItemData m_IngredientData;
    private RestaurantManager m_RestaurantManager;
    private void Start()
    {
        m_RestaurantManager = RestaurantManager.Instance;
    }

    private void Update() => UpdateUI();

    public void UpdateUI()
    {
        m_IngredientNameText.text = m_IngredientData.id;
        m_Icon.sprite = m_IngredientData.icon;
        if ( m_RestaurantManager.StockIngredients.TryGetValue( m_IngredientData.id, out StockIngredient ingredient ) )
        {
            EnableOverlayUI( ingredient );
        }
        else
        {
            DisableOverlayUI();
        }
    }

    private void EnableOverlayUI( StockIngredient ingredient)
    {
        m_OverlayGO.SetActive( false );
        m_QuantityText.text = ingredient.quantity.ToString();
    }

    private void DisableOverlayUI()
    {
        m_OverlayGO.SetActive( true );
        m_QuantityText.text = "0";
    }
}
