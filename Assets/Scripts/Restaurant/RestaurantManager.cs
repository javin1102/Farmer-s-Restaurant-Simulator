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
    public Dictionary<FoodData, FoodConfig> AllFoods => m_AllFoods;
    public Queue<KeyValuePair<Seat, FoodData>> OrderQueue => m_OrderQueue;
    public Queue<KeyValuePair<Seat, ServedFood>> FoodsToServe => m_FoodsToServe;
    public Dictionary<string, StockIngredient> StockIngredients => m_StockIngredients;
    public Transform RestaurantGround { get => m_RestaurantGround; }
    public BoxCollider GroundCollider { get => m_GroundCollider; }

    //Furniture
    [SerializeField] private List<Table> m_Tables = new();
    [SerializeField] private List<Seat> m_Seats = new();
    private int RandomSeatIndex { get => Random.Range( 0, m_Seats.Count ); }
    public float WaiterMoveSpeed { get => m_WaiterMoveSpeed; set => m_WaiterMoveSpeed =  value ; }

    //Food
    private readonly Dictionary<FoodData, FoodConfig> m_AllFoods = new();
    private readonly Queue<KeyValuePair<Seat, FoodData>> m_OrderQueue = new();
    private readonly Queue<KeyValuePair<Seat, ServedFood>> m_FoodsToServe = new();
    private readonly Dictionary<string, StockIngredient> m_StockIngredients = new();

    //Chefs
    [SerializeField] private List<Chef> m_Chefs = new();

    //Waiters
    [SerializeField] private List<Waiter> m_Waiters = new();
    private float m_WaiterMoveSpeed = 1.5f;
    //Others
    private static RestaurantManager m_Instance;
    [SerializeField] private Transform m_RestaurantGround;
    [SerializeField] private Transform m_FoodPlace;
    private bool m_FirstLoad = true;
    private BoxCollider m_GroundCollider;

    //Upgrades
    [SerializeField] private GameObject m_ChefPrefab;
    [SerializeField] private GameObject m_WaiterPrefab;
    [SerializeField] private RestaurantUpgradesChannel m_RestaurantUpgradesChannel;
    //Debug
    [SerializeField] private IngredientData garlic;
    [SerializeField] private IngredientData onion;
    [SerializeField] private IngredientData carrot;

    private void Awake()
    {
        if ( m_Instance == null ) m_Instance = this;
        else Destroy( gameObject );

        //Check if player has save file
        if ( m_FirstLoad ) LoadRecipeData();
        m_GroundCollider = m_RestaurantGround.GetComponent<BoxCollider>();
        StockIngredient garlicStock = new( garlic, 100 );
        StockIngredient onionStock = new( onion, 100 );
        //StockIngredient carrotStock = new( carrot, 100 );
        m_StockIngredients.Add( garlic.id, garlicStock );
        m_StockIngredients.Add( onion.id, onionStock );
        //m_StockIngredients.Add( carrot.id, carrotStock );
        

    }

    private void OnEnable()
    {
        m_RestaurantUpgradesChannel.AddChef += AddChef;
        m_RestaurantUpgradesChannel.AddWaiter += AddWaiter;
        m_RestaurantUpgradesChannel.IncreaseWaiterSpeed += IncreaseWaiterSpeed;
    }
    private void OnDisable()
    {
        m_RestaurantUpgradesChannel.AddChef -= AddChef;
        m_RestaurantUpgradesChannel.AddWaiter -= AddWaiter;
        m_RestaurantUpgradesChannel.IncreaseWaiterSpeed -= IncreaseWaiterSpeed;
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
    public void StoreIngredient( IngredientData ingredient )
    {
        if ( m_StockIngredients.TryGetValue( ingredient.id, out StockIngredient stockIngredient ) )
        {
            stockIngredient.quantity += 1;
        }
        else
        {
            stockIngredient = new StockIngredient( ingredient );
            m_StockIngredients.Add( ingredient.id, stockIngredient );
        }
    }
    public void DecreaseStock( FoodData food ) => food.ingredients.ForEach( i =>
    {
        StockIngredient ingredient = m_StockIngredients[i.ingredient.id];
        ingredient.quantity -= i.quantity;
        if ( ingredient.quantity <= 0 ) m_StockIngredients.Remove( ingredient.data.id );

    } );
    public bool TryGetRecipeToCook( out FoodData foodData, out FoodConfig foodConfig )
    {
        var availableRecipes = m_AllFoods.Where( IsFoodAvailable ).ToList();
        if ( availableRecipes.Count == 0 )
        {
            foodData = null;
            foodConfig = null;
            return false;
        }
        int rand = Random.Range( 0, availableRecipes.Count );
        foodData = availableRecipes[rand].Key;
        foodConfig = availableRecipes[rand].Value;
        return true;
    }

    public bool IsFoodAvailable( KeyValuePair<FoodData, FoodConfig> food )
    {
        if ( !food.Value.IsUnlock || !food.Value.IsSelling ) return false;
        return food.Key.ingredients.All( i => m_StockIngredients.TryGetValue( i.ingredient.id, out StockIngredient stockIngredient ) && stockIngredient.quantity >= i.quantity );
    }

    private void LoadRecipeData()
    {
        var foodData = Resources.LoadAll<FoodData>( "Data/Recipes" ).ToList();
        foodData.ForEach( recipe => m_AllFoods.Add( recipe, new( true, true ) ) );
    }

    public bool FindNoStoveChef( out Chef chef )
    {
        chef = m_Chefs.FirstOrDefault( c => c.Stove == null );
        if ( chef == null ) return false;
        return true;
    }

    public Vector3 GetGroundRandPos()
    {
        float randX = Random.Range( GroundCollider.bounds.min.x, GroundCollider.bounds.max.x );
        float randZ = Random.Range( GroundCollider.bounds.min.z, GroundCollider.bounds.max.z );
        return new( randX, transform.position.y, randZ );
    }

    private void AddChef()
    {
        Chef chef = Instantiate( m_ChefPrefab, transform ).GetComponent<Chef>();
        chef.Agent.Warp( GetGroundRandPos() );
    }

    private void AddWaiter()
    {
        Waiter waiter = Instantiate( m_WaiterPrefab, transform ).GetComponent<Waiter>();
        waiter.Agent.Warp( GetGroundRandPos() );
    }

    private void IncreaseWaiterSpeed(float speed)
    {
        m_WaiterMoveSpeed += speed;
    }


}