using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Waiter
{
    [RequireComponent(typeof(Hoverable))]
    public class Waiter : NPCManager, IInteractable
    {
        public Vector3 InitPos { get => m_InitPos; }
        public RestaurantManager Restaurant { get => m_Restaurant; }
        public KeyValuePair<Seat, ServedFood> FoodToServe { get => m_FoodToServe; set => m_FoodToServe =  value ; }
        public bool IsServing { get => m_IsServing; set => m_IsServing =  value ; }
        public Hoverable Hoverable { get => m_Hoverable; set => m_Hoverable =  value ; }

        private RestaurantManager m_Restaurant;
        private Vector3 m_InitPos;
        private KeyValuePair<Seat, ServedFood> m_FoodToServe;
        private readonly IdleState m_IdleState = new();
        private bool m_IsServing;
        private Hoverable m_Hoverable;
        private void Start()
        {
            m_InitPos = transform.position;
            m_Restaurant = RestaurantManager.Instance;
            m_Agent = GetComponent<NavMeshAgent>();
            m_Restaurant.Waiters.Add( this );
            m_CurrentState = m_IdleState;
            m_CurrentState.OnEnterState( this );
            m_Hoverable = GetComponent<Hoverable>();
        }

        private void Update()
        {
            m_CurrentState.OnUpdateState( this );
            m_Animator.SetFloat( Utils.NPC_SPEED_ANIM_PARAM, Mathf.Abs( m_Agent.velocity.magnitude ) );
            m_Agent.speed = m_Restaurant.WaiterMoveSpeed;
        }

        public void Interact()
        {
            if ( !m_Hoverable.IsHoverable ) return;
            float randX = Random.Range( m_Restaurant.GroundCollider.bounds.min.x, m_Restaurant.GroundCollider.bounds.max.x );
            float randZ = Random.Range( m_Restaurant.GroundCollider.bounds.min.z, m_Restaurant.GroundCollider.bounds.max.z );
            Vector3 randPos = new( randX, transform.position.y, randZ );
            m_Agent.SetDestination( randPos );
        }
    }
}

