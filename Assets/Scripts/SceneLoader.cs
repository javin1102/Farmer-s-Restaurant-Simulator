using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public enum SPAWN_TYPE
{
    HOUSE_BED,
    HOUSE_DOOR,
    CITY_DOOR,
    CITY_FARM,
    FARM_CITY

}

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get => m_Instance; }
    public SPAWN_TYPE SpawnType { get => m_SpawnType; set => m_SpawnType = value; }

    private static SceneLoader m_Instance;
    public event UnityAction OnStartLoading;
    public event UnityAction OnFinishLoading;
    [SerializeField] private PlayerAction m_PlayerAction;
    [SerializeField] private PlayerSpawnPosData m_SpawnPosData;
    [SerializeField] private GameObject m_LoadingUI;
    private SPAWN_TYPE m_SpawnType = SPAWN_TYPE.HOUSE_BED;
    private void Awake()
    {
        if ( m_Instance == null ) m_Instance = this;
        else Destroy( gameObject );
        DontDestroyOnLoad( this );
    }

    private void Start()
    {
        m_SpawnType = SPAWN_TYPE.HOUSE_BED;
        LoadSceneAsynchronous( Utils.SCENE_HOUSE, LoadSceneMode.Additive );
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded( Scene scene, LoadSceneMode mode )
    {

    }

    public void UnloadAndLoadSceneAsynchronous( string unloadScene, string loadScene, LoadSceneMode loadSceneMode ) => StartCoroutine( UnloadAndLoadSceneAsync( unloadScene, loadScene, loadSceneMode ) );
    public void LoadSceneAsynchronous( string scene, LoadSceneMode loadSceneMode ) => StartCoroutine( LoadSceneAsync( scene, loadSceneMode ) );
    public void UnloadSceneAsynchronous( string scene ) => StartCoroutine( UnloadSceneAsync( scene ) );
    private IEnumerator UnloadSceneAsync( string scene )
    {
        OnStartLoading?.Invoke();
        m_LoadingUI.SetActive( true );
        m_PlayerAction.OnEnableUI?.Invoke();
        AsyncOperation operation = SceneManager.UnloadSceneAsync( scene );
        while ( !operation.isDone )
        {
            yield return null;
        }
        m_LoadingUI.SetActive( false );
        m_PlayerAction.OnDisableUI?.Invoke();
        OnFinishLoading?.Invoke();
    }
    private IEnumerator LoadSceneAsync( string scene, LoadSceneMode loadSceneMode )
    {
        OnStartLoading?.Invoke();
        m_LoadingUI.SetActive( true );
        m_PlayerAction.OnEnableUI?.Invoke();
        AsyncOperation operation = SceneManager.LoadSceneAsync( scene, loadSceneMode );
        while ( !operation.isDone )
        {
            yield return null;
        }
        DeterminePlayerSpawnPos();
        SceneManager.SetActiveScene( SceneManager.GetSceneByName( scene ) );
        m_LoadingUI.SetActive( false );
        m_PlayerAction.OnDisableUI?.Invoke();
        OnFinishLoading?.Invoke();
    }

    private void DeterminePlayerSpawnPos()
    {
        switch ( m_SpawnType )
        {
            case SPAWN_TYPE.HOUSE_BED:
                m_PlayerAction.transform.position = m_SpawnPosData.houseBedSpawnTf.position;
                m_PlayerAction.transform.eulerAngles = m_SpawnPosData.houseBedSpawnTf.eulerAngles;
                break;
            case SPAWN_TYPE.HOUSE_DOOR:
                m_PlayerAction.transform.position = m_SpawnPosData.houseDoorSpawnTf.position;
                m_PlayerAction.transform.eulerAngles = m_SpawnPosData.houseDoorSpawnTf.eulerAngles;
                break;
            case SPAWN_TYPE.CITY_DOOR:
                m_PlayerAction.transform.position = m_SpawnPosData.cityDoorSpawnTf.position;
                m_PlayerAction.transform.eulerAngles = m_SpawnPosData.cityDoorSpawnTf.eulerAngles;
                break;
        }
    }

    private IEnumerator UnloadAndLoadSceneAsync( string unloadScene, string loadScene, LoadSceneMode loadSceneMode )
    {
        yield return StartCoroutine( UnloadSceneAsync( unloadScene ) );
        yield return StartCoroutine( LoadSceneAsync( loadScene, loadSceneMode ) );

    }
}
