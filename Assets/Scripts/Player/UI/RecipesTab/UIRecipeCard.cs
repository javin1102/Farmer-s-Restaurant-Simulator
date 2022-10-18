using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRecipeCard : MonoBehaviour
{
    public KeyValuePair<FoodData, bool> Food { get => m_Food; set => m_Food = value; }

    private KeyValuePair<FoodData, bool> m_Food;
    [SerializeField] private GameObject m_IngredientPrefab;
    [SerializeField] private GameObject m_IngredientContent, m_LockedOverlay;
    [SerializeField] private Image m_FoodIcon;
    [SerializeField] private TMP_Text m_CookDurText, m_DishPriceText, m_UnlockedPriceText, m_FoodNameText;
    private void Start()
    {
        foreach ( var ingredient in m_Food.Key.ingredients )
        {
            UIRecipeIngredient uiIngredient =  Instantiate( m_IngredientPrefab, m_IngredientContent.transform ).GetComponent<UIRecipeIngredient>();
            uiIngredient.FoodIngredient = ingredient;
            uiIngredient.UpdateUI();
        }
    }

    private void OnEnable()
    {
        if ( m_Food.Key == null ) return;
        UpdateUI();
    }
    public void UpdateUI()
    {
        if ( m_Food.Value ) m_LockedOverlay.SetActive( false );
        else m_LockedOverlay.SetActive( true );
        m_FoodIcon.sprite = m_Food.Key.icon;
        m_CookDurText.text = $"{m_Food.Key.cookDuration} Seconds";
        m_DishPriceText.text = $"{m_Food.Key.dishPrice}/Dish";
        m_UnlockedPriceText.text = $"Unlock For <color=yellow>{m_Food.Key.unlockPrice}</color>";
    }
}
