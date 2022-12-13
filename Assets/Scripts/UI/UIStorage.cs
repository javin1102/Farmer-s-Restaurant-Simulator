using UnityEngine;
using DG.Tweening;
public class UIStorage : MonoBehaviour
{
    private PlayerAction m_PlayerAction;
    private void Start()
    {
        m_PlayerAction = PlayerAction.Instance;
        transform.DOLocalMoveY(1.25f, 2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    void LateUpdate()
    {
        Vector3 dir = (transform.position - m_PlayerAction.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
