using UnityEngine.UI;

public class UIInventorySlot : UIItemSlot
{
    private Button m_Button;
    private UIInventoryController m_UIInventoryController;
    private Image m_Image;
    private void Awake()
    {
        m_SlotsController = transform.root.GetComponent<InventorySlotsController>();
        m_UIInventoryController  = transform.parent.parent.GetComponent<UIInventoryController>();
        m_Image = GetComponent<Image>();
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener( Select );
    }

    private new void Update()
    {
        base.Update();
        if ( Slot == null ) ResetSprite();
        if ( m_UIInventoryController.SelectedSlot == this && Slot == null ) m_UIInventoryController.SelectedSlot = null;
    }
    private void Select()
    {
        if ( m_UIInventoryController.SelectedSlot != null ) m_UIInventoryController.SelectedSlot.ResetSprite();
        m_UIInventoryController.SelectedSlot = this;
        if ( m_SlotsController.Slots[m_SlotIndex] == null )
        {
            m_UIInventoryController.DisableExtraUI();
            return;
        }
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


}




