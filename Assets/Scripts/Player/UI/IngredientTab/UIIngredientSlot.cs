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
    private void OnEnable()
    {
        if ( m_IngredientData == null ) return;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if ( m_RestaurantManager == null )
            m_RestaurantManager = RestaurantManager.Instance;

        m_IngredientNameText.text = m_IngredientData.id;
        m_Icon.sprite = m_IngredientData.icon;
        if ( m_RestaurantManager.StockIngredients.TryGetValue( m_IngredientData.id, out StockIngredient ingredient ) )
        {
            EnableUI( ingredient );
        }
        else
        {
            DisableUI();
        }
    }

    private void EnableUI( StockIngredient ingredient)
    {
        m_OverlayGO.SetActive( false );
        m_QuantityText.text = ingredient.quantity.ToString();
    }

    private void DisableUI()
    {
        m_OverlayGO.SetActive( true );
        m_QuantityText.text = "0";
    }
}
