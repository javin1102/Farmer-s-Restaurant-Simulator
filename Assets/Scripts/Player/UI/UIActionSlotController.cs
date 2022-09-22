//NOTE:Must attach to parent obj
public class UIActionSlotController : UIItemSlotsController
{
    private ActionSlotsController m_ActionSlotsController;
    protected override void Awake()
    {
        base.Awake();
        m_ActionSlotsController = transform.root.GetComponent<ActionSlotsController>();
    }


    protected void SetUISlotReference( int slotIndex, ItemSlot itemSlot )
    {
        m_UIItemSlots[slotIndex].ItemSlot = itemSlot;
        m_UIItemSlots[slotIndex].UpdateUI();
    }

    private void OnEnable()
    {
        for ( int i = 0; i < m_ActionSlotsController.ActionSlots.Length; i++ )
        {
            m_UIItemSlots[i].ItemSlot = m_ActionSlotsController.ActionSlots[i];
            m_UIItemSlots[i].UpdateUI();
        }

        m_ActionSlotsController.OnStoreNewItem += SetUISlotReference;
        m_ActionSlotsController.OnUIDropItem += UpdateActionSlots;
    }

    private void OnDisable()
    {
        m_ActionSlotsController.OnStoreNewItem -= SetUISlotReference;
        m_ActionSlotsController.OnUIDropItem -= UpdateActionSlots;
    }

    private void UpdateActionSlots()
    {
        for ( int i = 0; i < m_ActionSlotsController.ActionSlots.Length; i++ )
        {
            m_ActionSlotsController.ActionSlots[i] = m_UIItemSlots[i].ItemSlot;
        }
    }


}
