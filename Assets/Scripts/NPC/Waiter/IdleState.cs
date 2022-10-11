using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Waiter
{
    public class IdleState : NPCBaseState
    {
        private Waiter m_Waiter;
        public override void OnEnterState( NPCManager NPC )
        {
            m_Waiter = NPC as Waiter;
            m_Waiter.Agent.SetDestination( m_Waiter.InitPos );

        }

        public override void OnExitState( NPCManager NPC )
        {
        }

        public override void OnUpdateState( NPCManager NPC )
        {
            if ( !m_Waiter.Agent.pathPending && m_Waiter.Agent.HasReachedDestination() )
            {
                m_Waiter.IsServing = false;
                m_Waiter.Hoverable.IsHoverable = true;
            }
            if ( !m_Waiter.Restaurant.FoodsToServe.TryPeek( out KeyValuePair<Seat, ServedFood> food ) ) return;
            m_Waiter.FoodToServe = food;
            m_Waiter.Restaurant.FoodsToServe.Dequeue();
            GrabFoodState grabFoodState = new();
            NPC.ChangeState( grabFoodState );
        }
    }

}
