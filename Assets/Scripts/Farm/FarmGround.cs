using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FarmGround : MonoBehaviour
{
    public Dictionary<int, BaseFarmObject> FarmObjects { get => m_FarmObjects; set => m_FarmObjects = value; }
    public static FarmGround Instance { get => m_Instance; }
    public bool HasLoadData { get => m_HasLoadData; private set => m_HasLoadData = value; }

    private Dictionary<int, BaseFarmObject> m_FarmObjects = new();
    private static FarmGround m_Instance;
    private Vector3 m_MinTilePos, m_MaxTilePos, m_GroundAreaSize;
    private SaveManager m_SaveManager;
    private ResourcesLoader m_ResourcesLoader;
    private bool m_HasLoadData;
    private void Awake()
    {
        if (m_Instance == null) m_Instance = this;
        else Destroy(this);
        DontDestroyOnLoad(this);

    }
    private void Start()
    {
        m_ResourcesLoader = ResourcesLoader.Instance;
        m_SaveManager = SaveManager.Instance;
        m_SaveManager.OnSave += SaveFarmObjectData;
    }
    private void OnDestroy()
    {
        m_SaveManager.OnSave -= SaveFarmObjectData;
    }
    public void Initialize(Vector3 minTilePos, Vector3 maxTilePos)
    {
        m_MinTilePos = minTilePos;
        m_MaxTilePos = maxTilePos;
        m_GroundAreaSize = maxTilePos - minTilePos;
    }

    /**
     * <summary>Get farm ground tile index from 0 to (groundSize.x * groundSize.z) according to world tile position</summary>
     */
    public int GetUniqueIdx(Vector3 worldTilePos)
    {
        Vector3 localPos = m_MaxTilePos - worldTilePos;
        Vector2Int indices = new((int)localPos.x, (int)localPos.z);
        int uniqueIdx = (indices.x * (int)m_GroundAreaSize.z) + (indices.y + 1);
        return uniqueIdx;
    }

    public bool TryLoadFarmObjectData(out JSONNode farmObjectsData)
    {
        return m_SaveManager.LoadData(Utils.FARM_OBJECTS_FILENAME, out farmObjectsData);
    }


    private async void SaveFarmObjectData()
    {
        var farmObjects = m_FarmObjects.ToArray();
        int plantTileResourcesIndex = m_ResourcesLoader.GetFarmObjectIndex<PlantTile>();
        JSONArray jsonArray = new();
        foreach (var obj in farmObjects)
        {
            if (obj.Value.ResourcesIndex == plantTileResourcesIndex)
            {
                PlantTile plantTile = obj.Value as PlantTile;
                SerializablePlantTileData plantTileData = new(plantTile);
                jsonArray.Add(plantTileData.Serialize());
            }
            else
            {
                SerializableFarmObjectData farmObjectData = new(obj.Value);
                jsonArray.Add(farmObjectData.Serialize());
            }
        }


        await m_SaveManager.SaveData(jsonArray.ToString(), Utils.FARM_OBJECTS_FILENAME);
    }

}