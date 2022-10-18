using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class ExtensionMethods
{
    public static bool HasReachedDestination( this NavMeshAgent agent ) => agent.remainingDistance - .5f  <= agent.stoppingDistance;
    public static Vector3 Round(this Vector3 vec)
    {
        vec.x = Mathf.Round( vec.x );
        vec.y = Mathf.Round( vec.y);
        vec.z = Mathf.Round( vec.z );
        return vec;
    }

    public static void SetLayer( this Transform trans, int layer )
    {
        trans.gameObject.layer = layer;
        foreach ( Transform child in trans )
            child.SetLayer( layer );
    }
}
