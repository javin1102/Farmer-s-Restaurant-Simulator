using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventoryController : MonoBehaviour, IPointerClickHandler
{
    public UIInventorySlot SelectedSlot { get => m_SelectedSlot; set => m_SelectedSlot = value; }
    public Sprite SelectedSprite { get => m_SelectedSprite; }
    public Sprite UnselectedSprite { get => m_UnselectedSprite; }

    [SerializeReference] private UIInventorySlot m_SelectedSlot;
    [SerializeField] private UIInventoryTooltip m_Tooltip;
    [SerializeField] private GameObject m_ExtraUI;
    [SerializeField] private Sprite m_SelectedSprite, m_UnselectedSprite;
    private PlayerUpgrades m_PlayerUpgrades;
    private Transform m_ContentTf;

    void Start()
    {
        InitalizeInventorySlots();
    }

    private void InitalizeInventorySlots()
    {
        m_PlayerUpgrades = transform.root.GetComponent<PlayerUpgrades>();
        int slotSize = m_PlayerUpgrades.GetSlotSize();
        m_ContentTf = transform.GetChild(0);
        for (int i = 0; i < slotSize; i++) m_ContentTf.GetChild(i).gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (m_SelectedSlot != null)
            m_SelectedSlot.ResetSprite();
        m_SelectedSlot = null;
        DisableExtraUI();
    }

    public void EnableExtraUI() => m_ExtraUI.SetActive(true);
    public void DisableExtraUI() => m_ExtraUI.SetActive(false);
    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_SelectedSlot != null)
            m_SelectedSlot.ResetSprite();
        DisableExtraUI();
        m_SelectedSlot = null;
    }

    public void ShowTooltip(ItemData itemData, Vector3 pos)
    {
        m_Tooltip.gameObject.SetActive(true);
        m_Tooltip.SetPos(pos);
        m_Tooltip.UpdateUI(itemData.ID);
    }

    public void DisableTooltip() => m_Tooltip.gameObject.SetActive(false);

}
