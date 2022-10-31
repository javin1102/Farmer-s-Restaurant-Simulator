using NPC.Citizen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Waiter
{
    public class ServeState : NPCBaseState
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
            m_Waiter.Agent.SetDestination( m_Food.Seat.transform.position );
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
                //Serve Food -> place on the table
                m_Food.transform.SetParent( m_Food.Seat.transform );
                m_Food.transform.forward = m_Food.Seat.transform.forward;
                m_Food.Seat.Citizen.ServedFood = m_Food;
                //TODO::Set food pos on table
                Transform foodTf = m_Food.Seat.Table.GetFoodPlace( m_Food.Seat );
                m_Food.transform.position = foodTf.transform.position;
                IdleState idleState = new();
                NPC.ChangeState( idleState );
            }


        }
    }

}
