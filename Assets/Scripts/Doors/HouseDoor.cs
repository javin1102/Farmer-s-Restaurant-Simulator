using UnityEngine.SceneManagement;

public class HouseDoor : Door
{
    protected override void OnInteract()
    {
        m_SceneLoader.SpawnType = SPAWN_TYPE.CITY_DOOR;
        m_SceneLoader.UnloadAndLoadSceneAsynchronous( Utils.SCENE_HOUSE, Utils.SCENE_CITY, LoadSceneMode.Additive );
    }

    protected override void ShowHelper()
    {
        m_UIManager.ShowActionHelperPrimary( "Left", "Ke Kota" );
    }
}

