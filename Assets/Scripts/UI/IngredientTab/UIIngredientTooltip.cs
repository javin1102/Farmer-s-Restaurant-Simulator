using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIIngredientTooltip : MonoBehaviour
{
    public bool IsHovered { get => m_IsHovered; }
    [SerializeField] private TMP_Text m_NameText;
    [SerializeField] private TMP_Text m_KandunganText;
    [SerializeField] private TMP_Text m_NutrisiText;
    [SerializeField] private TMP_Text m_ManfaatText;

    [SerializeField] private RectTransform m_ContentTf;
    private RectTransform m_ParentTf;
    private VerticalLayoutGroup m_ContentLayoutGroup;
    private RectTransform m_RectTf;
    private bool m_IsHovered;


    private void Start()
    {
        m_ContentLayoutGroup = m_ContentTf.GetComponent<VerticalLayoutGroup>();
        m_ParentTf = m_ContentTf.parent.GetComponent<RectTransform>();
        m_RectTf = GetComponent<RectTransform>();

    }

    private void Update()
    {
        float height = m_ContentLayoutGroup.preferredHeight > m_ParentTf.rect.height ? m_ContentLayoutGroup.preferredHeight : m_ParentTf.rect.height;
        m_ContentTf.sizeDelta = new Vector2( m_ContentTf.sizeDelta.x, height );
    }

    public void UpdateUI( string name, string kandungan, string nutrisi, string manfaat )
    {
        m_NameText.text = name;
        m_KandunganText.text = kandungan;
        m_NutrisiText.text = nutrisi;
        m_ManfaatText.text = manfaat;
    }

    public void SetPos( Vector3 pos )
    {
        m_RectTf = m_RectTf != null ? m_RectTf : GetComponent<RectTransform>();
        float posX = pos.x + m_RectTf.rect.width / 2;
        float posY = pos.y - ( m_RectTf.rect.height / 2 - 30 );
        m_RectTf.anchoredPosition = new Vector2( posX, posY );
    }

}
