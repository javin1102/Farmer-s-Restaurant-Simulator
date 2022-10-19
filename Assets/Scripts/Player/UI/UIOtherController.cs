using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOtherController : MonoBehaviour
{
    public static int TabIndex { get => m_TabIndex; set => m_TabIndex = value; }
    [SerializeField] private GameObject m_UI;
    [SerializeField] private UIMenu m_RecipesMenu;
    [SerializeField] private UIMenu m_UpgradesMenu;
    [SerializeField] private UIMenu m_InventoryMenu;
    [SerializeField] private UIMenu m_IngredientsStockMenu;
    [SerializeField] private UIMarker m_Marker;
    [SerializeField] private Button m_CloseButton;
    [SerializeField] private UITab[] m_Tabs;
    private PlayerAction m_PlayerAction;
    private static int m_TabIndex = 1;

    private void Awake()
    {
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
        m_CloseButton.onClick.AddListener( CloseUI );
    }
    private void OnEnable()
    {
        m_PlayerAction.ToggleUI += ToggleUI;
        m_PlayerAction.ToggleInventoryUI += ToggleInventory;
        m_PlayerAction.OnDisableUI += SelectDefaultTab;
        m_RecipesMenu.SelectAction += SelectRecipesTab;
        m_InventoryMenu.SelectAction += SelectInventoryTab;
        m_IngredientsStockMenu.SelectAction += SelectIngredientsStockTab;
        m_UpgradesMenu.SelectAction += SelectUpgradesTab;
    }
    private void OnDisable()
    {
        m_PlayerAction.OnDisableUI -= SelectDefaultTab;
        m_PlayerAction.ToggleInventoryUI -= ToggleInventory;
        m_PlayerAction.ToggleUI -= ToggleUI;
        m_RecipesMenu.SelectAction -= SelectRecipesTab;
        m_InventoryMenu.SelectAction -= SelectInventoryTab;
        m_IngredientsStockMenu.SelectAction -= SelectIngredientsStockTab;
        m_UpgradesMenu.SelectAction -= SelectUpgradesTab;
    }

    private void ToggleUI()
    {
        m_UI.SetActive( !m_UI.activeInHierarchy );
        m_PlayerAction.IsUIOpen = m_UI.activeInHierarchy;
        if ( m_UI.activeInHierarchy )
        {
            m_PlayerAction.OnEnableUI?.Invoke();
            SelectDefaultTab();
        }
        else m_PlayerAction.OnDisableUI?.Invoke();
    }

    private void ToggleInventory()
    {
        ToggleUI();
        if ( !m_UI.activeInHierarchy ) return;
        SelectTab( "Inventory", 0 );
    }

    private void SelectDefaultTab() => SelectTab( "Recipes", 0 );
    private void SelectRecipesTab() => SelectTab( "Recipes" );
    private void SelectInventoryTab() => SelectTab( "Inventory" );
    private void SelectUpgradesTab() => SelectTab( "Upgrades" );
    private void SelectIngredientsStockTab() => SelectTab( "Ingredients Stock" );
    private void CloseUI()
    {
        m_UI.SetActive( false );
        m_PlayerAction.IsUIOpen = false;
        m_PlayerAction.OnDisableUI?.Invoke();
    }
    private void SelectTab( string name, float dur = 0.25f )
    {
        foreach ( UITab tab in m_Tabs )
        {
            tab.gameObject.SetActive( false );
            if ( tab.Name.Equals( name ) )
            {
                tab.gameObject.SetActive( true );
                m_TabIndex = tab.transform.GetSiblingIndex();
                m_Marker.TweenTo( tab.Menu.transform.localPosition.x, dur );
            }
        }

    }
}
