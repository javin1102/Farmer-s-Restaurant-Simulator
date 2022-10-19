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


    private void Update() => UpdateUI();
    private void Start()
    {
        m_RestaurantManager = RestaurantManager.Instance;
    }
    public void UpdateUI()
    {
        m_Icon.sprite = m_FoodIngredient.ingredient.icon;
        if ( m_RestaurantManager.StockIngredients.TryGetValue( m_FoodIngredient.ingredient.id, out StockIngredient stockIngredient ) )
        {
            m_QtyText.text = $"{stockIngredient.quantity}/{m_FoodIngredient.quantity}";
        }
        else
        {
            m_QtyText.text = $"{0}/{m_FoodIngredient.quantity}";
        }

    }
}
