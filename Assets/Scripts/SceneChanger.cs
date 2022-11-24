using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private SPAWN_TYPE m_SpawnType;
    private SceneLoader m_SceneLoader;
    private void Start()
    {
        m_SceneLoader = SceneLoader.Instance;
    }
    private void OnTriggerEnter( Collider other )
    {
        if ( other.CompareTag( Utils.PLAYER_TAG ) )
        {
            m_SceneLoader.SpawnToScene( m_SpawnType );
        }
    }
}
