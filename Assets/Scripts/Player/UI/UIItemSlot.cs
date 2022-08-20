using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public abstract class UIItemSlot : MonoBehaviour, IDropHandler
{
    public ItemSlot ItemSlotData { get => m_ItemSlotData; set => m_ItemSlotData = value; }
    public int SlotIndex { get => m_SlotIndex; set => m_SlotIndex = value; }

    [SerializeReference] protected ItemSlot m_ItemSlotData;
    [SerializeField] protected Image m_IconImage;
    [SerializeField] protected TMP_Text m_QuantityText;


    protected ItemSlotsController m_ItemsController;
    [SerializeField] protected int m_SlotIndex;

    protected virtual void OnEnable()
    {
        m_SlotIndex = transform.GetSiblingIndex();
        m_ItemsController.OnStoreItem += UpdateUI;
    }

    protected virtual void OnDisable()
    {
        m_ItemsController.OnStoreItem -= UpdateUI;
    }
    public void UpdateUI()
    {
        m_ItemSlotData = m_ItemsController.ItemSlots[m_SlotIndex];
        if ( m_ItemsController.ItemSlots[m_SlotIndex] == null )
        {
            transform.GetChild( 0 ).gameObject.SetActive( false );
            return;
        }

        
        transform.GetChild( 0 ).gameObject.SetActive( true );
        m_IconImage.sprite = m_ItemSlotData.data.icon;
        m_QuantityText.text = m_ItemSlotData.quantity.ToString();
    }


    public void SetReference( Image iconImage, TMP_Text quantityText )
    {
        m_IconImage = iconImage;
        m_QuantityText = quantityText;
    }

    public void OnDrop( PointerEventData eventData )
    {
        Transform targetContentTf = eventData.pointerDrag.transform;
        UIItemSlot OriginUIInventorySlot = targetContentTf.parent.GetComponent<UIItemSlot>();
        OnDropAction( OriginUIInventorySlot );
        UpdateUI();
        OriginUIInventorySlot.UpdateUI();
        targetContentTf.GetComponent<UIDraggable>().OnEndDrag( eventData );
    }

    protected abstract void OnDropAction( UIItemSlot originItemSlot );
}
