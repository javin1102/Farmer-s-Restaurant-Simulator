using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourcesLoader : MonoBehaviour
{
    public static ResourcesLoader Instance { get => m_Instance; }
    public List<ItemData> EquipmentsData { get => m_EquipmentsData; }
    public List<ItemData> CropsData { get => m_CropsData; set => m_CropsData = value; }
    public List<ItemData> StarterPackData { get => m_StarterPackData; }
    public List<SeedData> SeedsData { get => m_SeedsData; }
    public List<FurnitureData> FurnituresData { get => m_FurnituresData; }
    public List<IngredientData> IngredientsData { get => m_IngredientsData; }
    public List<FoodData> FoodsData { get => m_FoodsData; }

    private List<ItemData> m_EquipmentsData, m_CropsData, m_StarterPackData;
    private List<SeedData> m_SeedsData;
    private List<FurnitureData> m_FurnituresData;
    private List<IngredientData> m_IngredientsData;
    private List<FoodData> m_FoodsData;
    private static ResourcesLoader m_Instance;
    private void Awake()
    {
        if ( m_Instance == null ) m_Instance = this;
        else Destroy( gameObject );
        DontDestroyOnLoad( this );

        m_EquipmentsData = Resources.LoadAll<ItemData>( "Data/Equipments" ).ToList();
        m_CropsData = Resources.LoadAll<ItemData>( "Data/Crops" ).ToList();
        m_StarterPackData = Resources.LoadAll<ItemData>( "Data/StarterPack" ).ToList();
        m_SeedsData = Resources.LoadAll<SeedData>( "Data/Seeds" ).ToList();
        m_IngredientsData = Resources.LoadAll<IngredientData>( "Data/Ingredients" ).ToList();
        m_FurnituresData = Resources.LoadAll<FurnitureData>( "Data/Furnitures" ).ToList();
        m_FoodsData = Resources.LoadAll<FoodData>( "Data/Recipes" ).ToList();
    }
}
