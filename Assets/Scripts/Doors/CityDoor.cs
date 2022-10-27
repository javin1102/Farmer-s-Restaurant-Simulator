using UnityEngine;
using UnityEngine.SceneManagement;

public class CityDoor : Door
{

    protected override void OnInteract()
    {
        m_SceneLoader.SpawnType = SPAWN_TYPE.HOUSE_DOOR;
        m_SceneLoader.UnloadAndLoadSceneAsynchronous( Utils.SCENE_CITY, Utils.SCENE_HOUSE, LoadSceneMode.Additive );
    }

    protected override void ShowHelper()
    {
        m_UIManager.ShowActionHelperPrimary( "Left", "Masuk Rumah" );
    }
}
