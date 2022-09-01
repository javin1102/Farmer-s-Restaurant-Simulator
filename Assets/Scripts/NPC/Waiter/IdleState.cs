using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Waiter
{
    public class IdleState : NPCBaseState
    {
        private Waiter m_Waiter;
        private readonly GrabFoodState m_GrabFoodState = new();
        public override void OnEnterState( NPCManager NPC )
        {
            m_Waiter = NPC as Waiter;
        }

        public override void OnExitState( NPCManager NPC )
        {
        }

        public override void OnUpdateState( NPCManager NPC )
        {
            if ( m_Waiter.FoodsToServe.TryPeek( out _ ) ) NPC.ChangeState( m_GrabFoodState );
        }
    }

}
