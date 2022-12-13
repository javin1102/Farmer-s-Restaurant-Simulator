using System.Collections.Generic;
using System.Linq;

public class UIFarmStoreController : UIStoreController
{
    public List<SeedData> SeedsData { get => m_SeedsData; }
    public List<ItemData> CropsData { get => m_CropsData; }
    public List<ItemData> FarmsData { get => m_FarmsData; }

    private List<SeedData> m_SeedsData;
    private List<ItemData> m_CropsData;
    private List<ItemData> m_FarmsData = new();

    protected static UIFarmStoreController m_Instance;
    public static UIFarmStoreController Instance { get => m_Instance; }
    private new void Awake()
    {
        if (m_Instance == null) m_Instance = this;
        base.Awake();
        m_SeedsData = m_ResourcesLoader.SeedsData;
        m_CropsData = m_ResourcesLoader.CropsData;
        m_FarmsData = m_ResourcesLoader.CropsData.Concat(m_SeedsData).ToList();
    }
}
