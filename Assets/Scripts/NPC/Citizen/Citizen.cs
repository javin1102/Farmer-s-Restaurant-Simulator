using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Citizen
{

    public class Citizen : NPCManager
    {

        public Food ServedFood { get => m_ServedFood; set => m_ServedFood = value; }
        public bool TravelBackwards { get => m_TravelBackwards; set => m_TravelBackwards = value; }
        public Waypoint CurrentWaypoint { get => m_CurrentWaypoint; set => m_CurrentWaypoint = value; }

        [SerializeField] private Waypoint m_CurrentWaypoint;
        private Food m_ServedFood;
        [SerializeField] private bool m_TravelBackwards;
        private void OnEnable()
        {
            ChangeState( new IdleState() );
        }

        private void Update()
        {
            m_CurrentState.OnUpdateState( this );
        }

        public Waypoint DetermineNextWaypoint()
            => m_TravelBackwards == true ? m_CurrentWaypoint.previousWaypoint : m_CurrentWaypoint.nextWayPoint;


    }
}

