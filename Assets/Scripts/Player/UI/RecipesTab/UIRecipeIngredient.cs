using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRecipeIngredient : MonoBehaviour
{
    public FoodIngredient FoodIngredient { get => m_FoodIngredient; set => m_FoodIngredient = value; }
    [SerializeField] private Image m_Icon;
    [SerializeField] private TMP_Text m_QtyText;
    [SerializeField] private Image m_QtySlider;

    private FoodIngredient m_FoodIngredient;
    private RestaurantManager m_RestaurantManager;
    private StockIngredient m_Stock;

    private void OnEnable()
    {
        if ( m_FoodIngredient == null ) return;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if ( m_RestaurantManager == null )
            m_RestaurantManager = RestaurantManager.Instance;
        m_Stock ??= m_RestaurantManager.StockIngredients[m_FoodIngredient.ingredient.id];
        m_Icon.sprite = m_FoodIngredient.ingredient.icon;
        m_QtyText.text = $"{m_Stock.quantity}/{m_FoodIngredient.quantity}";
    }
}
