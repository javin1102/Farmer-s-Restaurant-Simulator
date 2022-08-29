using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCBaseState 
{
    public abstract void OnEnterState(NPCManager NPC);
    public abstract void OnUpdateState( NPCManager NPC );
    public abstract void OnExitState( NPCManager NPC );
}
