using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using UnityEngine;

public class FoodsController : MonoBehaviour
{
    public Dictionary<FoodData, FoodConfig> AllFoods => m_AllFoods;
    public Dictionary<string, StockIngredient> StockIngredients => m_StockIngredients;

    public static FoodsController Instance { get => m_Instance; }

    private readonly Dictionary<FoodData, FoodConfig> m_AllFoods = new();
    private readonly Dictionary<string, StockIngredient> m_StockIngredients = new();
    private static FoodsController m_Instance;
    private ResourcesLoader m_ResourcesLoader;
    //Debug
    private SaveManager m_SaveManager;
    private void Awake()
    {
        if (m_Instance == null) m_Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        m_ResourcesLoader = ResourcesLoader.Instance;
        m_SaveManager = SaveManager.Instance;

        StoreAllFoodData();

        m_SaveManager.LoadData(Utils.FOODS_FILENAME, OnLoadSucceded, OnLoadFailed);
        m_SaveManager.OnSave += SaveData;

    }

    private void OnLoadFailed()
    {
        FoodData gado_gado = m_ResourcesLoader.GetFoodDataByID("Jagung Bakar");
        gado_gado.ingredients.ForEach(ingredient =>
        {
            StockIngredient stock = new(ingredient.ingredient, 50);
            m_StockIngredients.Add(ingredient.ingredient.ID, stock);
        });

        m_AllFoods[gado_gado].IsUnlock = true;
        m_AllFoods[gado_gado].IsSelling = true;
    }

    private void OnLoadSucceded(JSONNode jsonNode)
    {
        JSONArray foodsArr = (JSONArray)jsonNode["foods"];
        JSONArray stocksArr = (JSONArray)jsonNode["stocks"];
        foreach (var food in foodsArr)
        {
            SerializableRecipeData data = new(food.Value);
            FoodData foodData = m_ResourcesLoader.GetFoodDataByID(data.ID);
            m_AllFoods[foodData].IsSelling = data.isSelling;
            m_AllFoods[foodData].IsUnlock = data.isUnlock;
        }

        foreach (var stock in stocksArr)
        {
            SerializableStockIngredientData data = new(stock.Value);
            StockIngredient stockIngredient = new(m_ResourcesLoader.GetIngredientDataByID(data.ID), data.quantity);
            m_StockIngredients.Add(data.ID, stockIngredient);
        }
    }


    private void StoreAllFoodData()
    {
        var foodData = m_ResourcesLoader.FoodsData;
        foodData.ForEach(recipe => m_AllFoods.Add(recipe, new(false, false)));
    }

    public void StoreIngredient(IngredientData ingredient, int quantity = 1)
    {
        if (m_StockIngredients.TryGetValue(ingredient.ID, out StockIngredient stockIngredient))
        {
            stockIngredient.quantity += quantity;
        }
        else
        {
            stockIngredient = new StockIngredient(ingredient, quantity);
            m_StockIngredients.Add(ingredient.ID, stockIngredient);
        }
    }

    public void DecreaseStock(FoodData food) => food.ingredients.ForEach(i =>
    {
        if (m_StockIngredients.TryGetValue(i.ingredient.ID, out StockIngredient ingredient))
        {
            ingredient.quantity -= i.quantity;
            if (ingredient.quantity <= 0) m_StockIngredients.Remove(ingredient.data.ID);
        }

    });

    private async void SaveData()
    {
        JSONObject rootObject = new();
        JSONArray foodsJsonArray = new(), stockIngredientsJsonArray = new();

        //Serialize foods
        foreach (var food in m_AllFoods)
        {
            foodsJsonArray.Add(new SerializableRecipeData(food.Key.ID, food.Value.IsUnlock, food.Value.IsSelling).Serialize());
        }

        //serialize stock ingredients
        foreach (var stock in m_StockIngredients)
        {
            StockIngredient stockIngredient = stock.Value;
            stockIngredientsJsonArray.Add(new SerializableStockIngredientData(stock.Key, stockIngredient.quantity).Serialize());
        }


        rootObject.Add("foods", foodsJsonArray);
        rootObject.Add("stocks", stockIngredientsJsonArray);
        await m_SaveManager.SaveData(rootObject.ToString(), Utils.FOODS_FILENAME);
    }


}
