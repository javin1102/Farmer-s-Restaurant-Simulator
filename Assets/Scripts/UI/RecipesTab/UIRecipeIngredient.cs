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
    private FoodsController m_FoodsController;


    private void Update() => UpdateUI();
    private void Start()
    {
        m_FoodsController = FoodsController.Instance;
        
    }
    public void UpdateUI()
    {
        m_Icon.sprite = m_FoodIngredient.ingredient.icon;
        
        if ( m_FoodsController.StockIngredients.TryGetValue( m_FoodIngredient.ingredient.id, out StockIngredient stockIngredient ) )
        {
            m_QtyText.text = $"{stockIngredient.quantity}/{m_FoodIngredient.quantity}";
            m_QtySlider.fillAmount = Mathf.InverseLerp( 0, m_FoodIngredient.quantity, stockIngredient.quantity );
            m_QtyText.alpha = 1f;
        }
        else
        {
            m_QtyText.text = $"{0}/{m_FoodIngredient.quantity}";
            m_QtySlider.fillAmount = 0;
            m_QtyText.alpha = .5f;
        }

    }
}
