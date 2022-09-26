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
    // private GameTimeStamp timeHoed;
    private GameTimeStamp m_time;
    //  private GameTimeStamp timePlanted;

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
                break;
            case TileStatus.HOED:
                materialSwitch = m_hoedMaterial;
                this.tag = Utils.TILE_TAG;
                m_time = TimeManager.Instance.GetGameTimeStamp();
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
            if (this.timeElapsed > 36)
            {
                this.m_timeManager.UnRegisterListener(this);
                Destroy(this.gameObject);
            }
        }
        if (this.m_tileStatus == TileStatus.WATERED)
        {
            Debug.Log(timeElapsed);
            if (this.timeElapsed > 18)
            {
                this.SwitchStatus(TileStatus.HOED);
            }
        }
    }
}
