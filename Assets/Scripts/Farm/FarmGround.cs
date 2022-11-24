using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmGround : MonoBehaviour
{
    public bool[,] TileObjects { get => m_TilesObjects; set => m_TilesObjects = value; }
    public static FarmGround Instance { get => m_Instance; }
    private bool[,] m_TilesObjects;
    private static FarmGround m_Instance;
    private Vector3 m_MinTilePos, m_MaxTilePos, m_GroundAreaSize;
    private void Awake()
    {
        if ( m_Instance == null ) m_Instance = this;
        else Destroy( this );
        DontDestroyOnLoad( this );
        
    }

    public void Initialize( Vector3 minTilePos, Vector3 maxTilePos )
    {
        m_MinTilePos = minTilePos;
        m_MaxTilePos = maxTilePos;
        m_GroundAreaSize = maxTilePos - minTilePos;
        TileObjects = new bool[( int ) m_GroundAreaSize.x + 1, ( int ) m_GroundAreaSize.z + 1];
    }

    /**
     * <summary>Get farm ground tile indeces from (0,0) to (maxSize.x, maxSize.z) according to world tile position</summary>
     */
    public Vector2 GetFarmGroundTileIndeces( Vector3 worldTilePos )
    {
        Vector3 localPos = m_MaxTilePos - worldTilePos;
        Vector2 indices = new( localPos.x, localPos.z );
        return indices;
    }
}
