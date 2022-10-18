using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Citizen
{

    public class Citizen : NPCManager
    {

        public ServedFood ServedFood { get => m_ServedFood; set => m_ServedFood = value; }
        public bool TravelBackwards { get => m_TravelBackwards; set => m_TravelBackwards = value; }
        public Waypoint CurrentWaypoint { get => m_CurrentWaypoint; set => m_CurrentWaypoint = value; }

        private Waypoint m_CurrentWaypoint;
        private ServedFood m_ServedFood;
        private readonly IdleState m_IdleState = new();
        private bool m_TravelBackwards;
        private void OnEnable()
        {
            ChangeState( m_IdleState );
        }

        private void Update()
        {
            m_CurrentState.OnUpdateState( this );
        }

        public Waypoint DetermineNextWaypoint()
            => m_TravelBackwards == true ? m_CurrentWaypoint.previousWaypoint : m_CurrentWaypoint.nextWayPoint;


    }
}

