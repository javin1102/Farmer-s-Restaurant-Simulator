using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC
{
    public abstract class NPCBaseState
    {
        public abstract void OnEnterState( NPCManager NPC );
        public abstract void OnUpdateState( NPCManager NPC );
        public abstract void OnExitState( NPCManager NPC );
    }

}
