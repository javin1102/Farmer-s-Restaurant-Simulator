using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    private Test test;
    void Start()
    {
        test = Test.test;
    }

    // Update is called once per frame
    void Update()
    {
        if ( !test.x.TryPeek( out _ ) ) return;
        Debug.Log( transform.name );
        test.x.Dequeue();
    }
}
