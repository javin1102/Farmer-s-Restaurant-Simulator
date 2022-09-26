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
            if ( m_Waiter.FoodsToServe.TryPeek( out _ ) ) 
            {
                GrabFoodState grabFoodState = new();
                NPC.ChangeState( grabFoodState );
            } 
        }
    }

}
