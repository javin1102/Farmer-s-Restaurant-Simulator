using UnityEngine;
using UnityEngine.SceneManagement;

public class CityDoor : Door
{

    protected override void OnInteract()
    {
        m_SceneLoader.LoadSceneAsynchronous( Utils.SCENE_HOUSE, LoadSceneMode.Additive, SPAWN_TYPE.HOUSE_DOOR );
    }

    protected override void ShowHelper()
    {
        m_UIManager.ShowActionHelperPrimary( "Left", "Masuk Rumah" );
    }
}
