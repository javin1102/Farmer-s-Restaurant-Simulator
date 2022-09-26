using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Citizen
{
    [RequireComponent( typeof( NavMeshAgent ) )]
    public class Citizen : NPCManager
    {
        public NavMeshAgent Agent { get => m_Agent; }
        public ServedFood ServedFood { get => m_ServedFood; set => m_ServedFood = value; }
        public bool TravelBackwards { get => m_TravelBackwards; set => m_TravelBackwards = value; }
        public Waypoint CurrentWaypoint { get => m_CurrentWaypoint; set => m_CurrentWaypoint = value; }

        private Waypoint m_CurrentWaypoint;
        //Debug
        private ServedFood m_ServedFood;
        private NavMeshAgent m_Agent;
        private readonly IdleState m_IdleState = new();
        [SerializeField] private bool m_TravelBackwards;
        private void OnEnable()
        {
            m_Agent = GetComponent<NavMeshAgent>();
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

