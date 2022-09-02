using NPC.Citizen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Waiter
{
    public class ServeState : NPCBaseState
    {
        private Waiter m_Waiter;
        private KeyValuePair<Seat, ServedFood> m_Food;
        private GameObject FoodGO => m_Food.Value.foodGO;
        private Seat Seat => m_Food.Key;

        public override void OnEnterState( NPCManager NPC )
        {
            m_Waiter = NPC as Waiter;
            m_Food = m_Waiter.FoodsToServe.Peek();
            m_Waiter.Agent.SetDestination( Seat.transform.position );
        }

        public override void OnExitState( NPCManager NPC )
        {
        }

        public override void OnUpdateState( NPCManager NPC )
        {
            if ( m_Waiter.Agent.remainingDistance <= m_Waiter.Agent.stoppingDistance )
            {
                //Serve Food -> place on the table
                FoodGO.transform.SetParent( Seat.transform );
                FoodGO.transform.forward = Seat.transform.forward;
                Seat.Citizen.ServedFood = m_Food.Value;

                //TODO::Set food pos on table
                m_Waiter.FoodsToServe.Dequeue();
                if ( !m_Waiter.FoodsToServe.TryPeek( out _ ) ) {
                    IdleState idleState = new();
                    NPC.ChangeState( idleState );
                } 
                else
                {
                    GrabFoodState grabFoodState = new();
                    NPC.ChangeState( grabFoodState );
                }
            }


        }
    }

}
