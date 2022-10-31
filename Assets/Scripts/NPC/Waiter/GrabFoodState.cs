using UnityEngine;
namespace NPC.Waiter
{
    public class GrabFoodState : NPCBaseState
    {
        private Waiter m_Waiter;
        private Food m_Food;
        public override void OnEnterState( NPCManager NPC )
        {
            m_Waiter = NPC as Waiter;
            m_Food = m_Waiter.FoodToServe;
            if ( m_Food.Seat == null || m_Food.Seat.Citizen == null )
            {
                GameObject.Destroy( m_Food.gameObject );
                NPC.ChangeState( new IdleState() );
                return;
            }
            m_Waiter.Agent.SetDestination( m_Food.transform.position );
            m_Waiter.IsServing = true;
            m_Waiter.Hoverable.IsHoverable = false;
        }

        public override void OnExitState( NPCManager NPC )
        {

        }

        public override void OnUpdateState( NPCManager NPC )
        {
            if ( m_Food.Seat == null || m_Food.Seat.Citizen == null )
            {
                GameObject.Destroy( m_Food.gameObject );
                NPC.ChangeState( new IdleState() );
                return;
            }

            if ( !m_Waiter.Agent.pathPending && m_Waiter.Agent.HasReachedDestination() )
            {
                m_Food.transform.SetParent( m_Waiter.transform );
                //TODO::Set food position on waiter
                //TODO::Set anim
                ServeState serveState = new();
                NPC.ChangeState( serveState );
            }
        }
    }

}
