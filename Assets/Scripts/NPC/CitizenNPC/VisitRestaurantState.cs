using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Citizen
{
    public class VisitRestaurantState : NPCBaseState
    {
        private RestaurantManager m_Restaurant;
        private Citizen m_Citizen;
        private Seat m_Seat;
        private bool m_HasOrder;
        public override void OnEnterState( NPCManager NPC )
        {
            m_Restaurant = RestaurantManager.Instance;
            m_Citizen = NPC as Citizen;
            if ( !m_Restaurant.FindUnoccupiedSeat( out m_Seat ) || !FindSeatDest( m_Citizen, m_Seat ) )
            {
                //TODO::Can't find unoccupied seat, or all seats has no path -> Exit State
                return;
            }

        }

        public override void OnExitState( NPCManager NPC )
        {
            if ( m_Seat != null )
            {
                m_Restaurant.UnoccupiedSeats.Add( m_Seat );
            }
            
            if ( m_Citizen.ServedFood == null ) return;

            //TODO::ADD PLAYER MONEY FROM SERVED FOOD

            m_HasOrder = false;
            m_Seat = null;
            m_Citizen.ServedFood = null;

        }

        public override void OnUpdateState( NPCManager NPC )
        {
            if ( m_Seat == null ) return;
            if ( !m_HasOrder && m_Citizen.Agent.isStopped )
            {
                m_Restaurant.OrderFood( m_Seat );
                m_HasOrder = true;
                //Switch to sit animation
            }

            
            if ( m_Citizen.ServedFood != null )
            {
                //Switch to eat animation
            }
        }

        private bool FindSeatDest( Citizen citizen, Seat seat )
        {
            if ( citizen.Agent.SetDestination( seat.transform.position ) )
            {
                seat.IsOccupied = true;
                m_Restaurant.UnoccupiedSeats.Remove( seat );
                seat.Citizen = m_Citizen;
                return true;
            }

            Debug.Log( "NOPE" );
            //All seat has no path, go to exit state
            return false;
        }
    }
}

