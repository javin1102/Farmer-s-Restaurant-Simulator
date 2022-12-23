using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;

public class UIInventorySlot : UIItemSlot, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public UnityAction<ItemData, Vector3> OnHoverEnter { get => m_OnHoverEnter; set => m_OnHoverEnter = value; }
    public UnityAction OnHoverExit { get => m_OnHoverExit; set => m_OnHoverExit = value; }

    private Button m_Button;
    private UIInventoryController m_UIInventoryController;
    private Image m_Image;
    private UnityAction<ItemData, Vector3> m_OnHoverEnter;
    private UnityAction m_OnHoverExit;
    private RectTransform m_RectTf;
    private PlayerUpgrades m_PlayerUpgrades;
    private void Awake()
    {
        m_SlotsController = transform.root.GetComponent<InventorySlotsController>();
        m_UIInventoryController = transform.parent.parent.GetComponent<UIInventoryController>();
        m_PlayerUpgrades = transform.root.GetComponent<PlayerUpgrades>();
        m_Image = GetComponent<Image>();
        m_Button = GetComponent<Button>();
        m_RectTf = GetComponent<RectTransform>();
        m_Button.onClick.AddListener(Select);

    }
    private new void Start()
    {
        base.Start();
        if (m_SlotIndex > m_PlayerUpgrades.GetSlotSize() - 1) gameObject.SetActive(false);
    }
    private new void Update()
    {
        base.Update();
        if (Slot == null) ResetSprite();
        if (m_UIInventoryController.SelectedSlot == this && Slot == null) m_UIInventoryController.SelectedSlot = null;
    }
    private void Select()
    {
        if (m_UIInventoryController.SelectedSlot != null) m_UIInventoryController.SelectedSlot.ResetSprite();
        m_UIInventoryController.SelectedSlot = this;
        if (m_SlotsController.Slots[m_SlotIndex] == null)
        {
            m_UIInventoryController.DisableExtraUI();
            return;
        }
        if (Slot.data.decreaseable)
            m_UIInventoryController.EnableExtraUI();
        UseSelectedSprite();
    }

    public void ResetSprite()
    {
        m_Image.sprite = m_UIInventoryController.UnselectedSprite;
    }

    public void UseSelectedSprite()
    {
        m_Image.sprite = m_UIInventoryController.SelectedSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Slot == null || UIDraggable.isDragging) return;
        m_UIInventoryController.ShowTooltip(Slot.data, m_RectTf.anchoredPosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_UIInventoryController.DisableTooltip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_UIInventoryController.DisableTooltip();
    }

}




