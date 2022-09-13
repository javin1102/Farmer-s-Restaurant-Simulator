using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace NPC
{
    public abstract class BaseSpawner : MonoBehaviour
    {
        public static BaseSpawner Instance { get => m_Instance; }
        //Debug
        [SerializeField] protected GameObject m_Prefab;
        [SerializeField] protected List<Transform> m_Waypoints;
        [SerializeField] protected int m_SpawnSize = 20;
        protected ObjectPool<NPCManager> m_Pool;
        protected bool m_HasSpawn;
        private static BaseSpawner m_Instance;


        private void Awake()
        {
            if ( m_Instance == null ) m_Instance = this;
            m_Pool = new( () => Instantiate( m_Prefab ).GetComponent<NPCManager>(), npc => Get( npc ), npc => npc.gameObject.SetActive( false ), npc => Destroy( npc.gameObject ), false, m_SpawnSize, 60 );

            //for ( int i = 0; i < m_SpawnSize - 15; i++ )
            //{
            //    StartCoroutine( SpawnWithDelay( 1 ));
            //}
        }

        private void Update()
        {
            if ( m_Pool.CountActive >= m_SpawnSize ) return;
            if ( !m_HasSpawn )
            {
                StartCoroutine( SpawnWithDelay( Random.Range( 1, 10 ) ) );
            }
        }


        protected abstract void Get( NPCManager npc );
        public void ReleaseNPC( NPCManager npc ) => m_Pool.Release( npc );

        IEnumerator SpawnWithDelay( float delay )
        {
            m_HasSpawn = true;
            yield return new WaitForSeconds( delay );
            m_Pool.Get();
            m_HasSpawn = false;
        }

    }


}
