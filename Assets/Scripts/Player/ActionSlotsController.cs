using UnityEngine;

public class ActionSlotsController : MonoBehaviour
{
    public ItemSlotData CurrentSelectedSlot { get => m_ItemSlots[m_SelectedSlotIndex]; set => m_ItemSlots[m_SelectedSlotIndex] = value; }
    [SerializeField] private ItemSlotData[] m_ItemSlots = new ItemSlotData[6];
    [SerializeField] private Transform m_Hand;
    [SerializeField] private ItemMainActionChannel m_DeacreasableItemChannel;

    private int m_SelectedSlotIndex = 0;

}
