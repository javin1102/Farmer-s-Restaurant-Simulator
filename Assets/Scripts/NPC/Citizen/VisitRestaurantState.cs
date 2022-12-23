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
        private UIManager m_UIManager;
        private bool m_PathIsInvalid => m_Citizen.Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid || m_Citizen.Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial;
        public override void OnEnterState(NPCManager NPC)
        {
            m_UIManager = UIManager.Instance;
            m_Restaurant = RestaurantManager.Instance;
            m_Citizen = NPC as Citizen;

            if (!m_Restaurant.TryGetFoodToCook(out m_FoodData, out m_FoodConfig) || !m_Restaurant.FindUnoccupiedSeat(out m_Seat) || !FindSeatDest(m_Citizen, m_Seat))
            {
                TravelState travelState = new();
                m_Citizen.ChangeState(travelState);
                return;
            }

            m_Citizen.Animator.SetTrigger(Utils.NPC_WALK_ANIM_PARAM);
        }

        public override void OnExitState(NPCManager NPC)
        {
            m_Citizen.StopAudio();
            m_Citizen.transform.position = m_LastPos;
            m_Citizen.Agent.enabled = true;
            if (m_Seat != null)
            {
                m_Seat.Citizen = null;
                m_Seat.IsOccupied = false;
                //m_Restaurant.Seats.Add( m_Seat );
            }

            if (m_Citizen.ServedFood != null)
            {
                PlayerAction.Instance.IncreaseCoins(m_Citizen.ServedFood.Data.dishPrice);
                m_UIManager.NotificationQueue.Enqueue($"<color=yellow>+{m_Citizen.ServedFood.Data.dishPrice}</color> Koin");
                GameObject.Destroy(m_Citizen.ServedFood.gameObject);
            }

            m_HasOrder = false;
            m_Seat = null;
            m_Citizen.ServedFood = null;
            m_IsEating = false;

        }

        public override void OnUpdateState(NPCManager NPC)
        {
            if (m_Seat == null || m_Seat.Table == null || (!m_HasOrder && !m_FoodConfig.IsSelling) || m_WaitTime <= 0)
            {
                NPC.StopAllCoroutines();
                m_Citizen.ChangeState(new TravelState());
                return;
            }

            if (m_StartWaitTimer && !m_IsEating) m_WaitTime -= Time.deltaTime;

            if (!m_IsSitting && !m_Citizen.Agent.pathPending && m_Citizen.Agent.HasReachedDestination())
            {
                m_Citizen.Agent.enabled = false;
                m_LastPos = m_Citizen.transform.position;
                m_Citizen.transform.position = m_Seat.SitTf.position;
                m_Citizen.transform.forward = m_Seat.transform.forward;
                m_Citizen.Animator.SetTrigger(Utils.NPC_SIT_ANIM_PARAM);
                m_StartWaitTimer = true;
                m_IsSitting = true;
            }

            if (m_IsSitting && !m_HasOrder && m_Restaurant.AnyChefHasStove())
            {
                m_Restaurant.OrderFood(m_Seat, m_FoodData);
                m_HasOrder = true;
            }


            if (!m_IsEating && m_Citizen.ServedFood != null)
            {
                NPC.StartCoroutine(EatFinish(10));
                m_IsEating = true;
                m_Citizen.Animator.SetTrigger(Utils.NPC_EAT_ANIM_PARAM);
                m_Citizen.PlayAudio("plateclack_sfx");
            }

        }

        private bool FindSeatDest(Citizen citizen, Seat seat)
        {
            if (seat == null || seat.IsOccupied || m_PathIsInvalid) return false;
            if (citizen.Agent.SetDestination(seat.transform.position))
            {
                seat.IsOccupied = true;
                seat.Citizen = m_Citizen;
                return true;
            }


            //All seat has no path, go to exit state
            return false;
        }

        private IEnumerator EatFinish(float delay)
        {
            yield return new WaitForSeconds(delay);
            m_Citizen.StartCoroutine("CoinTipDrop");
            TravelState travelState = new();
            m_Citizen.ChangeState(travelState);

        }
    }
}

