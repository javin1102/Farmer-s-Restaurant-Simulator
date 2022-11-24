using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionTime
{
    abstract public float ActionTime { get; set; }
    abstract public float DefaultActionTime { get; }

    void OnHoldMainAction( PlayerAction playerAction );
    void OnReleaseMainAction( PlayerAction playerAction );
}
