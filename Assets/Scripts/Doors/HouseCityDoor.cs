using UnityEngine.SceneManagement;

public class HouseCityDoor : Door
{
    protected override void OnInteract()
    {
        m_SceneLoader.LoadSceneAsynchronous( Utils.SCENE_CITY, LoadSceneMode.Additive, SPAWN_TYPE.CITY_DOOR );
    }

    protected override void ShowHelper()
    {
        m_UIManager.ShowActionHelperPrimary( "Left", "Ke Kota" );
    }
}

