using UnityEngine;

/// <summary>
/// 
/// SCRIPT BUAT SPAWN SEED [WIP]
/// 
/// HARUSNYA BUAT TILE DOANG JANGAN LGSUNG INSTATIATE SEEDNYA
/// 
/// </summary>
public class PlantTile : BaseFarmObject, ITimeTracker
{
    public PlantGrowHandler PlantGrowHandler { get => m_plantGrowHandler; }
    public GameTimeStamp Time { get => m_Time; set => m_Time = value; }

    public bool IsUsed = false;
    public GameObject crop;

    [Header("Material Switcher")]
    public Material m_defaultMaterial;
    public Material m_hoedMaterial;
    public Material m_wateredMaterial;
    public Material m_planteMaterial;

    public enum TileStatus { NORMAL, WATERED, HOED, PLANTED };
    public TileStatus Status;

    //public static Tile Instance { get; set; }
    private GameTimeStamp m_Time;
    private PlantGrowHandler m_plantGrowHandler;
    [SerializeField] private TimeManager m_TimeManager;


    private new void Start()
    {
        base.Start();
        m_TimeManager = TimeManager.Instance;
        m_TimeManager.RegisterListener(this);
        m_Time = m_TimeManager.GetCurrentTimeStamp();
    }
    public void SpawnCrop(GameObject cropPrefab)
    {
        IsUsed = true;
        crop = Instantiate(cropPrefab, gameObject.transform.position, Quaternion.identity);
        crop.transform.SetParent(this.transform);
        m_plantGrowHandler = crop.GetComponent<PlantGrowHandler>();
    }

    public void SwitchStatus(TileStatus tileStatus)
    {
        if (m_TimeManager == null) m_TimeManager = TimeManager.Instance;
        this.Status = tileStatus;
        Material materialSwitch = m_hoedMaterial;

        switch (tileStatus)
        {
            case TileStatus.WATERED:
                materialSwitch = m_wateredMaterial;
                this.tag = Utils.TILE_WET_TAG;
                this.m_Time = m_TimeManager.GetCurrentTimeStamp();
                break;
            case TileStatus.HOED:
                materialSwitch = m_hoedMaterial;
                this.tag = Utils.TILE_TAG;
                this.m_Time = m_TimeManager.GetCurrentTimeStamp();
                break;
            case TileStatus.PLANTED:
                if (this.CompareTag(Utils.TILE_WET_TAG)) materialSwitch = m_wateredMaterial;
                else if (this.CompareTag(Utils.TILE_TAG)) materialSwitch = m_hoedMaterial;
                this.m_Time = m_TimeManager.GetCurrentTimeStamp();
                break;
        }

        GetComponent<Renderer>().material = materialSwitch;

    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        int timeElapsed = GameTimeStamp.CompareTimeStamps(m_Time, timeStamp);
        if (this.Status == TileStatus.HOED)
        {
            if (this.transform.childCount > 0)
            {
                // do nothing
            }
            else
            {
                if (timeElapsed > 36)
                {
                    this.m_TimeManager.UnRegisterListener(this);
                    Destroy(this.gameObject);
                }
            }
        }
        if (this.Status == TileStatus.WATERED)
        {
            if (this.transform.childCount > 0)
            {
                this.SwitchStatus(TileStatus.PLANTED);
            }
            else
            {
                if (timeElapsed > 18)
                {
                    this.SwitchStatus(TileStatus.HOED);
                }
            }
        }
        if (this.Status == TileStatus.PLANTED)
        {
            if (timeElapsed >= m_plantGrowHandler.SeedData.hourToGrow && this.CompareTag(Utils.TILE_WET_TAG))
            {
                this.m_plantGrowHandler.GrowProgression();
                this.SwitchStatus(TileStatus.HOED);
            }
            else if (timeElapsed > 35 && this.CompareTag(Utils.TILE_TAG))
            {
                this.m_TimeManager.UnRegisterListener(this);
                Destroy(this.transform.GetChild(0).gameObject);
            }
        }

    }
}
