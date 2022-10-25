using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu( fileName = "New Item Channel", menuName = "Channel/Item Main Action" )]
public class ItemMainActionChannel : ScriptableObject
{
    public event UnityAction OnMainAction;

    public void RaiseEvent()
    {
        OnMainAction?.Invoke();
    }
}
