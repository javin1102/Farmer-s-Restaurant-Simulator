using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIActionSlot : MonoBehaviour, IDropHandler
{
    private ItemSlotData m_ItemSlotData;
    [SerializeField] private Image m_IconImage;
    [SerializeField] private TMP_Text m_QuantityText;
    public ItemSlotData ItemSlotData { get => m_ItemSlotData; set => m_ItemSlotData = value; }

    public void OnDrop( PointerEventData eventData )
    {
        Transform target = eventData.pointerDrag.transform;
        target.SetParent( transform );
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void UpdateUI()
    {
        m_IconImage.sprite = m_ItemSlotData.itemData.icon;
        m_QuantityText.text = m_ItemSlotData.quantity.ToString();
    }

}
