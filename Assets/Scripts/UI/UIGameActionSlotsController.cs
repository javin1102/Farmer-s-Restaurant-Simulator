using UnityEngine;
using UnityEngine.InputSystem;

public class UIGameActionSlotsController : MonoBehaviour
{
    [SerializeField] private Sprite m_SelectedSprite, m_UnselectedSprite;
    private UIActionSlot[] m_ActionSlots;
    private PlayerAction m_PlayerAction;
    private void Awake()
    {
        m_ActionSlots = GetComponentsInChildren<UIActionSlot>();
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
    }
    private void OnEnable()
    {
        m_PlayerAction.SelectSlotInputAction[0].performed += SelectSlot_0;
        m_PlayerAction.SelectSlotInputAction[1].performed += SelectSlot_1;
        m_PlayerAction.SelectSlotInputAction[2].performed += SelectSlot_2;
        m_PlayerAction.SelectSlotInputAction[3].performed += SelectSlot_3;
        m_PlayerAction.SelectSlotInputAction[4].performed += SelectSlot_4;
        m_PlayerAction.SelectSlotInputAction[5].performed += SelectSlot_5;
    }

    private void OnDisable()
    {
        m_PlayerAction.SelectSlotInputAction[0].performed -= SelectSlot_0;
        m_PlayerAction.SelectSlotInputAction[1].performed -= SelectSlot_1;
        m_PlayerAction.SelectSlotInputAction[2].performed -= SelectSlot_2;
        m_PlayerAction.SelectSlotInputAction[3].performed -= SelectSlot_3;
        m_PlayerAction.SelectSlotInputAction[4].performed -= SelectSlot_4;
        m_PlayerAction.SelectSlotInputAction[5].performed -= SelectSlot_5;
    }

    private void SelectSlot_0( InputAction.CallbackContext ctx ) => SelectSlot( 0 );
    private void SelectSlot_1( InputAction.CallbackContext ctx ) => SelectSlot( 1 );
    private void SelectSlot_2( InputAction.CallbackContext ctx ) => SelectSlot( 2 );
    private void SelectSlot_3( InputAction.CallbackContext ctx ) => SelectSlot( 3 );
    private void SelectSlot_4( InputAction.CallbackContext ctx ) => SelectSlot( 4 );
    private void SelectSlot_5( InputAction.CallbackContext ctx ) => SelectSlot( 5 );
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
