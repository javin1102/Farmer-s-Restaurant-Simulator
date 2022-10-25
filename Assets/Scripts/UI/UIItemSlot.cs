using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public abstract class UIItemSlot : MonoBehaviour, IDropHandler
{
    public ItemSlot Slot { get => m_SlotsController.Slots[m_SlotIndex]; }

    [SerializeField] protected Image m_IconImage;
    [SerializeField] protected TMP_Text m_QuantityText;
    [SerializeField] protected int m_SlotIndex;
    protected ItemSlotsController m_SlotsController;
    void Start()
    {
        m_SlotIndex = transform.GetSiblingIndex();
    }
    protected void Update()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        if ( Slot == null || Slot.quantity == 0 )
        {
            transform.GetChild( 0 ).gameObject.SetActive( false );
            return;
        }
        transform.GetChild( 0 ).gameObject.SetActive( true );
        m_IconImage.sprite = Slot.data.icon;
        m_QuantityText.text = Slot.quantity.ToString();
    }

    public void OnDrop( PointerEventData eventData )
    {
        Transform targetContentTf = eventData.pointerDrag.transform;
        UIItemSlot OriginUISlot = targetContentTf.parent.GetComponent<UIItemSlot>();
        SwapItemSlot( OriginUISlot );

        targetContentTf.GetComponent<UIDraggable>().OnEndDrag( eventData );
    }

    protected void SwapItemSlot( UIItemSlot ui )
    {
        (m_SlotsController.Slots[m_SlotIndex], ui.m_SlotsController.Slots[ui.m_SlotIndex]) = (ui.m_SlotsController.Slots[ui.m_SlotIndex], m_SlotsController.Slots[m_SlotIndex]);
    }


}


