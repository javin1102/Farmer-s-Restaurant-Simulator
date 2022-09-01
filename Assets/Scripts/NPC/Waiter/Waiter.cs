using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Waiter
{
    [RequireComponent( typeof( NavMeshAgent ) )]
    public class Waiter : NPCManager
    {
        public NavMeshAgent Agent => m_Agent;
        public Queue<KeyValuePair<Seat, ServedFood>> FoodsToServe => m_FoodsToServe;
        public KeyValuePair<Seat, ServedFood> CurrentFood { get => m_CurrentFood; set => m_CurrentFood = value; }

        private NavMeshAgent m_Agent;
        private readonly Queue<KeyValuePair<Seat, ServedFood>> m_FoodsToServe = new();
        private RestaurantManager m_Restaurant;
        private KeyValuePair<Seat, ServedFood> m_CurrentFood;

        //States
        private readonly IdleState m_IdleState = new();

        private new void Start()
        {
            base.Start();
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

