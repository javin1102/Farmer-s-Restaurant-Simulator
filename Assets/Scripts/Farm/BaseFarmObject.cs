using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFarmObject : MonoBehaviour
{
    public int ResourcesIndex { get; set; }
    public int TileIndex { get; set; }
    protected FarmGround m_FarmGround;
    protected ResourcesLoader m_ResourcesLoader;
    protected void Start()
    {
        m_FarmGround = FarmGround.Instance;
        m_ResourcesLoader = ResourcesLoader.Instance;
    }

    public void Set( int resourcesIndex, int tileIndex )
    {
        ResourcesIndex = resourcesIndex;
        TileIndex = tileIndex;
    }
    protected void OnDestroy()
    {
        m_FarmGround = FarmGround.Instance;
        m_FarmGround.FarmObjects.Remove( TileIndex );
    }
}