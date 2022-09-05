using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Citizen
{
    public class IdleState : NPCBaseState
    {
        private Citizen m_Citizen;
        public override void OnEnterState( NPCManager NPC )
        {
            m_Citizen = NPC as Citizen;
            NPC.StartCoroutine( DetermineState( Random.Range( 1,2 ) ) );
        }

        public override void OnExitState( NPCManager NPC )
        {
        }

        public override void OnUpdateState( NPCManager NPC )
        {

        }
        IEnumerator DetermineState( float delay )
        {
            yield return new WaitForSeconds( delay );
            VisitRestaurantState visitRestaurantState = new();
            TravelState travelState = new();
            m_Citizen.ChangeState( travelState );
        }
    }



}
