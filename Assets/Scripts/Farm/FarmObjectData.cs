using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FarmObjectData : SerializableData
{
    /**
    * <summary>Index from resouces farm objects</summary>
    **/
    //public int ID { get => id; set => id = value; }
    //public GameObject Prefab { get => m_Prefab; set => m_Prefab = value; }
    //public Vector3 TilePos { get => tilePos; set => tilePos = value; }

    public int resourcesIndex;
    public int tileIndex;
    public Vector3 worldPos;
    public Vector3 eulerAngles;
    public Vector3 scale;
    public FarmObjectData() { }
    public FarmObjectData( int resourcesIndex, int tileIndex, Vector3 worldPos, Vector3 eulerAngles, Vector3 scale )
    {
        this.resourcesIndex = resourcesIndex;
        this.tileIndex = tileIndex;
        this.worldPos = worldPos;
        this.eulerAngles = eulerAngles;
        this.scale = scale;
    }
}
