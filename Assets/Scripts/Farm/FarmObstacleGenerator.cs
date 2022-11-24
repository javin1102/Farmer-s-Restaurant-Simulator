using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmObstacleGenerator : MonoBehaviour
{
    private BoxCollider m_BoxCollider;
    [SerializeField] private GameObject m_ObsPrefab;
    private TileManager m_TileManager;
    [SerializeField] private int m_Size = 50;
    private FarmGround m_FarmGround;
    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
        m_TileManager = TileManager.instance;
        Vector3 minTilePos = m_TileManager.WorldToTilePos( new Vector3( m_BoxCollider.bounds.min.x, 0, m_BoxCollider.bounds.min.z ) );
        Vector3 maxTilePos = m_TileManager.WorldToTilePos( new Vector3( m_BoxCollider.bounds.max.x, 0, m_BoxCollider.bounds.max.z ) );
        m_FarmGround = FarmGround.Instance;
        m_FarmGround.Initialize( minTilePos, maxTilePos );
        for ( int i = 0; i < m_Size; i++ )
        {
            float posX = Random.Range( m_BoxCollider.bounds.min.x, m_BoxCollider.bounds.max.x );
            float posZ = Random.Range( m_BoxCollider.bounds.min.z, m_BoxCollider.bounds.max.z );
            float posY = 0;
            Vector3 tilePos = m_TileManager.WorldToTilePos( new Vector3( posX, posY, posZ ) );
            Vector2 tileIndeces = m_FarmGround.GetFarmGroundTileIndeces( tilePos );
            if ( m_FarmGround.TileObjects[( int ) tileIndeces.x, ( int ) tileIndeces.y] ) continue;
            GameObject obstacle = Instantiate( m_ObsPrefab, tilePos, Quaternion.identity );
            obstacle.tag = Utils.TREE_OBSTACLE_TAG;
            obstacle.layer = Utils.RaycastableLayer;
            m_FarmGround.TileObjects[( int ) tileIndeces.x, ( int ) tileIndeces.y] = true;
        }
    }
}
