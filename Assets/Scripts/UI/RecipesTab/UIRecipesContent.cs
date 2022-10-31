using UnityEngine;
using UnityEngine.UI;

public class UIRecipesContent : MonoBehaviour
{
    [SerializeField] private GameObject m_CardPrefab;
    private HorizontalLayoutGroup m_HorizontalLayoutGroup;
    private RectTransform m_RectTransform;
    private FoodsController m_FoodsController;
    private void Start()
    {
        m_HorizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        m_RectTransform = GetComponent<RectTransform>();
        m_FoodsController = FoodsController.Instance;
        foreach ( var food in m_FoodsController.AllFoods )
        {
            UIRecipeCard card = Instantiate( m_CardPrefab, transform ).GetComponent<UIRecipeCard>();
            card.Food = food;
        }
    }


    private void Update()
    {
        m_RectTransform.sizeDelta = new Vector2( m_HorizontalLayoutGroup.preferredWidth, 380 );
    }
}
