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
        [SerializeField] protected GameObject[] m_Prefabs;
        [SerializeField] protected int m_SpawnSize = 20;
        protected ObjectPool<NPCManager> m_Pool;
        protected bool m_HasSpawn;
        private static BaseSpawner m_Instance;
        private Vector2 delayTimeRange;


        private void Awake()
        {
            if ( m_Instance == null ) m_Instance = this;
            int i = 0;
            m_Pool = new( () => Instantiate( m_Prefabs[i++ % m_Prefabs.Length] ).GetComponent<NPCManager>(), npc => Get( npc ), npc => npc.gameObject.SetActive( false ), npc => Destroy( npc.gameObject ), false, m_SpawnSize, 60 );
        }

        private void Update()
        {
            if ( m_Pool.CountActive >= m_SpawnSize ) return;
            delayTimeRange = Time.time < 30 ? new Vector2( .5f, 1.5f ) : new Vector2( 1, 5 );
            if ( !m_HasSpawn )
            {
                StartCoroutine( SpawnWithDelay( Random.Range( delayTimeRange.x, delayTimeRange.y ) ) );
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
