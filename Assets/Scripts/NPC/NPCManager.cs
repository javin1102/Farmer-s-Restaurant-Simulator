using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class NPCManager : MonoBehaviour
{
    private NavMeshAgent m_Agent;

    public NavMeshAgent Agent { get => m_Agent; }

    // Start is called before the first frame update
    protected void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
