using System.Collections.Generic;
using System.Linq;

public class UIFarmStoreController : UIStoreController
{
    public List<SeedData> SeedsData { get => m_SeedsData; }

    private List<SeedData> m_SeedsData = new();

    protected static UIFarmStoreController m_Instance;
    public static UIFarmStoreController Instance { get => m_Instance; }
    private new void Awake()
    {
        if (m_Instance == null) m_Instance = this;
        base.Awake();
        m_SeedsData = m_ResourcesLoader.SeedsData;
    }
}
