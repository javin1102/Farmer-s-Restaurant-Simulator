using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIFurnitureStoreController : UIStoreController
{
    public List<FurnitureData> FurnituresData { get => m_FurnituresData; }
    private List<FurnitureData> m_FurnituresData;
    private new void Awake()
    {
        base.Awake();
        m_FurnituresData = Resources.LoadAll<FurnitureData>( "Data/Furnitures" ).ToList();
    }
}