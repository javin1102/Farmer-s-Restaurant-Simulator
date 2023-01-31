using TMPro;
using UnityEngine;
using DG.Tweening;
public class UIInventoryTooltip : MonoBehaviour
{
    private RectTransform m_RectTf;
    [SerializeField] private TMP_Text m_NameText;
    private CanvasGroup m_CanvasGroup;
    void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_RectTf = GetComponent<RectTransform>();
    }
    void OnEnable()
    {
        DOTween.To(x => m_CanvasGroup.alpha = x, 0, 1, .25f).SetEase(Ease.InCirc);
    }
    public void UpdateUI(string name)
    {
        m_NameText.text = name;
    }

    public void SetPos(Vector3 pos)
    {
        m_RectTf = m_RectTf != null ? m_RectTf : GetComponent<RectTransform>();
        float posX = pos.x + m_RectTf.rect.width / 2;
        float posY = pos.y - (m_RectTf.rect.height / 2 - 10);
        m_RectTf.anchoredPosition = new Vector2(posX, posY);
    }
}
