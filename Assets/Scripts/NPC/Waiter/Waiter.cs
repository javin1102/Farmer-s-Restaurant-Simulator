using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Waiter
{
    [RequireComponent( typeof( NavMeshAgent ) )]
    public class Waiter : NPCManager
    {
        public NavMeshAgent Agent => m_Agent;
        public Vector3 InitPos { get => m_InitPos; }
        public RestaurantManager Restaurant { get => m_Restaurant; }
        public KeyValuePair<Seat, ServedFood> FoodToServe { get => m_FoodToServe; set => m_FoodToServe =  value ; }

        private NavMeshAgent m_Agent;
        private RestaurantManager m_Restaurant;
        private Vector3 m_InitPos;
        private KeyValuePair<Seat, ServedFood> m_FoodToServe;
        //States
        private readonly IdleState m_IdleState = new();

        private new void Start()
        {
            base.Start();
            m_InitPos = transform.position;
            m_Restaurant = RestaurantManager.Instance;
            m_Agent = GetComponent<NavMeshAgent>();
            m_Restaurant.Waiters.Add( this );
            m_CurrentState = m_IdleState;
            m_CurrentState.OnEnterState( this );
        }

        private void Update()
        {
            m_CurrentState.OnUpdateState( this );
        }


    }
}

