using UnityEngine;
namespace NPC
{
    public abstract class NPCManager : MonoBehaviour
    {
        protected NPCBaseState m_CurrentState;

        // Start is called before the first frame update
        protected void Start()
        {
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