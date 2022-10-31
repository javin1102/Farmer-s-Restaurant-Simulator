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
        public UIChef UIChef { get => m_UIChef; }

        private RestaurantManager m_Restaurant;
        private readonly IdleState m_IdleState = new();
        private KeyValuePair<Seat, FoodData> m_OrderedFood;
        [SerializeField] private Stove m_Stove;
        [SerializeField] private UIChef m_UIChef;
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
            else m_Hoverable.IsHoverable = true;
        }


        public void Interact(PlayerAction playerAction)
        {
            if ( m_Stove != null ) return;

            //if ( !m_Hoverable.IsHoverable ) return;
            m_Agent.SetDestination( m_Restaurant.GetGroundRandPos() );

        }
    }
}

