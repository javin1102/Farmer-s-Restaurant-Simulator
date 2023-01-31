using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu( fileName = "New Restaurant Channel", menuName = "Channel/Restaurant Upgrades" )]
public class RestaurantUpgradesChannel : ScriptableObject
{
    public UnityAction AddChef;
    public UnityAction AddWaiter;
    public UnityAction<float> IncreaseWaiterSpeed;

    public void RaiseAddChefEvent() => AddChef?.Invoke();
    public void RaiseAddWaiterEvent() => AddWaiter?.Invoke();
    public void RaiseIncreaseWaiterSpeedEvent( float speed ) => IncreaseWaiterSpeed?.Invoke( speed );
}
