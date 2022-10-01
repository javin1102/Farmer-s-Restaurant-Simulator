 using UnityEngine;

/// <summary>
/// 
/// SCRIPT BUAT SPAWN SEED [WIP]
/// 
/// HARUSNYA BUAT TILE DOANG JANGAN LGSUNG INSTATIATE SEEDNYA
/// 
/// </summary>
public class Tile : MonoBehaviour,ITimeTracker
{
    public bool IsUsed = false;
    public GameObject crop;

    [Header("Material Switcher")]
    public Material m_defaultMaterial;
    public Material m_hoedMaterial;
    public Material m_wateredMaterial;
    public Material m_planteMaterial;

    public enum TileStatus {NORMAL,WATERED,HOED,PLANTED };
    public TileStatus m_tileStatus;

    public static Tile Instance { get; set; }

    private int timeElapsed;
    private GameTimeStamp m_time;
    private PlantGrowHandler m_plantGrowHandler;
    private TimeManager m_timeManager;


    private void Awake()
    {
        Instance = this;
        m_timeManager = FindObjectOfType<TimeManager>();
    }
    private void Start()
    {
        this.m_timeManager.RegisterListener(this);
        //TimeManager.Instance.RegisterListener(this);        
    }
    public void SpawnCrop()
    {
        IsUsed = true;
        crop = Instantiate(this.crop, gameObject.transform.position,Quaternion.identity);
        crop.transform.SetParent(this.transform);
    }

    public void SwitchStatus(TileStatus tileStatus)
    {
        this.m_tileStatus = tileStatus;

        Material materialSwitch = m_defaultMaterial;

        switch(tileStatus)
        {
            case TileStatus.NORMAL:
                materialSwitch = m_defaultMaterial;
                break;
            case  TileStatus.WATERED:
                materialSwitch = m_wateredMaterial;
                this.tag = Utils.TILE_WET_TAG;
                this.m_time = TimeManager.Instance.GetGameTimeStamp();
                break;
            case TileStatus.HOED:
                materialSwitch = m_hoedMaterial;
                this.tag = Utils.TILE_TAG;
                this.m_time = TimeManager.Instance.GetGameTimeStamp();
                break;
            case TileStatus.PLANTED:
                if (this.CompareTag(Utils.TILE_WET_TAG)) materialSwitch = m_wateredMaterial;
                else if (this.CompareTag(Utils.TILE_TAG)) materialSwitch = m_hoedMaterial;
                this.m_plantGrowHandler = GetComponentInChildren<PlantGrowHandler>();
                this.m_time = TimeManager.Instance.GetGameTimeStamp();
                break;
        }

        GetComponent<Renderer>().material = materialSwitch;

    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        this.timeElapsed = GameTimeStamp.CompareTImeStamps(m_time, timeStamp);
        if (this.m_tileStatus == TileStatus.HOED)
        {
            Debug.Log("timehoed : " + timeElapsed);
            if (this.transform.childCount > 0)
            {
                // do nothing
            }
            else
            {
                if (this.timeElapsed > 36)
                {
                    this.m_timeManager.UnRegisterListener(this);
                    Destroy(this.gameObject);
                }
            }
        }
        if (this.m_tileStatus == TileStatus.WATERED)
        {
            Debug.Log(timeElapsed);
            if (this.transform.childCount > 0)
            {
                this.SwitchStatus(TileStatus.PLANTED);
            }
            else
            {
                if (this.timeElapsed > 18)
                {
                    this.SwitchStatus(TileStatus.HOED);
                }
            }
        }
        if (this.m_tileStatus == TileStatus.PLANTED)
        {
            if (this.timeElapsed > 19 && this.CompareTag(Utils.TILE_WET_TAG))
            {
                this.m_plantGrowHandler.GrowProgression();
                this.SwitchStatus(TileStatus.HOED);
            }
            else if(this.timeElapsed > 35 && this.CompareTag(Utils.TILE_TAG))
            {
                this.m_timeManager.UnRegisterListener(this);
                Destroy(this.transform.GetChild(0).gameObject);
            }
        }

    }
}
