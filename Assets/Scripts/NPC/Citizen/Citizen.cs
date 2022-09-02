using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Citizen
{
    [RequireComponent( typeof( NavMeshAgent ) )]
    public class Citizen : NPCManager
    {
        public NavMeshAgent Agent { get => m_Agent; }
        public ServedFood ServedFood { get => m_ServedFood; set => m_ServedFood = value; }
        //Debug
        public Vector3 initPos;
        private ServedFood m_ServedFood;
        private NavMeshAgent m_Agent;
        private readonly IdleState m_IdleState = new();
        private new void Start()
        {
            base.Start();
            m_Agent = GetComponent<NavMeshAgent>();
            initPos = transform.position;
            ChangeState( m_IdleState );
        }

        private void Update()
        {
            m_CurrentState.OnUpdateState( this );
        }

    }
}

