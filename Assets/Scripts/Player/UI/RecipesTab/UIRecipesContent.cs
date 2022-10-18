using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRecipesContent : MonoBehaviour
{
    [SerializeField] private GameObject m_CardPrefab;
    private HorizontalLayoutGroup m_HorizontalLayoutGroup;
    private RectTransform m_RectTransform;
    private RestaurantManager m_Restaurant;
    private void Start()
    {
        m_HorizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        m_RectTransform = GetComponent<RectTransform>();
        m_Restaurant = RestaurantManager.Instance;
        foreach ( var food in m_Restaurant.AllFoods )
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
