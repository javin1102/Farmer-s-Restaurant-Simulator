using NPC.Chef;
using NPC.Waiter;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager Instance { get => m_Instance; }
    public List<Table> Tables { get => m_Tables; }
    public List<Seat> UnoccupiedSeats { get => m_Seats; }
    public Transform FoodPlace { get => m_FoodPlace; }
    public List<Chef> Chefs => m_Chefs;
    public List<Waiter> Waiters => m_Waiters;
    public int WaiterIndex => m_WaiterIndexOffset > m_Waiters.Count - 1 ? m_WaiterIndexOffset = 0 : m_WaiterIndexOffset++;

    //Furniture
    [SerializeField] private List<Table> m_Tables = new();
    [SerializeField] private List<Seat> m_Seats = new();
    private int RandomSeatIndex { get => Random.Range( 0, m_Seats.Count ); }

    //Food
    private readonly Dictionary<FoodData, bool> m_AllFoods = new();
    private readonly List<FoodData> m_UnlockedFoods = new();
    private readonly Dictionary<string, StockIngredient> m_StockIngredients = new();

    //Chefs
    [SerializeField] private List<Chef> m_Chefs = new();
    [SerializeField] private int m_ChefIndexOffset = 0;
    private int ChefIndex => m_ChefIndexOffset > m_Chefs.Count - 1 ? m_ChefIndexOffset = 0 : m_ChefIndexOffset++;

    //Waiters
    [SerializeField] private List<Waiter> m_Waiters = new();
    private int m_WaiterIndexOffset;

    //Others
    private static RestaurantManager m_Instance;
    [SerializeField] private Transform m_FoodPlace;
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
        m_AllFoods.ToList().ForEach( AddUnlockRecipeToList );
        StockIngredient garlicStock = new( garlic, 10 );
        StockIngredient onionStock = new( onion, 10 );
        StockIngredient carrotStock = new( carrot, 10 );
        m_StockIngredients.Add( garlic.id, garlicStock );
        m_StockIngredients.Add( onion.id, onionStock );
        m_StockIngredients.Add( carrot.id, carrotStock );
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
    public void OrderFood( Seat seat )
    {
        if ( !TryGetRecipeToCook( out FoodData food ) ) return;
        int index = ChefIndex;
        Debug.Log( index );
        m_Chefs[index].OrderQueue.Enqueue( KeyValuePair.Create( seat, food ) );

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
    public void DecreaseStock( FoodData food ) => food.ingredients.ForEach( i => m_StockIngredients[i.ingredient.id].quantity -= i.quantity );
    private void LoadRecipeData()
    {
        //ItemData[] x = Resources.LoadAll<ItemData>( "Data" );
        //foreach ( var item in x )
        //{
        //    Debug.Log( item.id );
        //}

        var guids = AssetDatabase.FindAssets( "t:FoodData", new[] { "Assets/Data/Recipes" } );
        var paths = guids.Select( AssetDatabase.GUIDToAssetPath );
        var recipeData = paths.Select( AssetDatabase.LoadAssetAtPath<FoodData> ).ToList();
        recipeData.ForEach( recipe => m_AllFoods.Add( recipe, true ) );
    }
    private void AddUnlockRecipeToList( KeyValuePair<FoodData, bool> recipe )
    {
        if ( recipe.Value == false ) return;
        m_UnlockedFoods.Add( recipe.Key );
    }

    [ContextMenu( "Get Recipe" )]
    private bool TryGetRecipeToCook( out FoodData recipe )
    {
        var availableRecipes = m_UnlockedFoods.Where( RecipeIsAvailable ).ToList();
        if ( availableRecipes.Count == 0 )
        {
            recipe = null;
            return false;
        }
        recipe = availableRecipes[Random.Range( 0, availableRecipes.Count )];
        return true;
    }


    //Check stock is sufficient to make food/recipe
    private bool RecipeIsAvailable( FoodData recipe )
    => recipe.ingredients.All( i => m_StockIngredients.TryGetValue( i.ingredient.id, out StockIngredient stockIngredient ) && stockIngredient.quantity >= i.quantity );

}