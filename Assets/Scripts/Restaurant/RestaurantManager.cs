using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager Instance { get => m_Instance; }
    public List<Table> Tables { get => m_Tables; }
    public List<Seat> UnoccupiedSeats { get => m_Seats; }

    //Furniture
    [SerializeField] private List<Table> m_Tables = new();
    [SerializeField] private List<Seat> m_Seats = new();
    private int RandomSeatIndex { get => Random.Range( 0, m_Seats.Count ); }

    //Food
    private readonly Dictionary<RecipeData, bool> m_AllRecipes = new();
    private readonly List<RecipeData> m_UnlockedRecipes = new();
    private readonly Dictionary<string, StockIngredient> m_StockIngredients = new();

    //Others
    private static RestaurantManager m_Instance;
    private bool m_FirstLoad = true;

    //Debug
    [SerializeField] private ItemData garlic;
    [SerializeField] private ItemData onion;
    [SerializeField] private ItemData carrot;

    private void Awake()
    {
        if ( m_Instance == null ) m_Instance = this;
        else Destroy( gameObject );

        //Check if player has save file
        if ( m_FirstLoad ) LoadRecipeData();
        m_AllRecipes.ToList().ForEach( AddUnlockRecipeToList );
        StockIngredient garlicStock = new( garlic, 10 );
        StockIngredient onionStock = new( onion, 10 );
        StockIngredient carrotStock = new( carrot, 10 );
        m_StockIngredients.Add( garlic.id, garlicStock );
        m_StockIngredients.Add( onion.id, onionStock );
        m_StockIngredients.Add( carrot.id, carrotStock );
        Debug.Log( GetRecipeToCook() );

    }
    public bool FindUnoccupiedSeat( out Seat seat )
    {
        if ( m_Seats.Count == 0 )
        {
            seat = null;
            return false;
        }

        seat = m_Seats[RandomSeatIndex];
        return true;
    }
    public void OrderFood()
    {
        RecipeData food = GetRecipeToCook();
        //Add to chef queue
    }
    public void StoreIngredient( ItemData itemData )
    {
        if ( itemData.type != ItemType.Ingredient ) return;
        if ( m_StockIngredients.TryGetValue( itemData.id, out StockIngredient stockIngredient ) )
        {
            stockIngredient.quantity += 1;
        }
        else
        {
            stockIngredient = new StockIngredient( itemData );
            m_StockIngredients.Add( itemData.id, stockIngredient );
        }
    }
    private void LoadRecipeData()
    {
        //ItemData[] x = Resources.LoadAll<ItemData>( "Data" );
        //foreach ( var item in x )
        //{
        //    Debug.Log( item.id );
        //}

        var guids = AssetDatabase.FindAssets( "t:RecipeData", new[] { "Assets/Data/Recipes" } );
        var paths = guids.Select( AssetDatabase.GUIDToAssetPath );
        var recipeData = paths.Select( AssetDatabase.LoadAssetAtPath<RecipeData> ).ToList();
        recipeData.ForEach( recipe => m_AllRecipes.Add( recipe, true ) );
    }
    private void AddUnlockRecipeToList( KeyValuePair<RecipeData, bool> recipe )
    {
        if ( recipe.Value == false ) return;
        m_UnlockedRecipes.Add( recipe.Key );
    }
    
    [ContextMenu( "Get Recipe" )]
    private RecipeData GetRecipeToCook()
    {
        var availableRecipes = m_UnlockedRecipes.Where( RecipeIsAvailable ).ToList();
        if ( availableRecipes.Count == 0 ) return null;
        var randomRecipe = availableRecipes[Random.Range( 0, availableRecipes.Count )];
        return randomRecipe;
    }

    //Check stock is sufficient to make food/recipe
    private bool RecipeIsAvailable( RecipeData recipe )
    => recipe.ingredients.All( i => m_StockIngredients.TryGetValue( i.ingredient.id, out StockIngredient stockIngredient ) && stockIngredient.quantity >= i.quantity );

}