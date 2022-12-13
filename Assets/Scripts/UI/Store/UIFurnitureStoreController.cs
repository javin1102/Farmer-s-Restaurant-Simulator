using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIFurnitureStoreController : UIStoreController
{
    public List<FurnitureData> FurnituresData { get => m_FurnituresData; }
    public List<FurnitureData> ChairsData { get => m_ChairsData; }
    public List<FurnitureData> TablesData { get => m_TablesData; }
    public List<FurnitureData> StovesData { get => m_StovesData; }

    private List<FurnitureData> m_ChairsData;
    private List<FurnitureData> m_TablesData;
    private List<FurnitureData> m_StovesData;
    private List<FurnitureData> m_FurnituresData;
    protected static UIFurnitureStoreController m_Instance;
    public static UIFurnitureStoreController Instance { get => m_Instance; }
    private new void Awake()
    {
        if (m_Instance == null) m_Instance = this;
        base.Awake();
        m_FurnituresData = m_ResourcesLoader.FurnituresData;
        m_ChairsData = m_FurnituresData.Where(furniture => furniture.furnitureType == FurnitureType.SEAT).ToList();
        m_TablesData = m_FurnituresData.Where(furniture => furniture.furnitureType == FurnitureType.TABLE).ToList();
        m_StovesData = m_FurnituresData.Where(furniture => furniture.furnitureType == FurnitureType.STOVE).ToList();
    }
}