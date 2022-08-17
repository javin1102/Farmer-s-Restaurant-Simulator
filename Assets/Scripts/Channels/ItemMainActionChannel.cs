using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu( fileName = "New Item Channel", menuName = "Create Item Channel" )]
public class ItemMainActionChannel : ScriptableObject
{
    public event UnityAction OnMainAction;

    public void RaiseEvent()
    {
        OnMainAction?.Invoke();
    }
}
