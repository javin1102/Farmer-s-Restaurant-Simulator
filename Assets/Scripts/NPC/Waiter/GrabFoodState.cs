using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC.Waiter
{
    public class GrabFoodState : NPCBaseState
    {
        private Waiter m_Waiter;
        private bool m_HasFood;
        private KeyValuePair<Seat, ServedFood> m_Food;
        private ServedFood ServedFood => m_Food.Value;
        private readonly ServeState m_ServeState = new();
        public override void OnEnterState( NPCManager NPC )
        {
            m_Waiter = NPC as Waiter;
            m_Food = m_Waiter.FoodsToServe.Peek();
            m_Waiter.Agent.SetDestination( ServedFood.foodGO.transform.position );
        }

        public override void OnExitState( NPCManager NPC )
        {
            
        }

        public override void OnUpdateState( NPCManager NPC )
        {
            if ( m_Waiter.Agent.isStopped )
            {
                ServedFood.foodGO.transform.SetParent( m_Waiter.transform );
                //TODO::Set food position on waiter
                //TODO::Set anim
                NPC.ChangeState( m_ServeState );
            }
        }
    }

}
