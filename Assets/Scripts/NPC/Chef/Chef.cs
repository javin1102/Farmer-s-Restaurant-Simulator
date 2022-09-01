using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace NPC.Chef
{
    public class Chef : NPCManager
    {
        public Queue<KeyValuePair<Seat, FoodData>> OrderQueue => m_OrderQueue;


        private readonly Queue<KeyValuePair<Seat,FoodData>> m_OrderQueue = new();
        private RestaurantManager m_Restaurant;
        private readonly CookState m_CookState = new();
        private readonly IdleState m_IdleState = new();
        private new void Start()
        {
            base.Start();
            m_Restaurant = RestaurantManager.Instance;
            m_Restaurant.Chefs.Add( this );
        }



        private void Update()
        {
            if ( m_OrderQueue.Count <= 0 )
            {
                ChangeState( m_IdleState );
                return;
            }

            ChangeState( m_CookState );
            m_CurrentState.OnUpdateState( this );
        }

    }
}

