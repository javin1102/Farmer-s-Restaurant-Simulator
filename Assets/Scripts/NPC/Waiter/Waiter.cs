using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Waiter
{
    [RequireComponent( typeof( NavMeshAgent ) )]
    public class Waiter : NPCManager
    {
        public NavMeshAgent Agent => m_Agent;
        public Queue<KeyValuePair<Seat, ServedFood>> FoodsToServe => m_FoodsToServe;
        public Vector3 InitPos { get => m_InitPos; }

        private NavMeshAgent m_Agent;
        private readonly Queue<KeyValuePair<Seat, ServedFood>> m_FoodsToServe = new();
        private RestaurantManager m_Restaurant;
        private Vector3 m_InitPos;
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

