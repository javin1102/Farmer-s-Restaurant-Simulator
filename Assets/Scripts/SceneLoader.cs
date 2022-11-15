using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
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

    [SerializeField] private Volume m_PostProcessingVolume;
    [SerializeField] private VolumeProfile m_CityProfile, m_HouseProfile;
    [SerializeField] private SPAWN_TYPE m_SpawnType = SPAWN_TYPE.HOUSE_BED;
    [SerializeField] private CinemachineVirtualCamera m_VCam;
    private FirstPersonMovement m_FirstPersonMovement;
    private Camera m_MainCam;
    private void Awake()
    {
        if ( m_Instance == null ) m_Instance = this;
        else Destroy( gameObject );
        DontDestroyOnLoad( this );

        m_MainCam = Camera.main;
    }

    private void Start()
    {
        m_FirstPersonMovement = m_PlayerAction.GetComponent<FirstPersonMovement>();
        StartCoroutine( InitializeScene() );
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

    public void UnloadAndLoadSceneAsynchronous( string unloadScene, string loadScene, LoadSceneMode loadSceneMode, SPAWN_TYPE spawnType ) => StartCoroutine( UnloadAndLoadSceneAsync( unloadScene, loadScene, loadSceneMode, spawnType ) );
    public void LoadSceneAsynchronous( string scene, LoadSceneMode loadSceneMode, SPAWN_TYPE spawnType ) => StartCoroutine( LoadSceneAsync( scene, loadSceneMode, spawnType ) );
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

    private IEnumerator LoadSceneAsync( string scene, LoadSceneMode loadSceneMode, SPAWN_TYPE spawnType )
    {
        m_SpawnType = spawnType;
        m_PlayerAction.OnEnableUI?.Invoke();
        m_LoadingUI.SetActive( true );
        OnStartLoading?.Invoke();

        for ( int i = 0; i < SceneManager.sceneCount; i++ )
        {
            Scene s = SceneManager.GetSceneAt( i );
            if ( s == SceneManager.GetSceneByName( scene ) )
            {
                m_LoadingUI.SetActive( false );
                m_PlayerAction.OnDisableUI?.Invoke();
                OnFinishLoading?.Invoke();
                DeterminePlayerSpawnPos();
                SceneManager.SetActiveScene( SceneManager.GetSceneByName( scene ) );
                yield break;
            }

        }

        AsyncOperation operation = SceneManager.LoadSceneAsync( scene, loadSceneMode );
        while ( !operation.isDone )
        {
            yield return null;
        }
        m_LoadingUI.SetActive( false );
        m_PlayerAction.OnDisableUI?.Invoke();
        OnFinishLoading?.Invoke();
        SceneManager.SetActiveScene( SceneManager.GetSceneByName( scene ) );
        DeterminePlayerSpawnPos();
    }

    private void DeterminePlayerSpawnPos()
    {
        m_FirstPersonMovement.enabled = false;
        switch ( m_SpawnType )
        {
            case SPAWN_TYPE.HOUSE_BED:
                m_PlayerAction.transform.position = m_SpawnPosData.houseBedSpawnTf.position;
                m_VCam.ForceCameraPosition( m_SpawnPosData.houseBedSpawnTf.position, Quaternion.Euler( m_SpawnPosData.houseBedSpawnTf.eulerAngles ) );
                m_PostProcessingVolume.profile = m_HouseProfile;
                m_MainCam.cullingMask = Utils.HouseMask;
                break;
            case SPAWN_TYPE.HOUSE_DOOR:
                m_PlayerAction.transform.position = m_SpawnPosData.houseDoorSpawnTf.position;
                m_VCam.ForceCameraPosition( m_SpawnPosData.houseDoorSpawnTf.position, Quaternion.Euler( m_SpawnPosData.houseDoorSpawnTf.eulerAngles ) );
                m_PostProcessingVolume.profile = m_HouseProfile;
                m_MainCam.cullingMask = Utils.HouseMask;
                break;
            case SPAWN_TYPE.CITY_DOOR:
                m_PlayerAction.transform.position = m_SpawnPosData.cityDoorSpawnTf.position;
                m_VCam.ForceCameraPosition( m_SpawnPosData.cityDoorSpawnTf.position, Quaternion.Euler( m_SpawnPosData.cityDoorSpawnTf.eulerAngles ) );
                m_PostProcessingVolume.profile = m_CityProfile;
                m_MainCam.cullingMask = Utils.CityMask;
                break;
            case SPAWN_TYPE.CITY_FARM:
                m_PlayerAction.transform.position = m_SpawnPosData.farmSpawnTf.position;
                m_VCam.ForceCameraPosition( m_SpawnPosData.farmSpawnTf.position, Quaternion.Euler( m_SpawnPosData.farmSpawnTf.eulerAngles ) );
                m_PostProcessingVolume.profile = m_CityProfile;
                m_MainCam.cullingMask = Utils.FarmMask;
                break;
        }

        m_FirstPersonMovement.enabled = true;
    }

    private IEnumerator UnloadAndLoadSceneAsync( string unloadScene, string loadScene, LoadSceneMode loadSceneMode, SPAWN_TYPE spawnType )
    {
        yield return StartCoroutine( UnloadSceneAsync( unloadScene ) );
        yield return StartCoroutine( LoadSceneAsync( loadScene, loadSceneMode, spawnType ) );
    }

    private IEnumerator InitializeScene()
    {
        yield return StartCoroutine( LoadSceneAsync( Utils.SCENE_CITY, LoadSceneMode.Additive, SPAWN_TYPE.HOUSE_BED ) );
        yield return StartCoroutine( LoadSceneAsync( Utils.SCENE_HOUSE, LoadSceneMode.Additive, SPAWN_TYPE.HOUSE_BED ) );
        yield return StartCoroutine( LoadSceneAsync( Utils.SCENE_FARM, LoadSceneMode.Additive, SPAWN_TYPE.HOUSE_BED ) );
    }
}
