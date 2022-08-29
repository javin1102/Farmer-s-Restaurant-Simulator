using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : NPCManager
{
    private NPCBaseState m_CurrentState;
    private readonly VisitRestaurantState m_VisitRestaurantState = new();
    [SerializeField] private bool visit;
    private new void Start()
    {
        base.Start();
    }


    private void Update()
    {
        if ( visit )
        {
            ChangeState( m_VisitRestaurantState );
            m_CurrentState.OnUpdateState( this );
        }
        else
        {
            m_CurrentState?.OnExitState( this );
            m_CurrentState = null;
        }

        
    }
    private void ChangeState( NPCBaseState state )
    {
        if ( state == m_CurrentState ) return;
        m_CurrentState = state;
        m_CurrentState.OnEnterState( this );
    }
}
