using Cinemachine;
using System;
using System.Collections;
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
    private float m_MinTimeSpawnCooldown;
    public event UnityAction OnStartLoading;
    public event UnityAction OnFinishLoading;
    [SerializeField] private PlayerAction m_PlayerAction;
    [SerializeField] private PlayerSpawnPosData m_SpawnPosData;
    [SerializeField] private Volume m_PostProcessingVolume;
    [SerializeField] private VolumeProfile m_CityProfile, m_HouseProfile;
    [SerializeField] private SPAWN_TYPE m_SpawnType = SPAWN_TYPE.HOUSE_BED;
    [SerializeField] private CinemachineVirtualCamera m_VCam;
    private FirstPersonMovement m_FirstPersonMovement;
    private Camera m_MainCam;
    private UIManager m_UIManager;
    private void Awake()
    {
        if (m_Instance == null) m_Instance = this;
        else Destroy(gameObject);
        m_MainCam = Camera.main;
    }

    private void Start()
    {
        m_PlayerAction = PlayerAction.Instance;
        m_UIManager = UIManager.Instance;
        m_FirstPersonMovement = m_PlayerAction.GetComponent<FirstPersonMovement>();
        StartCoroutine(Init());
        // SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    void OnDestroy()
    {
        // SceneManager.activeSceneChanged -= ActiveSceneChanged;
    }

    private void ActiveSceneChanged(Scene arg0, Scene arg1)
    {
        DeterminePlayerSpawnPos();
    }

    private void Update()
    {
        if (m_MinTimeSpawnCooldown <= 0)
        {
            m_MinTimeSpawnCooldown = 0;
            return;
        }

        m_MinTimeSpawnCooldown -= Time.deltaTime;
    }
    public void LoadSceneAsynchronous(string scene, LoadSceneMode loadSceneMode) => StartCoroutine(LoadSceneAsync_Coroutine(scene, loadSceneMode));
    public void UnloadSceneAsynchronous(string scene) => StartCoroutine(UnloadSceneAsync_Coroutine(scene));
    public void SpawnToScene(SPAWN_TYPE spawnType)
    {
        if (m_MinTimeSpawnCooldown > 0) return;
        StartCoroutine(SpawnToScene_Coroutine(spawnType));
    }
    private IEnumerator UnloadSceneAsync_Coroutine(string scene)
    {
        OnStartLoading?.Invoke();
        m_UIManager.LoadingUI.Activate(0);
        m_PlayerAction.OnEnableOtherUI?.Invoke();
        AsyncOperation operation = SceneManager.UnloadSceneAsync(scene);
        while (!operation.isDone)
        {
            yield return null;
        }
        m_UIManager.LoadingUI.Deactivate();
        m_PlayerAction.OnDisableOtherUI?.Invoke();
        OnFinishLoading?.Invoke();
    }
    private IEnumerator LoadSceneAsync_Coroutine(string scene, LoadSceneMode loadSceneMode)
    {
        m_PlayerAction.OnEnableOtherUI?.Invoke();
        OnStartLoading?.Invoke();
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene, loadSceneMode);
        while (!operation.isDone)
        {
            yield return null;
        }
        m_PlayerAction.OnDisableOtherUI?.Invoke();
        OnFinishLoading?.Invoke();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
    }
    private IEnumerator LoadSceneAsync_Coroutine(string scene, bool setActiveScene, LoadSceneMode loadSceneMode, SPAWN_TYPE spawnType)
    {
        m_SpawnType = spawnType;
        m_PlayerAction.OnEnableOtherUI?.Invoke();

        OnStartLoading?.Invoke();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            if (s == SceneManager.GetSceneByName(scene))
            {
                m_UIManager.LoadingUI.Deactivate();
                m_PlayerAction.OnDisableOtherUI?.Invoke();
                OnFinishLoading?.Invoke();
                // DeterminePlayerSpawnPos();
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
                yield break;
            }

        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene, loadSceneMode);
        while (!operation.isDone)
        {
            yield return null;
        }

        m_PlayerAction.OnDisableOtherUI?.Invoke();
        OnFinishLoading?.Invoke();
        if (setActiveScene) SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
        // DeterminePlayerSpawnPos();
    }

    private IEnumerator SpawnToScene_Coroutine(SPAWN_TYPE spawnType)
    {
        if (m_MinTimeSpawnCooldown > 0) yield break;
        m_FirstPersonMovement.enabled = false;
        m_PlayerAction.OnEnableOtherUI?.Invoke();
        m_UIManager.LoadingUI.Activate(0);
        OnStartLoading?.Invoke();
        m_SpawnType = spawnType;
        string scene = DetermineScene(spawnType);
        DeterminePlayerSpawnPos();
        yield return new WaitForSeconds(.5f);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
        m_UIManager.LoadingUI.Deactivate();
        m_PlayerAction.OnDisableOtherUI?.Invoke();
        OnFinishLoading?.Invoke();
        m_FirstPersonMovement.enabled = true;

    }
    private string DetermineScene(SPAWN_TYPE spawnType)
    {
        if (spawnType == SPAWN_TYPE.HOUSE_BED || spawnType == SPAWN_TYPE.HOUSE_DOOR)
        {
            return Utils.SCENE_HOUSE;
        }

        if (spawnType == SPAWN_TYPE.CITY_DOOR || spawnType == SPAWN_TYPE.FARM_CITY)
        {
            return Utils.SCENE_CITY;
        }

        if (spawnType == SPAWN_TYPE.CITY_FARM)
        {
            return Utils.SCENE_FARM;
        }

        return null;
    }

    private void DeterminePlayerSpawnPos()
    {

        // m_VCam.PreviousStateIsValid = false;
        switch (m_SpawnType)
        {
            case SPAWN_TYPE.HOUSE_BED:
                m_PlayerAction.transform.position = m_SpawnPosData.houseBedSpawnTf.position;
                m_PlayerAction.transform.eulerAngles = m_SpawnPosData.houseBedSpawnTf.eulerAngles;
                m_VCam.ForceCameraPosition(m_SpawnPosData.houseBedSpawnTf.position, Quaternion.Euler(m_SpawnPosData.houseBedSpawnTf.eulerAngles));
                m_PostProcessingVolume.profile = m_HouseProfile;
                m_MainCam.cullingMask = Utils.HouseMask;
                break;
            case SPAWN_TYPE.HOUSE_DOOR:
                m_PlayerAction.transform.position = m_SpawnPosData.houseDoorSpawnTf.position;
                m_PlayerAction.transform.eulerAngles = m_SpawnPosData.houseDoorSpawnTf.eulerAngles;
                m_VCam.ForceCameraPosition(m_SpawnPosData.houseDoorSpawnTf.position, Quaternion.Euler(m_SpawnPosData.houseDoorSpawnTf.eulerAngles));
                m_PostProcessingVolume.profile = m_HouseProfile;
                m_MainCam.cullingMask = Utils.HouseMask;
                break;
            case SPAWN_TYPE.CITY_DOOR:
                m_PlayerAction.transform.position = m_SpawnPosData.cityDoorSpawnTf.position;
                m_PlayerAction.transform.eulerAngles = m_SpawnPosData.cityDoorSpawnTf.eulerAngles;
                m_VCam.ForceCameraPosition(m_SpawnPosData.cityDoorSpawnTf.position, Quaternion.Euler(m_SpawnPosData.cityDoorSpawnTf.eulerAngles));
                m_PostProcessingVolume.profile = m_CityProfile;
                m_MainCam.cullingMask = Utils.CityMask;
                break;
            case SPAWN_TYPE.CITY_FARM:
                m_PlayerAction.transform.position = m_SpawnPosData.cityFarmSpawnTf.position;
                m_PlayerAction.transform.eulerAngles = m_SpawnPosData.cityFarmSpawnTf.eulerAngles;
                m_VCam.ForceCameraPosition(m_SpawnPosData.cityFarmSpawnTf.position, Quaternion.Euler(m_SpawnPosData.cityFarmSpawnTf.eulerAngles));
                m_PostProcessingVolume.profile = m_CityProfile;
                m_MainCam.cullingMask = Utils.FarmMask;
                break;
            case SPAWN_TYPE.FARM_CITY:
                m_PlayerAction.transform.position = m_SpawnPosData.farmCitySpawnTf.position;
                m_PlayerAction.transform.eulerAngles = m_SpawnPosData.farmCitySpawnTf.eulerAngles;
                m_VCam.ForceCameraPosition(m_SpawnPosData.cityFarmSpawnTf.position, Quaternion.Euler(m_SpawnPosData.farmCitySpawnTf.eulerAngles));
                m_PostProcessingVolume.profile = m_CityProfile;
                m_MainCam.cullingMask = Utils.CityMask;
                break;

        }
        // m_VCam.PreviousStateIsValid = true;
        m_MinTimeSpawnCooldown = 1;

    }

    private IEnumerator UnloadAndLoadSceneAsync_Coroutine(string unloadScene, string loadScene, LoadSceneMode loadSceneMode, SPAWN_TYPE spawnType)
    {
        yield return StartCoroutine(UnloadSceneAsync_Coroutine(unloadScene));
        yield return StartCoroutine(LoadSceneAsync_Coroutine(loadScene, true, loadSceneMode, spawnType));
    }

    private IEnumerator LoadAllGameScene()
    {

        yield return StartCoroutine(LoadSceneAsync_Coroutine(Utils.SCENE_HOUSE, true, LoadSceneMode.Additive, SPAWN_TYPE.HOUSE_BED));
        yield return StartCoroutine(LoadSceneAsync_Coroutine(Utils.SCENE_CITY, false, LoadSceneMode.Additive, SPAWN_TYPE.CITY_DOOR));
        yield return StartCoroutine(LoadSceneAsync_Coroutine(Utils.SCENE_FARM, false, LoadSceneMode.Additive, SPAWN_TYPE.CITY_FARM));

    }

    private IEnumerator Init()
    {
        m_UIManager.LoadingUI.Activate(0);
        yield return StartCoroutine(LoadAllGameScene());
        yield return new WaitForEndOfFrame();
        SpawnToScene(SPAWN_TYPE.HOUSE_BED);
        m_PlayerAction.transform.position = m_SpawnPosData.houseBedSpawnTf.position;
        m_PlayerAction.transform.eulerAngles = m_SpawnPosData.houseBedSpawnTf.eulerAngles;
        m_VCam.ForceCameraPosition(m_SpawnPosData.houseBedSpawnTf.position, Quaternion.Euler(m_SpawnPosData.houseBedSpawnTf.eulerAngles));
        m_UIManager.LoadingUI.Deactivate();

    }
}
