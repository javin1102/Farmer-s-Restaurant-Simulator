using NPC.Chef;
using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager Instance { get => m_Instance; }
    public List<Table> Tables { get => m_Tables; }
    public List<Seat> Seats { get => m_Seats; }
    public List<Stove> Stoves { get => m_Stoves; }
    public List<Chef> Chefs => m_Chefs;
    public Queue<KeyValuePair<Seat, FoodData>> OrderQueue => m_OrderQueue;
    public Queue<Food> FoodsToServe => m_FoodsToServe;
    public Transform RestaurantGround { get => m_RestaurantGround; }
    public BoxCollider GroundCollider { get => m_GroundCollider; }


    public BoxCollider GroundCollider2 { get => m_GroundCollider2; }

    private readonly Queue<KeyValuePair<Seat, FoodData>> m_OrderQueue = new();
    private readonly Queue<Food> m_FoodsToServe = new();


    //Furniture
    private readonly List<Table> m_Tables = new();
    private readonly List<Seat> m_Seats = new();
    private readonly List<Stove> m_Stoves = new();
    //Chefs
    [SerializeField] private List<Chef> m_Chefs = new();

    //Others
    private static RestaurantManager m_Instance;
    [SerializeField] private Transform m_RestaurantGround;
    [SerializeField] private Transform m_RestaurantGround2;
    private bool m_FirstLoad = true;
    private BoxCollider m_GroundCollider;
    private BoxCollider m_GroundCollider2;

    //Upgrades
    [SerializeField] private GameObject m_ChefPrefab;
    [SerializeField] private RestaurantUpgradesChannel m_RestaurantUpgradesChannel;

    private FoodsController m_FoodsController;
    private SaveManager m_SaveManager;
    private ResourcesLoader m_ResourcesLoader;
    private void Awake()
    {
        if (m_Instance == null) m_Instance = this;
        else Destroy(gameObject);

        m_GroundCollider = m_RestaurantGround.GetComponent<BoxCollider>();
        m_GroundCollider2 = m_RestaurantGround2.GetComponent<BoxCollider>();
    }

    private void Start()
    {
        m_FoodsController = FoodsController.Instance;
        m_SaveManager = SaveManager.Instance;
        m_ResourcesLoader = ResourcesLoader.Instance;
        AddChefsFromUpgradeData(PlayerAction.Instance.PlayerUpgrades.ChefQuantityLevel);
        m_SaveManager.OnSave += SaveRestaurantObjectsData;
        m_SaveManager.LoadData(Utils.RESTAURANT_OBJECTS_FILENAME, OnRestaurantObjectsLoadSucceeded, null);
    }

    private void AddChefsFromUpgradeData(int quantity)
    {
        for (int i = 0; i < quantity; i++) AddChef();
    }

    private void OnEnable()
    {
        m_RestaurantUpgradesChannel.AddChef += AddChef;

    }
    private void OnDisable()
    {
        m_RestaurantUpgradesChannel.AddChef -= AddChef;
        m_SaveManager.OnSave -= SaveRestaurantObjectsData;
    }
    public bool FindUnoccupiedSeat(out Seat seat)
    {
        List<Seat> seats = m_Seats.Where(seat => !seat.IsOccupied).ToList();
        if (seats.Count == 0)
        {
            seat = null;
            return false;
        }

        int randomSeatIndex = Random.Range(0, seats.Count);
        seat = seats[randomSeatIndex];
        return true;
    }
    public void OrderFood(Seat seat, FoodData food) => m_OrderQueue.Enqueue(KeyValuePair.Create(seat, food));
    public void DecreaseStock(FoodData food) => m_FoodsController.DecreaseStock(food);
    public bool TryGetFoodToCook(out FoodData foodData, out FoodConfig foodConfig)
    {
        var availableRecipes = m_FoodsController.AllFoods.Where(IsFoodAvailable).ToList();
        if (availableRecipes.Count == 0)
        {
            foodData = null;
            foodConfig = null;
            return false;
        }
        int rand = Random.Range(0, availableRecipes.Count);
        foodData = availableRecipes[rand].Key;
        foodConfig = availableRecipes[rand].Value;
        return true;
    }

    public bool IsFoodAvailable(KeyValuePair<FoodData, FoodConfig> food)
    {
        if (!food.Value.IsUnlock || !food.Value.IsSelling) return false;
        return food.Key.ingredients.All(i => m_FoodsController.StockIngredients.TryGetValue(i.ingredient.ID, out StockIngredient stockIngredient) && stockIngredient.quantity >= i.quantity);
    }
    public bool FindNoStoveChef(out Chef chef) => chef = m_Chefs.FirstOrDefault(c => c.Stove == null);
    public bool AnyChefHasStove() => m_Chefs.Any(chef => chef.Stove != null);
    public Vector3 GetGroundRandPos()
    {
        float randX = Random.Range(GroundCollider.bounds.min.x, GroundCollider.bounds.max.x);
        float randZ = Random.Range(GroundCollider.bounds.min.z, GroundCollider.bounds.max.z);
        return new(randX, transform.position.y, randZ);
    }

    private void AddChef()
    {
        Chef chef = Instantiate(m_ChefPrefab, transform).GetComponent<Chef>();
        m_Chefs.Add(chef);
        chef.Agent.Warp(GetGroundRandPos());
    }

    private async void SaveRestaurantObjectsData()
    {
        JSONObject rootObject = new();
        JSONArray tableJsonArray = new(), seatJsonArray = new(), stoveJsonArray = new();
        m_Seats.ForEach(seat => seatJsonArray.Add(new SerializableFurnitureData(seat).Serialize()));
        m_Tables.ForEach(table => tableJsonArray.Add(new SerializableFurnitureData(table).Serialize()));
        m_Stoves.ForEach(stove => stoveJsonArray.Add(new SerializableStoveData(stove).Serialize()));
        rootObject.Add("tables", tableJsonArray);
        rootObject.Add("seats", seatJsonArray);
        rootObject.Add("stoves", stoveJsonArray);
        await m_SaveManager.SaveData(rootObject.ToString(), Utils.RESTAURANT_OBJECTS_FILENAME);
    }

    private void OnRestaurantObjectsLoadSucceeded(JSONNode jsonNode)
    {
        JSONNode tableNode = jsonNode["tables"];
        JSONNode seatNode = jsonNode["seats"];
        JSONNode stoveNode = jsonNode["stoves"];

        SpawnFromJsonData(tableNode);
        SpawnFromJsonData(seatNode);
        SpawnFromJsonData(stoveNode);


        void SpawnFromJsonData(JSONNode jsonNode)
        {
            foreach (var node in jsonNode)
            {
                SerializableFurnitureData jsonData = new(node);
                FurnitureData furnitureData = m_ResourcesLoader.GetFurnitureDataByID(jsonData.ID);
                Furniture furniturePrefab = furnitureData.prefab.GetComponent<Furniture>();
                Furniture spawnedFurniture = furniturePrefab.SpawnFurniture(jsonData.transform.position, jsonData.transform.eulerAngles, jsonData.transform.localScale);
                if (furnitureData.furnitureType == FurnitureType.STOVE)
                {
                    Stove stove = (Stove)spawnedFurniture;
                    SerializableStoveData stoveJsonData = new(node);
                    if (stoveJsonData.hasChef) stove.SetChef_Warp();
                }
            }
        }
    }
}