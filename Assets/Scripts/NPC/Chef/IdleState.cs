using System.Collections.Generic;
namespace NPC.Chef
{
    public class IdleState : NPCBaseState
    {
        private Chef m_Chef;
        public override void OnEnterState(NPCManager NPC)
        {
            m_Chef = NPC as Chef;
            m_Chef.Animator.SetBool(Utils.NPC_COOKING_ANIM_PARAM, false);
            m_Chef.UIChef.DisableTimerUI();
            m_Chef.StopAudio();
        }

        public override void OnExitState(NPCManager NPC)
        {
        }

        public override void OnUpdateState(NPCManager NPC)
        {
            if (m_Chef.Stove == null)
            {
                m_Chef.Hoverable.IsHoverable = true;
                return;
            }
            m_Chef.transform.forward = m_Chef.Stove.transform.forward;
            if (!m_Chef.Restaurant.OrderQueue.TryPeek(out KeyValuePair<Seat, FoodData> food)) return;
            m_Chef.OrderedFood = food;
            m_Chef.Restaurant.OrderQueue.Dequeue();
            CookState cookState = new();
            NPC.ChangeState(cookState);
        }
    }
}

