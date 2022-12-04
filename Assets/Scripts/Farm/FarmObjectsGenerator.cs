using SimpleJSON;
using System.Linq;
using UnityEngine;

public class FarmObjectsGenerator : MonoBehaviour
{
    private BoxCollider m_BoxCollider;
    //private int m_TreeResourcesIndex;
    private GameObject m_ObjectPrefab; //prefab
    private TileManager m_TileManager;
    [SerializeField] private int m_Size = 50;
    private FarmGround m_FarmGround;
    private ResourcesLoader m_ResourcesLoader;

    private void Awake()
    {
        m_ResourcesLoader = ResourcesLoader.Instance;
        m_FarmGround = FarmGround.Instance;
        m_TileManager = TileManager.instance;
        m_BoxCollider = GetComponent<BoxCollider>();

        Vector3 minTilePos = m_TileManager.WorldToTilePos(new Vector3(m_BoxCollider.bounds.min.x, 0, m_BoxCollider.bounds.min.z));
        Vector3 maxTilePos = m_TileManager.WorldToTilePos(new Vector3(m_BoxCollider.bounds.max.x, 0, m_BoxCollider.bounds.max.z));
        m_FarmGround.Initialize(minTilePos, maxTilePos);
        //m_ObjectPrefab = m_ResourcesLoader.FarmObjects[Tree.ResourcesIndex] ;
        GenerateFarmObjects();
    }

    private void GenerateFarmObjects()
    {
        if (m_FarmGround.TryLoadFarmObjectData(out JSONNode farmObjectsNode))
        {
            GenerateFromSaveData(farmObjectsNode);
        }
        else
        {
            GenerateTreeRandom();
        }
    }

    private void GenerateTreeRandom()
    {
        int treeIndex = m_ResourcesLoader.GetFarmObjectIndex<Tree>();
        for (int i = 0; i < m_Size; i++)
        {
            float posX = Random.Range(m_BoxCollider.bounds.min.x, m_BoxCollider.bounds.max.x);
            float posZ = Random.Range(m_BoxCollider.bounds.min.z, m_BoxCollider.bounds.max.z);
            float posY = 0;
            GameObject treeGO = GenerateObject(new(posX, posY, posZ), Vector3.zero, Vector3.one, treeIndex);
            if (treeGO != null)
            {
                treeGO.tag = Utils.TREE_OBSTACLE_TAG;
            }
        }
    }

    private GameObject GenerateObject(Vector3 position, Vector3 eulerAngles, Vector3 scale, int resourcesIndex)
    {
        m_ObjectPrefab = m_ResourcesLoader.FarmObjects[resourcesIndex];
        Vector3 tilePos = m_TileManager.WorldToTilePos(new Vector3(position.x, position.y, position.z));
        int tileIndex = m_FarmGround.GetUniqueIdx(tilePos);

        if (m_FarmGround.FarmObjects.ContainsKey(tileIndex)) return null;
        BaseFarmObject farmObject = Instantiate(m_ObjectPrefab, tilePos, Quaternion.Euler(eulerAngles)).GetComponent<BaseFarmObject>();
        farmObject.transform.localScale = scale;
        farmObject.Set(resourcesIndex, tileIndex);
        farmObject.gameObject.layer = Utils.RaycastableLayer;
        m_FarmGround.FarmObjects.Add(tileIndex, farmObject);
        return farmObject.gameObject;

    }

    private void GenerateFromSaveData(JSONNode farmObjectsNode)
    {
        int plantTileIndex = m_ResourcesLoader.GetFarmObjectIndex<PlantTile>();
        foreach (var node in farmObjectsNode)
        {
            SerializableFarmObjectData data = new(node.Value);
            GameObject obj = GenerateObject(data.transform.position, data.transform.eulerAngles, data.transform.localScale, data.resourcesIndex);

            if (data.resourcesIndex == plantTileIndex)
            {
                SerializablePlantTileData plantTileData = new(node.Value);
                PlantTile plantTile = obj.GetComponent<PlantTile>();
                plantTile.Time = plantTileData.time;
                if (!string.IsNullOrEmpty(plantTileData.seedID))
                {
                    SeedData resourcesSeedData = m_ResourcesLoader.GetSeedDataByID(plantTileData.seedID);
                    plantTile.SpawnCrop(resourcesSeedData.cropPrefab);
                    plantTile.tag = plantTileData.isWet ? Utils.TILE_WET_TAG : Utils.TILE_TAG;
                    plantTile.SwitchStatus((PlantTile.TileStatus)plantTileData.plantStatus);
                    plantTile.PlantGrowHandler.SeedData = resourcesSeedData;
                }
            }

            else
            {
                obj.tag = Utils.TREE_OBSTACLE_TAG;
            }
        }
    }

}
