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
            m_Citizen.Agent.SetDestination( m_Citizen.currentWaypoint.GetPosition() );
        }

        public override void OnExitState( NPCManager NPC )
        {
        }

        public override void OnUpdateState( NPCManager NPC )
        {
            if ( m_Citizen.Agent.HasReachedDestination() )
            {
                m_Citizen.currentWaypoint = m_Citizen.currentWaypoint.nextWayPoint;
                m_Citizen.Agent.SetDestination( m_Citizen.currentWaypoint.GetPosition() );
            }
        }
    }
}

