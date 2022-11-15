using System.Collections;
using UnityEngine;

namespace NPC.Citizen
{
    public class VisitRestaurantState : NPCBaseState
    {
        private RestaurantManager m_Restaurant;
        private Citizen m_Citizen;
        private Seat m_Seat;
        private bool m_HasOrder, m_IsEating, m_IsSitting;
        private FoodData m_FoodData;
        private FoodConfig m_FoodConfig;
        private bool m_StartWaitTimer;
        private float m_WaitTime = 30f;
        private Vector3 m_LastPos;
        private bool m_PathIsInvalid => m_Citizen.Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid || m_Citizen.Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial;
        public override void OnEnterState( NPCManager NPC )
        {
            m_Restaurant = RestaurantManager.Instance;
            m_Citizen = NPC as Citizen;
            if ( !m_Restaurant.TryGetFoodToCook( out m_FoodData, out m_FoodConfig ) || !m_Restaurant.FindUnoccupiedSeat( out m_Seat ) || !FindSeatDest( m_Citizen, m_Seat ) )
            {
                TravelState travelState = new();
                m_Citizen.ChangeState( travelState );
                return;
            }

        }

        public override void OnExitState( NPCManager NPC )
        {
            m_Citizen.transform.position = m_LastPos;
            m_Citizen.Agent.enabled = true;
            if ( m_Seat != null )
            {
                m_Seat.Citizen = null;
                m_Seat.IsOccupied = false;
                m_Restaurant.UnoccupiedSeats.Add( m_Seat );
            }

            //TODO::HAS Served food -> ADD PLAYER MONEY FROM SERVED FOOD
            if ( m_Citizen.ServedFood != null )
            {
                GameObject.Destroy( m_Citizen.ServedFood.gameObject );
            }

            m_HasOrder = false;
            m_Seat = null;
            m_Citizen.ServedFood = null;
            m_IsEating = false;

        }

        public override void OnUpdateState( NPCManager NPC )
        {
            if (  m_Seat == null || m_Seat.Table == null || ( !m_HasOrder && !m_FoodConfig.IsSelling ) || m_WaitTime <= 0 )
            {
                NPC.StopAllCoroutines();
                m_Citizen.ChangeState( new TravelState() );
                return;
            }

            if ( m_StartWaitTimer && !m_IsEating ) m_WaitTime -= Time.deltaTime;

            if ( !m_IsSitting && !m_Citizen.Agent.pathPending && m_Citizen.Agent.HasReachedDestination() )
            {
                m_Citizen.Agent.enabled = false;
                m_LastPos = m_Citizen.transform.position;
                m_Citizen.transform.position = m_Seat.SitTf.position;
                m_Citizen.transform.forward = m_Seat.transform.forward;
                m_Citizen.Animator.SetTrigger( Utils.NPC_SIT_ANIM_PARAM );
                m_StartWaitTimer = true;
                m_IsSitting = true;
            }

            if ( m_IsSitting && !m_HasOrder && m_Restaurant.AnyChefHasStove() )
            {
                m_Restaurant.OrderFood( m_Seat, m_FoodData );
                m_HasOrder = true;
            }


            if ( !m_IsEating && m_Citizen.ServedFood != null )
            {
                NPC.StartCoroutine( EatFinish( 5 ) );
                m_IsEating = true;
                m_Citizen.Animator.SetTrigger( Utils.NPC_EAT_ANIM_PARAM );
            }

        }

        private bool FindSeatDest( Citizen citizen, Seat seat )
        {
            if ( seat == null || seat.IsOccupied || m_PathIsInvalid ) return false;
            if ( citizen.Agent.SetDestination( seat.transform.position ) )
            {
                seat.IsOccupied = true;
                m_Restaurant.UnoccupiedSeats.Remove( seat );
                seat.Citizen = m_Citizen;
                return true;
            }


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

