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
        public override void OnEnterState(NPCManager NPC)
        {
            m_Chef = NPC as Chef;
            m_Food = m_Chef.OrderedFood;
            m_CookTime = m_Food.Value.cookDuration;
            m_Chef.Animator.SetBool(Utils.NPC_COOKING_ANIM_PARAM, true);
            m_Chef.Hoverable.IsHoverable = false;
            m_Chef.UIChef.gameObject.SetActive(true);
            m_Chef.UIChef.SetIcon(m_Food.Value.icon);
        }

        public override void OnExitState(NPCManager NPC)
        {
        }

        public override void OnUpdateState(NPCManager NPC)
        {
            if (m_Chef.Stove == null)
            {
                IdleState idleState = new();
                NPC.ChangeState(idleState);
                m_Chef.Restaurant.OrderQueue.Enqueue(m_Food);
                return;
            }

            if (m_Food.Key == null || m_Food.Key.Citizen == null)
            {
                IdleState idleState = new();
                NPC.ChangeState(idleState);
                return;
            }

            m_Chef.transform.forward = m_Chef.Stove.transform.forward;
            //TODO::Switch to cook anim
            m_Chef.UIChef.SetFillAmount(m_CookTime, m_Food.Value.cookDuration);
            if (m_CookTime <= 0)
            {
                m_CookTime = 0;
                Food food = GameObject.Instantiate(m_Food.Value.foodPrefab).GetComponent<Food>();
                food.Seat = m_Food.Key;
                //m_Chef.Restaurant.FoodsToServe.Enqueue( food );
                food.transform.position = food.Seat.Table.GetFoodPlace(food.Seat).position;
                food.Seat.Citizen.ServedFood = food;
                m_Chef.Restaurant.DecreaseStock(m_Food.Value);
                IdleState idleState = new();
                NPC.ChangeState(idleState);
                return;
            }



            m_CookTime -= Time.deltaTime;
        }

        private float RandomZ => Random.Range(m_FoodPlace.z - 2.5f, m_FoodPlace.z + 2.5f);
        private float RandomX => Random.Range(m_FoodPlace.x - 1, m_FoodPlace.x + 1);
        private Vector3 RandomPos()
        {
            m_FoodPos.Set(RandomX, m_FoodPlace.y + .5f, RandomZ);
            return m_FoodPos;
        }

    }
}

