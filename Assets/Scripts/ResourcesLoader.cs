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
    public List<GameObject> FarmObjects { get => m_FarmObjects; }
    public Dictionary<string, AudioClip> AudioClips { get => m_AudioClips; }

    private List<ItemData> m_EquipmentsData, m_CropsData;
    [SerializeField] private List<ItemData> m_StarterPackData;
    private List<SeedData> m_SeedsData;
    private List<FurnitureData> m_FurnituresData;
    private List<IngredientData> m_IngredientsData;
    private List<FoodData> m_FoodsData;
    private List<GameObject> m_FarmObjects;
    private List<FunfactSO> m_Funfacts;
    private readonly Dictionary<string, AudioClip> m_AudioClips = new();
    private static ResourcesLoader m_Instance;
    private void Awake()
    {
        if (m_Instance == null) m_Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(this);

        m_EquipmentsData = Resources.LoadAll<ItemData>("Data/Equipments").ToList();
        m_CropsData = Resources.LoadAll<ItemData>("Data/Crops").ToList();
        m_SeedsData = Resources.LoadAll<SeedData>("Data/Seeds").ToList();
        m_IngredientsData = Resources.LoadAll<IngredientData>("Data/Ingredients").ToList();
        m_FurnituresData = Resources.LoadAll<FurnitureData>("Data/Furnitures").ToList();
        m_FoodsData = Resources.LoadAll<FoodData>("Data/Recipes").ToList();
        m_FarmObjects = Resources.LoadAll<GameObject>("Prefabs/FarmObjects").ToList();
        m_Funfacts = Resources.LoadAll<FunfactSO>("Data/Funfacts").ToList();
        LoadAudioClips();
    }

    private void LoadAudioClips()
    {
        List<AudioClip> audioClips = Resources.LoadAll<AudioClip>("AudioClips").ToList();
        audioClips.ForEach(clip => m_AudioClips.Add(clip.name, clip));
    }

    public GameObject GetPlantTileObject() => m_FarmObjects[GetFarmObjectIndex<Tree>()];
    public int GetFarmObjectIndex<T>() => FarmObjects.Select(GetFarmResourcesIndexByType<T>).Where(index => index != -1).First();
    public IngredientData GetIngredientDataByID(string id) => m_IngredientsData.Where(data => data.ID == id).First();
    public FoodData GetFoodDataByID(string id) => m_FoodsData.Where(data => data.ID == id).First();
    public FurnitureData GetFurnitureDataByID(string id) => m_FurnituresData.Where(data => data.ID == id).First();
    public SeedData GetSeedDataByID(string id) => m_SeedsData.Where(data => data.ID == id).First();
    public FunfactSO GetRandomFunfact() => m_Funfacts[Random.Range(0, m_Funfacts.Count)];
    public ItemData GetEquipmentDataByID(string id) => m_EquipmentsData.Where(data => data.ID == id).First();
    public ItemData GetCropDataByID(string id) => m_CropsData.Where(data => data.ID == id).First();
    private int GetFarmResourcesIndexByType<T>(GameObject obj, int index)
    {
        if (obj.TryGetComponent(out T _))
        {
            return index;
        }
        return -1;
    }

}
