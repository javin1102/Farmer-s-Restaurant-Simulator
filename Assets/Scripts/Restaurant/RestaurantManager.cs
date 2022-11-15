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
    public Queue<KeyValuePair<Seat, FoodData>> OrderQueue => m_OrderQueue;
    public Queue<Food> FoodsToServe => m_FoodsToServe;
    public Transform RestaurantGround { get => m_RestaurantGround; }
    public BoxCollider GroundCollider { get => m_GroundCollider; }

    //Furniture
    [SerializeField] private List<Table> m_Tables = new();
    [SerializeField] private List<Seat> m_Seats = new();
    private int RandomSeatIndex { get => Random.Range( 0, m_Seats.Count ); }
    public float WaiterMoveSpeed { get => m_WaiterMoveSpeed; set => m_WaiterMoveSpeed = value; }
    public BoxCollider GroundCollider2 { get => m_GroundCollider2;  }

    private readonly Queue<KeyValuePair<Seat, FoodData>> m_OrderQueue = new();
    private readonly Queue<Food> m_FoodsToServe = new();

    //Chefs
    [SerializeField] private List<Chef> m_Chefs = new();

    //Waiters
    [SerializeField] private List<Waiter> m_Waiters = new();
    private float m_WaiterMoveSpeed = 1.5f;
    //Others
    private static RestaurantManager m_Instance;
    [SerializeField] private Transform m_RestaurantGround;
    [SerializeField] private Transform m_RestaurantGround2;
    [SerializeField] private Transform m_FoodPlace;
    private bool m_FirstLoad = true;
    private BoxCollider m_GroundCollider;
    private BoxCollider m_GroundCollider2;

    //Upgrades
    [SerializeField] private GameObject m_ChefPrefab;
    [SerializeField] private GameObject m_WaiterPrefab;
    [SerializeField] private RestaurantUpgradesChannel m_RestaurantUpgradesChannel;
    //Debug
    [SerializeField] private IngredientData garlic;
    [SerializeField] private IngredientData onion;
    [SerializeField] private IngredientData carrot;

    private FoodsController m_FoodsController;
    private void Awake()
    {
        if ( m_Instance == null ) m_Instance = this;
        else Destroy( gameObject );

        m_GroundCollider = m_RestaurantGround.GetComponent<BoxCollider>();
        m_GroundCollider2 = m_RestaurantGround2.GetComponent<BoxCollider>();
    }

    private void Start()
    {
        m_FoodsController = FoodsController.Instance;
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

    public void DecreaseStock( FoodData food ) => m_FoodsController.DecreaseStock( food );
    public bool TryGetFoodToCook( out FoodData foodData, out FoodConfig foodConfig )
    {
        var availableRecipes = m_FoodsController.AllFoods.Where( IsFoodAvailable ).ToList();
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
        return food.Key.ingredients.All( i => m_FoodsController.StockIngredients.TryGetValue( i.ingredient.id, out StockIngredient stockIngredient ) && stockIngredient.quantity >= i.quantity );
    }


    public bool FindNoStoveChef( out Chef chef )
    {
        chef = m_Chefs.FirstOrDefault( c => c.Stove == null );
        if ( chef == null ) return false;
        return true;
    }

    public bool AnyChefHasStove()
    {
        foreach ( var chef in m_Chefs )
        {
            if ( chef.Stove != null )
            {
                return true;
            }
        }

        return false;
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

    private void IncreaseWaiterSpeed( float speed )
    {
        m_WaiterMoveSpeed += speed;
    }


}