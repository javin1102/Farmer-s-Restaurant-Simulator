using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public static Test test;
    public Queue<int> x = new();
    private void Awake()
    {
        if ( test == null ) test = this;
        x.Enqueue( 1 );
        x.Enqueue( 2 );
    }
}
