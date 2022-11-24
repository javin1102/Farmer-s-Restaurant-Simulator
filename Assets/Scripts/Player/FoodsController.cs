using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private IngredientData garlic;
    [SerializeField] private IngredientData onion;
    [SerializeField] private IngredientData carrot;
    private void Awake()
    {
        if ( m_Instance == null ) m_Instance = this;
        else Destroy( gameObject );
    }

    private void Start()
    {
        m_ResourcesLoader = ResourcesLoader.Instance;
        LoadFoodData();

        StockIngredient garlicStock = new( garlic, 100 );
        StockIngredient onionStock = new( onion, 100 );
        //StockIngredient carrotStock = new( carrot, 100 );
        m_StockIngredients.Add( garlic.id, garlicStock );
        m_StockIngredients.Add( onion.id, onionStock );
        //m_StockIngredients.Add( carrot.id, carrotStock );


    }

    private void LoadFoodData()
    {
        var foodData = m_ResourcesLoader.FoodsData;
        foodData.ForEach( recipe => m_AllFoods.Add( recipe, new( true, true ) ) );
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
}
