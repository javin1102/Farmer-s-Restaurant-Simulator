using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIFarmStoreController : UIStoreController
{
    public List<SeedData> SeedsData { get => m_SeedsData;  }
    public List<ItemData> CropsData { get => m_CropsData; }
    public List<ItemData> FarmsData { get => m_FarmsData; }

    private List<SeedData> m_SeedsData;
    private List<ItemData> m_CropsData;
    private List<ItemData> m_FarmsData = new();
    private new void Awake()
    {
        base.Awake();
        m_SeedsData = Resources.LoadAll<SeedData>( "Data/Seeds" ).ToList();
        m_CropsData = Resources.LoadAll<ItemData>( "Data/Crops" ).ToList();
        m_FarmsData = m_CropsData.Concat( m_SeedsData ).ToList();
    }
}
