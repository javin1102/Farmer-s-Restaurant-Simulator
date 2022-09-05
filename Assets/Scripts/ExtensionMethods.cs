using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class ExtensionMethods
{
    public static bool HasReachedDestination( this NavMeshAgent agent ) => agent.remainingDistance <= agent.stoppingDistance;
}
