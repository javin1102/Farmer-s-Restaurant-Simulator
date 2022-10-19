using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEditor.Search;
using System;

public class UIIngredientsStock : MonoBehaviour
{
    private List<ItemData> m_Ingredients;
    [SerializeField] private GameObject m_MainContent, m_SlotPrefab;
    [SerializeField] private TMP_InputField m_SearchField;
    private readonly List<UIIngredientSlot> m_IngredientSlots = new();
    private PlayerAction m_PlayerAction;
    
    private void Awake()
    {
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();   
        m_Ingredients = Resources.LoadAll<ItemData>( "Data/Ingredients" ).ToList();
        m_Ingredients.ForEach( ingredientData =>
        {
            UIIngredientSlot slot = Instantiate( m_SlotPrefab, m_MainContent.transform ).GetComponent<UIIngredientSlot>();
            slot.IngredientData = ingredientData;
            m_IngredientSlots.Add( slot );
        } );

        m_SearchField.onValueChanged.AddListener( OnSearch );
        m_SearchField.onSelect.AddListener( OnSelectSearchBar );
        m_SearchField.onDeselect.AddListener( OnDeselectSearchBar );
    }

    private void OnDeselectSearchBar( string arg0 )
    {
        m_PlayerAction.InventoryAction.Enable();
    }

    private void OnSelectSearchBar( string arg0 )
    {
        m_PlayerAction.InventoryAction.Disable();
    }

    private void OnSearch( string searchText )
    {
        StopAllCoroutines();
        if ( String.IsNullOrEmpty( searchText ) )
        {
            m_IngredientSlots.ForEach( slot => slot.gameObject.SetActive( true ) );
            return;
        }

        StartCoroutine( Search( searchText ) );

    }
    IEnumerator Search( string searchText )
    {
        yield return WaitSearch( searchText );
        //Finish search
    }
    IEnumerator WaitSearch( string searchText )
    {
        //Start Searching
        yield return new WaitUntil( () =>
        {
            m_IngredientSlots.ForEach( slot =>
            {
                long score = 0;
                if ( !FuzzySearch.FuzzyMatch( searchText, slot.IngredientData.id, ref score ) )
                {
                    slot.gameObject.SetActive( false );
                }
                else
                {
                    slot.gameObject.SetActive( true );
                }
            } );
            return true;
        } );
    }
}
