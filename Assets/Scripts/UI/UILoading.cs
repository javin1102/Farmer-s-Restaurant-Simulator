using UnityEngine;
using DG.Tweening;
public class UILoading : MonoBehaviour
{
    private CanvasGroup m_CanvasGroup;
    void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }
    void OnEnable()
    {

    }

    public void Activate(float timeSeconds = .25f) => DOTween.To(x => m_CanvasGroup.alpha = x, 0, 1, timeSeconds);
    public void Deactivate() => m_CanvasGroup.alpha = 0;
}
