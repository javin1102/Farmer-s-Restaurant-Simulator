using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC.Chef
{
    public class CookState : NPCBaseState
    {
        private Chef m_Chef;
        private KeyValuePair<Seat, FoodData> m_Food;
        private float m_CookTime;
        private Vector3 m_FoodPos = new();
        private Vector3 m_FoodPlace;
        private RestaurantManager m_Restaurant;
        private bool m_HasFood;
        public override void OnEnterState( NPCManager NPC )
        {
            m_Chef = NPC as Chef;
            m_Restaurant = RestaurantManager.Instance;
            m_FoodPlace = m_Restaurant.FoodPlace.position;
            if ( m_HasFood = m_Chef.OrderQueue.TryPeek( out m_Food ) ) m_CookTime = m_Food.Value.cookDuration;
        }

        public override void OnExitState( NPCManager NPC )
        {
        }

        public override void OnUpdateState( NPCManager NPC )
        {
            if ( !m_HasFood ) return;
            //TODO::Switch to cook anim
            if ( m_CookTime <= 0 )
            {
                m_CookTime = 0;
                GameObject foodGO = GameObject.Instantiate( m_Food.Value.foodPrefab, RandomPos(), Quaternion.identity );
                ServedFood servedFood = new( foodGO, m_Food.Value.price );
                m_Restaurant.Waiters[m_Restaurant.WaiterIndex].FoodsToServe.Enqueue( KeyValuePair.Create( m_Food.Key, servedFood ) );
                m_Chef.OrderQueue.Dequeue();
                OnEnterState( NPC );
                return;
            }

            m_CookTime -= Time.deltaTime;
        }

        private float RandomZ => Random.Range( m_FoodPlace.z - 2.5f, m_FoodPlace.z + 2.5f );
        private float RandomX => Random.Range( m_FoodPlace.x - 1, m_FoodPlace.x + 1 );
        private Vector3 RandomPos()
        {
            m_FoodPos.Set( RandomX, m_FoodPlace.y + .5f, RandomZ );
            return m_FoodPos;
        }

    }
}

