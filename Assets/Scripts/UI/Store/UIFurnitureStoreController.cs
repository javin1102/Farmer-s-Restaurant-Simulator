using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIFurnitureStoreController : UIStoreController
{
    public List<FurnitureData> FurnituresData { get => m_FurnituresData; }
    public List<FurnitureData> ChairsData { get => m_ChairsData;  }
    public List<FurnitureData> TablesData { get => m_TablesData;  }
    public List<FurnitureData> StovesData { get => m_StovesData;  }

    private List<FurnitureData> m_ChairsData;
    private List<FurnitureData> m_TablesData;
    private List<FurnitureData> m_StovesData;
    private List<FurnitureData> m_FurnituresData;
    private new void Awake()
    {
        base.Awake();
        m_FurnituresData = Resources.LoadAll<FurnitureData>( "Data/Furnitures" ).ToList();
        m_ChairsData = m_FurnituresData.Where( furniture => furniture.type == FurnitureType.CHAIR ).ToList();
        m_TablesData = m_FurnituresData.Where( furniture => furniture.type == FurnitureType.TABLE ).ToList();
        m_StovesData = m_FurnituresData.Where( furniture => furniture.type == FurnitureType.STOVE ).ToList();
    }
}