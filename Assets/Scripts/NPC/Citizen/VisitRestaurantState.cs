using System.Collections;
using UnityEngine;

namespace NPC.Citizen
{
    public class VisitRestaurantState : NPCBaseState
    {
        private RestaurantManager m_Restaurant;
        private Citizen m_Citizen;
        private Seat m_Seat;
        private bool m_HasOrder, m_IsEating;
        private FoodData m_Food;
        public override void OnEnterState( NPCManager NPC )
        {
            m_Restaurant = RestaurantManager.Instance;
            m_Citizen = NPC as Citizen;
            if (!m_Restaurant.TryGetRecipeToCook( out m_Food ) || !m_Restaurant.FindUnoccupiedSeat( out m_Seat ) || !FindSeatDest( m_Citizen, m_Seat ) )
            {
                TravelState travelState = new();
                m_Citizen.ChangeState( travelState );
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
            GameObject.Destroy( m_Citizen.ServedFood.foodGO );
            m_HasOrder = false;
            m_Seat = null;
            m_Citizen.ServedFood = null;
            m_IsEating = false;

        }

        public override void OnUpdateState( NPCManager NPC )
        {
            if ( m_Seat == null || m_Seat.Table == null )
            {
                m_Citizen.ChangeState( new TravelState() );
                return;
            }
            if ( !m_HasOrder && !m_Citizen.Agent.pathPending && m_Citizen.Agent.HasReachedDestination() )
            {
                m_Restaurant.OrderFood( m_Seat, m_Food );
                m_HasOrder = true;
                //Switch to sit animation
            }


            if ( !m_IsEating && m_Citizen.ServedFood != null )
            {
                NPC.StartCoroutine( EatFinish( 10 ) );
                m_IsEating = true;
                //Switch to eat animation
            }

            //TODO::Finish Eating -> leave
            

        }

        private bool FindSeatDest( Citizen citizen, Seat seat )
        {
            if ( seat == null ) return false;
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

        private IEnumerator EatFinish( float delay )
        {
            yield return new WaitForSeconds( delay );
            TravelState travelState = new();
            m_Citizen.ChangeState( travelState );

        }
    }
}

