using System.Linq;
using UnityEngine;

public class FarmObjectsGenerator : MonoBehaviour
{
    private BoxCollider m_BoxCollider;
    private int m_TreeResourcesIndex;
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

        Vector3 minTilePos = m_TileManager.WorldToTilePos( new Vector3( m_BoxCollider.bounds.min.x, 0, m_BoxCollider.bounds.min.z ) );
        Vector3 maxTilePos = m_TileManager.WorldToTilePos( new Vector3( m_BoxCollider.bounds.max.x, 0, m_BoxCollider.bounds.max.z ) );
        m_FarmGround.Initialize( minTilePos, maxTilePos );
        m_ResourcesLoader.GetFarmObjectIndex<Tree>( out m_TreeResourcesIndex );
        m_ObjectPrefab = m_ResourcesLoader.FarmObjects[m_TreeResourcesIndex];
        GenerateFarmObjects();
    }

    private void GenerateFarmObjects()
    {
        if ( m_FarmGround.TryLoadFarmObjectData( out FarmObjectData[] farmObjectsData ) )
        {
            GenerateFromSaveData( farmObjectsData );
        }
        else
        {
            GenerateObjectsRandom();
        }
    }

    private void GenerateObjectsRandom()
    {

        for ( int i = 0; i < m_Size; i++ )
        {
            float posX = Random.Range( m_BoxCollider.bounds.min.x, m_BoxCollider.bounds.max.x );
            float posZ = Random.Range( m_BoxCollider.bounds.min.z, m_BoxCollider.bounds.max.z );
            float posY = 0;
            GenerateObject( new( posX, posY, posZ ), Vector3.zero, Vector3.one, m_TreeResourcesIndex );
        }
    }

    private void GenerateObject( Vector3 position, Vector3 eulerAngles, Vector3 scale, int resourcesIndex )
    {
        m_ObjectPrefab = m_ResourcesLoader.FarmObjects[resourcesIndex];
        Vector3 tilePos = m_TileManager.WorldToTilePos( new Vector3( position.x, position.y, position.z ) );
        int tileIndex = m_FarmGround.GetUniqueIdx( tilePos );

        if ( m_FarmGround.FarmObjects.ContainsKey( tileIndex ) ) return;
        FarmObject farmObject = Instantiate( m_ObjectPrefab, tilePos, Quaternion.Euler( eulerAngles ) ).GetComponent<FarmObject>();
        farmObject.transform.localScale = scale;
        farmObject.Set( resourcesIndex, tileIndex );
        farmObject.tag = Utils.TREE_OBSTACLE_TAG;
        farmObject.gameObject.layer = Utils.RaycastableLayer;
        m_FarmGround.FarmObjects.Add( tileIndex, farmObject );

    }

    private void GenerateFromSaveData( FarmObjectData[] farmObjectsData )
    {
        Debug.Log( "Load From save" );
        foreach ( var data in farmObjectsData )
        {
            GenerateObject( data.worldPos, data.eulerAngles, data.scale, data.resourcesIndex );
        }
    }

}
