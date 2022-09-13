using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Citizen
{
    public class TravelState : NPCBaseState
    {
        private Citizen m_Citizen;
        private CitizenSpawner m_Spawner;
        private bool hasDetermine;
        public override void OnEnterState( NPCManager NPC )
        {
            m_Citizen = NPC as Citizen;
            m_Citizen.currentWaypoint = m_Citizen.DetermineNextWaypoint();
            m_Citizen.Agent.SetDestination( m_Citizen.currentWaypoint.GetPosition() );
            m_Spawner = BaseSpawner.Instance as CitizenSpawner;
        }

        public override void OnExitState( NPCManager NPC )
        {
            m_Spawner.ReleaseNPC( NPC );
        }

        public override void OnUpdateState( NPCManager NPC )
        {
            if ( !hasDetermine && m_Citizen.Agent.HasReachedDestination() )
            {

                if ( ( m_Citizen.TravelBackwards == false && m_Citizen.currentWaypoint.nextWayPoint == null )
                    || ( m_Citizen.TravelBackwards == true && m_Citizen.currentWaypoint.previousWaypoint == null ) )
                {
                    NPC.ChangeState( null );
                    return;
                }

                m_Citizen.currentWaypoint = m_Citizen.DetermineNextWaypoint();
                m_Citizen.Agent.SetDestination( m_Citizen.currentWaypoint.GetPosition() );
                hasDetermine = true;
                m_Citizen.StartCoroutine( ResetAfter( 1 ) );
            }
        }

        private IEnumerator ResetAfter( float delay )
        {
            yield return new WaitForSeconds( delay );
            hasDetermine = false;
        }
    }
}

