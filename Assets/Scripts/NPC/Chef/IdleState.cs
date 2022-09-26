using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC.Chef
{
    public class IdleState : NPCBaseState
    {
        private Chef m_Chef;
        public override void OnEnterState( NPCManager NPC )
        {
            m_Chef = NPC as Chef;
            //Set to idle anim
        }

        public override void OnExitState( NPCManager NPC )
        {
        }

        public override void OnUpdateState( NPCManager NPC )
        {
            if ( !m_Chef.OrderQueue.TryPeek( out _ ) ) return;
            CookState cookState = new();
            NPC.ChangeState( cookState );
            Debug.Log( "Cook State" );
        }
    }
}

