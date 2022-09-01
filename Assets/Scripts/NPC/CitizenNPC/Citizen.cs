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
        private ServedFood m_ServedFood;
        private readonly VisitRestaurantState m_VisitRestaurantState = new();
        private NavMeshAgent m_Agent;
        private bool m_IsEating;
        [SerializeField] private bool visit;
        private new void Start()
        {
            base.Start();
            m_Agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if ( visit )
            {
                ChangeState( m_VisitRestaurantState );
                m_CurrentState.OnUpdateState( this );
            }
            else
            {
                m_CurrentState?.OnExitState( this );
                m_CurrentState = null;
            }
        }

    }
}

