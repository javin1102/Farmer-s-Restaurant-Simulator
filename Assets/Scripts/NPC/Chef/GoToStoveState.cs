using UnityEngine;

namespace NPC.Chef
{
    public class GoToStoveState : NPCBaseState
    {
        private Chef m_Chef;
        public override void OnEnterState( NPCManager NPC )
        {
            m_Chef = NPC as Chef;
            m_Chef.Agent.SetDestination( m_Chef.Stove.ChefStandTf.position );
            m_Chef.Hoverable.IsHoverable = false;
        }

        public override void OnExitState( NPCManager NPC )
        {
        }

        public override void OnUpdateState( NPCManager NPC )
        {
            
            if ( !m_Chef.Agent.pathPending && m_Chef.Agent.HasReachedDestination() )
            {
                m_Chef.transform.forward = m_Chef.Stove.transform.forward;
                NPC.ChangeState( new IdleState() );
            }
        }
    }

}
