using NPC.Chef;
using NPC.Waiter;
using System.Collections.Generic;
using System.Linq;
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
    public Dictionary<FoodData, bool> AllFoods => m_AllFoods;
    public Queue<KeyValuePair<Seat, FoodData>> OrderQueue => m_OrderQueue;
    public Queue<KeyValuePair<Seat, ServedFood>> FoodsToServe => m_FoodsToServe;
    public Dictionary<string, StockIngredient> StockIngredients => m_StockIngredients;
    public Transform RestaurantGround { get => m_RestaurantGround; }
    public BoxCollider GroundCollider { get => m_GroundCollider; }

    //Furniture
    [SerializeField] private List<Table> m_Tables = new();
    [SerializeField] private List<Seat> m_Seats = new();
    private int RandomSeatIndex { get => Random.Range( 0, m_Seats.Count ); }

    //Food
    private readonly Dictionary<FoodData, bool> m_AllFoods = new();
    private readonly List<FoodData> m_UnlockedFoods = new();
    private readonly Queue<KeyValuePair<Seat, FoodData>> m_OrderQueue = new();
    private readonly Queue<KeyValuePair<Seat, ServedFood>> m_FoodsToServe = new();
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
    [SerializeField] private Transform m_RestaurantGround;
    [SerializeField] private Transform m_FoodPlace;
    private bool m_FirstLoad = true;
    private BoxCollider m_GroundCollider;

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
        m_GroundCollider = m_RestaurantGround.GetComponent<BoxCollider>();
        StockIngredient garlicStock = new( garlic, 100 );
        StockIngredient onionStock = new( onion, 100 );
        //StockIngredient carrotStock = new( carrot, 100 );
        m_StockIngredients.Add( garlic.id, garlicStock );
        m_StockIngredients.Add( onion.id, onionStock );
        //m_StockIngredients.Add( carrot.id, carrotStock );
        //long score = 0;
        //Debug.Log( FuzzySearch.FuzzyMatch( "was", "basket", ref score ) );
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
    public void OrderFood( Seat seat, FoodData food )
    {
        m_OrderQueue.Enqueue( KeyValuePair.Create( seat, food ) );
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
    public void DecreaseStock( FoodData food ) => food.ingredients.ForEach( i => {
        StockIngredient ingredient = m_StockIngredients[i.ingredient.id];
        ingredient.quantity -= i.quantity;
        if ( ingredient.quantity <= 0 ) m_StockIngredients.Remove( ingredient.data.id );

    } );
    public bool TryGetRecipeToCook( out FoodData recipe )
    {
        var availableRecipes = m_UnlockedFoods.Where( StockIsSufficient ).ToList();
        if ( availableRecipes.Count == 0 )
        {
            recipe = null;
            return false;
        }
        recipe = availableRecipes[Random.Range( 0, availableRecipes.Count )];
        return true;
    }

    //Check stock is sufficient to make food/recipe
    public bool StockIsSufficient( FoodData recipe )
    => recipe.ingredients.All( i => m_StockIngredients.TryGetValue( i.ingredient.id, out StockIngredient stockIngredient ) && stockIngredient.quantity >= i.quantity );
    private void LoadRecipeData()
    {
        var foodData = Resources.LoadAll<FoodData>( "Data/Recipes" ).ToList();
        foodData.ForEach( recipe => m_AllFoods.Add( recipe, true ) );
    }
    private void AddUnlockRecipeToList( KeyValuePair<FoodData, bool> recipe )
    {
        if ( recipe.Value == false ) return;
        m_UnlockedFoods.Add( recipe.Key );
    }
    public bool FindNoStoveChef( out Chef chef )
    {
        chef = m_Chefs.SingleOrDefault( c => c.Stove == null );
        if ( chef == null ) return false;
        return true;
    }


}