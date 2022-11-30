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

    //public static Tile Instance { get; set; }

    private int timeElapsed;
    private GameTimeStamp m_Time;
    private PlantGrowHandler m_plantGrowHandler;
    [SerializeField] private TimeManager m_TimeManager;


    private void Start()
    {
        m_TimeManager = TimeManager.Instance;
        m_TimeManager.RegisterListener(this);
        m_Time = m_TimeManager.GetCurrentTimeStamp();
    }
    public void SpawnCrop()
    {
        IsUsed = true;
        crop = Instantiate(this.crop, gameObject.transform.position,Quaternion.identity);
        crop.transform.SetParent(this.transform);
    }

    public void SwitchStatus(TileStatus tileStatus)
    {
        if ( m_TimeManager == null ) m_TimeManager = TimeManager.Instance;
        this.m_tileStatus = tileStatus;
        Material materialSwitch = m_defaultMaterial;

        switch(tileStatus)
        {
            case  TileStatus.WATERED:
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
                this.m_plantGrowHandler = GetComponentInChildren<PlantGrowHandler>();
                this.m_Time = m_TimeManager.GetCurrentTimeStamp();
                break;
        }

        GetComponent<Renderer>().material = materialSwitch;

    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        this.timeElapsed = GameTimeStamp.CompareTimeStamps(m_Time, timeStamp);
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
                    this.m_TimeManager.UnRegisterListener(this);
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
            if (this.timeElapsed > m_plantGrowHandler.m_SeedData.daytoGrow && this.CompareTag(Utils.TILE_WET_TAG))
            {
                this.m_plantGrowHandler.GrowProgression();
                this.SwitchStatus(TileStatus.HOED);
            }
            else if(this.timeElapsed > 35 && this.CompareTag(Utils.TILE_TAG))
            {
                this.m_TimeManager.UnRegisterListener(this);
                Destroy(this.transform.GetChild(0).gameObject);
            }
        }

    }
}
