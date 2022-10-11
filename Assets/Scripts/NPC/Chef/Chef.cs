using System.Collections.Generic;
using UnityEngine;
namespace NPC.Chef
{
    [RequireComponent( typeof( Hoverable ) )]
    public class Chef : NPCManager, IInteractable
    {
        public RestaurantManager Restaurant { get => m_Restaurant; }
        public KeyValuePair<Seat, FoodData> OrderedFood { get => m_OrderedFood; set => m_OrderedFood = value; }
        public Stove Stove { get => m_Stove; set => m_Stove = value; }
        public Hoverable Hoverable { get => m_Hoverable; }

        private RestaurantManager m_Restaurant;
        private readonly IdleState m_IdleState = new();
        private KeyValuePair<Seat, FoodData> m_OrderedFood;
        private Stove m_Stove;
        private Hoverable m_Hoverable;
        private void Start()
        {
            m_Restaurant = RestaurantManager.Instance;
            m_Hoverable = GetComponent<Hoverable>();
            m_Restaurant.Chefs.Add( this );
            m_CurrentState = m_IdleState;
            m_CurrentState.OnEnterState( this );
        }




        private void Update()
        {
            m_Animator.SetFloat( Utils.NPC_SPEED_ANIM_PARAM, Mathf.Abs( m_Agent.velocity.magnitude ) );
            m_CurrentState.OnUpdateState( this );
            if ( m_Stove ) m_Hoverable.IsHoverable = false;
        }


        public void Interact()
        {
            if ( m_Stove ) return;
            if ( !m_Hoverable.IsHoverable ) return;
            float randX = Random.Range( m_Restaurant.GroundCollider.bounds.min.x, m_Restaurant.GroundCollider.bounds.max.x );
            float randZ = Random.Range( m_Restaurant.GroundCollider.bounds.min.z, m_Restaurant.GroundCollider.bounds.max.z );
            Vector3 randPos = new( randX, transform.position.y, randZ );
            m_Agent.SetDestination( randPos );
        }
    }
}

