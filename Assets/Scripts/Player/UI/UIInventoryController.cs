//Must attach to active obj
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventoryController : UIItemSlotsController, IPointerClickHandler
{
    public ItemSlot SelectedItem { get => m_SelectedSlot; set => m_SelectedSlot =  value ; }
    private InventoryController m_InventoryController;
    [SerializeReference] private ItemSlot m_SelectedSlot;
    [SerializeField] private GameObject m_DropUI;

    protected override void Awake()
    {
        base.Awake();
        m_InventoryController = transform.root.GetComponent<InventoryController>();
    }

    private void OnEnable()
    {
        m_InventoryController.OnStoreNewItem += ( _, slot ) => SetUISlotReference( slot );
    }

    private void OnDisable()
    {
        m_InventoryController.OnStoreNewItem -= ( _, slot ) => SetUISlotReference( slot );
        DisableDropUI();
    }

    public void EnableDropUI() => m_DropUI.SetActive( true );
    public void DisableDropUI() => m_DropUI.SetActive( false );
    protected void SetUISlotReference( ItemSlot itemSlot )
    {
        for ( int i = 0; i < m_UIItemSlots.Length; i++ )
        {
            if ( m_UIItemSlots[i].ItemSlot == null )
            {
                m_UIItemSlots[i].ItemSlot = itemSlot;
                m_UIItemSlots[i].UpdateUI();
                break;
            }
        }
    }
    public void OnPointerClick( PointerEventData eventData )
    {
        DisableDropUI();
    }
}
