using NPC.Chef;
using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField] private Transform m_WallBoundary;

    private BoxCollider m_GroundCollider;
    private BoxCollider m_GroundCollider2;

    //Upgrades
    [SerializeField] private GameObject m_ChefPrefab;

    private FoodsController m_FoodsController;
    private SaveManager m_SaveManager;
    private ResourcesLoader m_ResourcesLoader;
    private PlayerAction m_PlayerAction;
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
        m_PlayerAction = PlayerAction.Instance;
        m_SaveManager.OnSave += SaveRestaurantObjectsData;
        m_SaveManager.LoadData(Utils.RESTAURANT_OBJECTS_FILENAME, OnLoadSucceeded, OnLoadFailed);
    }

    private void OnLoadFailed()
    {
        AddChef();
    }

    private void AddChefsFromUpgradeData(int quantity)
    {
        for (int i = 0; i < quantity; i++) AddChef();
    }

    private void OnDisable()
    {
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
        float randX = Random.Range(GroundCollider2.bounds.min.x, GroundCollider2.bounds.max.x);
        float randZ = Random.Range(GroundCollider2.bounds.min.z, GroundCollider2.bounds.max.z);
        return new(randX, transform.position.y, randZ);
    }

    public void AddChef()
    {
        Chef chef = Instantiate(m_ChefPrefab).GetComponent<Chef>();
        m_Chefs.Add(chef);
        chef.Agent.Warp(GetGroundRandPos());
        chef.transform.SetParent(transform);
    }

    public void ExpandRestaurant()
    {
        (float posX, float scaleX, float wallXBoundary) = m_PlayerAction.PlayerUpgrades.SetRestaurantSize();
        Vector3 helperGroundPos = m_RestaurantGround2.position;
        Vector3 helperGroundScale = m_RestaurantGround2.lossyScale;
        Vector3 wallBoundaryPos = m_WallBoundary.position;
        m_RestaurantGround2.position = new(posX, helperGroundPos.y, helperGroundPos.z);
        m_RestaurantGround2.localScale = new(scaleX, helperGroundScale.y, helperGroundScale.z);
        m_WallBoundary.position = new(wallXBoundary, wallBoundaryPos.y, wallBoundaryPos.z);
        if (m_PlayerAction.PlayerUpgrades.RestaurantExpandLevel == m_PlayerAction.PlayerUpgrades.RESTAURANT_EXPAND_MAX_LEVEL)
            m_WallBoundary.gameObject.SetActive(false);
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

    private void OnLoadSucceeded(JSONNode jsonNode)
    {
        AddChefsFromUpgradeData(m_PlayerAction.PlayerUpgrades.ChefQuantityLevel);

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