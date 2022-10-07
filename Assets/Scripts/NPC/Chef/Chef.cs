using System.Collections.Generic;
namespace NPC.Chef
{
    public class Chef : NPCManager
    {

        public RestaurantManager Restaurant { get => m_Restaurant; }
        public KeyValuePair<Seat, FoodData> OrderedFood { get => m_OrderedFood; set => m_OrderedFood =  value ; }

        private RestaurantManager m_Restaurant;
        private readonly CookState m_CookState = new();
        private readonly IdleState m_IdleState = new();
        private KeyValuePair<Seat, FoodData> m_OrderedFood;
        private new void Start()
        {
            base.Start();
            m_Restaurant = RestaurantManager.Instance;
            m_Restaurant.Chefs.Add( this );
            m_CurrentState = m_IdleState;
            m_CurrentState.OnEnterState( this );
        }



        private void Update()
        {
            m_CurrentState.OnUpdateState( this );
        }

    }
}

