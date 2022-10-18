using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    [RequireComponent( typeof( NavMeshAgent ) )]
    public abstract class NPCManager : MonoBehaviour
    {
        public NavMeshAgent Agent { get => m_Agent; }
        public Animator Animator { get => m_Animator; }

        protected NPCBaseState m_CurrentState;
        protected NavMeshAgent m_Agent;
        protected Animator m_Animator;
        // Start is called before the first frame update
        protected void Awake()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponent<Animator>();
        }

        public void ChangeState( NPCBaseState state )
        {
            if ( state == m_CurrentState ) return;
            m_CurrentState?.OnExitState( this );
            m_CurrentState = state;
            m_CurrentState?.OnEnterState( this );
        }
    }
}