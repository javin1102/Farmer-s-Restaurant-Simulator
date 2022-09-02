using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Citizen
{
    public class TravelState : NPCBaseState
    {
        private Citizen m_Citizen;
        public override void OnEnterState( NPCManager NPC )
        {
            m_Citizen = NPC as Citizen;
            m_Citizen.Agent.SetDestination( m_Citizen.initPos );
        }

        public override void OnExitState( NPCManager NPC )
        {
        }

        public override void OnUpdateState( NPCManager NPC )
        {
            if ( m_Citizen.Agent.remainingDistance <= m_Citizen.Agent.stoppingDistance )
            {
                IdleState idleState = new();
                NPC.ChangeState( idleState );
            }
        }
    }
}

