using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UISeedStoreController : UIStoreController
{
    public List<SeedData> SeedsData { get => m_SeedsData;  }
    private List<SeedData> m_SeedsData;
    private new void Awake()
    {
        base.Awake();
        m_SeedsData = Resources.LoadAll<SeedData>( "Data/Seeds" ).ToList();
    }
}
