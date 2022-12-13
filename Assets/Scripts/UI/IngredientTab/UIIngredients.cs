using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIIngredients : MonoBehaviour
{
    private List<IngredientData> m_Ingredients;
    [SerializeField] private GameObject m_MainContent, m_SlotPrefab;
    [SerializeField] private UIIngredientTooltip m_Tooltip;
    private readonly List<UIIngredientSlot> m_IngredientSlots = new();
    private ResourcesLoader m_ResourcesLoader;
    private void Awake()
    {
        m_ResourcesLoader = ResourcesLoader.Instance;
        m_Ingredients = m_ResourcesLoader.IngredientsData;
        m_Ingredients.ForEach(ingredientData =>
        {
            UIIngredientSlot slot = Instantiate(m_SlotPrefab, m_MainContent.transform).GetComponent<UIIngredientSlot>();
            slot.IngredientData = ingredientData;
            m_IngredientSlots.Add(slot);
        });
    }

}
