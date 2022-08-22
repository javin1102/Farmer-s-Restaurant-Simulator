using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public abstract class UIItemSlot : MonoBehaviour, IDropHandler
{
    public ItemSlot ItemSlot { get => m_ItemSlot; set => m_ItemSlot = value; }
    public int SlotIndex { get => m_SlotIndex; }
    [SerializeReference] protected ItemSlot m_ItemSlot;
    [SerializeField] protected Image m_IconImage;
    [SerializeField] protected TMP_Text m_QuantityText;
    [SerializeField] protected int m_SlotIndex;
    protected ItemSlotsController m_ItemsController;
   
    protected virtual void Awake()
    {
        m_SlotIndex = transform.GetSiblingIndex();
    }

    protected virtual void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if ( m_ItemSlot == null || m_ItemSlot.quantity == 0 )
        {
            m_ItemSlot = null;
            transform.GetChild( 0 ).gameObject.SetActive( false );
            return;
        }
        transform.GetChild( 0 ).gameObject.SetActive( true );
        m_IconImage.sprite = m_ItemSlot.data.icon;
        m_QuantityText.text = m_ItemSlot.quantity.ToString();

    }


    public void SetReference( Image iconImage, TMP_Text quantityText )
    {
        m_IconImage = iconImage;
        m_QuantityText = quantityText;
    }

    public void OnDrop( PointerEventData eventData )
    {
        Transform targetContentTf = eventData.pointerDrag.transform;
        UIItemSlot OriginUISlot = targetContentTf.parent.GetComponent<UIItemSlot>();
        OnDropAction( OriginUISlot );
        
        UpdateUI();
        OriginUISlot.UpdateUI();
        targetContentTf.GetComponent<UIDraggable>().OnEndDrag( eventData );
    }

    protected void SwapUIItemSlot( UIItemSlot UIItemSlot )
    {
        (m_ItemSlot, UIItemSlot.ItemSlot) = (UIItemSlot.ItemSlot, m_ItemSlot);
    }

    protected abstract void OnDropAction( UIItemSlot originItemSlot );
}
