using UnityEngine;
using UnityEngine.InputSystem;

public class UIGameActionSlotsController : MonoBehaviour
{
    [SerializeField] private Sprite m_SelectedSprite, m_UnselectedSprite;
    private UIActionSlot[] m_ActionSlots;
    private ActionSlotsController m_ActionSlotsController;
    private void Awake()
    {
        m_ActionSlots = GetComponentsInChildren<UIActionSlot>();
        m_ActionSlotsController = transform.root.GetComponent<ActionSlotsController>();
    }
    private void OnEnable()
    {
        m_ActionSlotsController.OnSelectSlot += SelectSlot;
    }

    private void OnDisable()
    {
        m_ActionSlotsController.OnSelectSlot -= SelectSlot;
    }

    private void SelectSlot( int index )
    {
        ResetAllSlot();
        m_ActionSlots[index].ChangeSprite( m_SelectedSprite );
    }
    private void ResetAllSlot()
    {
        foreach ( var slot in m_ActionSlots )
        {
            slot.ChangeSprite( m_UnselectedSprite );
        }
    }
}
