using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmObject : MonoBehaviour
{
    public int ResourcesIndex { get; set; }
    public int TileIndex { get; set; }
    private FarmGround m_FarmGround;
    private void Start()
    {
        m_FarmGround = FarmGround.Instance;
    }

    public void Set( int resourcesIndex, int tileIndex )
    {
        ResourcesIndex = resourcesIndex;
        TileIndex = tileIndex;
    }
    private void OnDestroy()
    {
        m_FarmGround = FarmGround.Instance;
        if(m_FarmGround.FarmObjects.Remove( TileIndex ) )
        {
            Debug.Log( "Remove" );
        }
    }
}