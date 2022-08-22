using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class PlayerAction : MonoBehaviour
{
    public event UnityAction OnPerformItemMainAction;
    private PlayerInput m_PlayerInput;
    private InputAction m_MainInputAction;
    private InputAction m_StoreInputAction;
    private readonly InputAction[] m_SelectSlotInputAction = new InputAction[6];
    private Camera m_Cam;
    private readonly float m_RaycastDistance = 5f;
    private ActionSlotsController m_ActionSlotsController;
    private InventoryController m_InventoryController;
    private void Awake()
    {
        m_Cam = Camera.main;
        m_PlayerInput = GetComponent<PlayerInput>();
        m_ActionSlotsController = GetComponent<ActionSlotsController>();
        m_InventoryController = GetComponent<InventoryController>();
    }
    private void Start()
    {
        m_ActionSlotsController.SelectActionSlot( 0 );
    }

    private void OnEnable()
    {
        InitializeInputAction();
        m_InventoryController.OnEnableInventoryUI += UnlockCursor;
        m_InventoryController.OnEnableInventoryUI += DisablePlayerInput;
        m_InventoryController.OnDisableInventoryUI += LockCursor;
        m_InventoryController.OnDisableInventoryUI += EnablePlayerInput;
    }

    private void OnDisable()
    {
        UnitializeSelectSlotAction();
        m_MainInputAction.performed -= PerformMainAction;
        m_InventoryController.OnEnableInventoryUI -= UnlockCursor;
        m_InventoryController.OnDisableInventoryUI -= LockCursor;
        m_InventoryController.OnEnableInventoryUI -= DisablePlayerInput;
        m_InventoryController.OnDisableInventoryUI -= EnablePlayerInput;
    }
    private void Update()
    {
        Ray ray = m_Cam.ViewportPointToRay( new Vector3( 0.5f, 0.5f, 0f ) );
        Physics.Raycast( ray, out RaycastHit hitInfo, m_RaycastDistance, Utils.FarmGroundMask | Utils.RaycastableMask | Utils.RestaurantGroundMask );
        if ( hitInfo.collider != null )
        {
            if ( m_ActionSlotsController.CurrEquippedItem != null ) TryPerformSelectedItemRaycastAction( hitInfo );

            if ( m_StoreInputAction.triggered && hitInfo.collider.CompareTag( Utils.PROP_TAG ) )
            {
                Item raycastedItem = hitInfo.collider.GetComponent<Item>();
                if ( m_ActionSlotsController.Store( raycastedItem ) ) return;
                if ( m_InventoryController.Store( raycastedItem ) ) return;

                //TODO::Handle Inventory is full
                Debug.Log( "Inventory is full" );

            }
        }


    }

    private void InitializeInputAction()
    {
        m_MainInputAction = m_PlayerInput.actions[Utils.MAIN_ACTION];
        m_StoreInputAction = m_PlayerInput.actions[Utils.STORE_ACTION];
        m_MainInputAction.performed += PerformMainAction;
        for ( int i = 0; i < 6; i++ )
            m_SelectSlotInputAction[i] = m_PlayerInput.actions[Utils.SELECT_SLOT_ACTION[i]];
        InitializeSelectSlotAction();
    }

    private void PerformMainAction( InputAction.CallbackContext context )
    {
        if ( m_ActionSlotsController.CurrEquippedItem != null )
        {
            m_ActionSlotsController.CurrEquippedItem.MainAction();
            OnPerformItemMainAction?.Invoke();
        }
    }

    private void TryPerformSelectedItemRaycastAction( RaycastHit hitInfo )
    {
        m_ActionSlotsController.CurrEquippedItem.ItemRaycastAction?.PerformRaycastAction( hitInfo );
    }

    private void InitializeSelectSlotAction()
    {
        m_SelectSlotInputAction[0].performed += _ => m_ActionSlotsController.SelectActionSlot( 0 );
        m_SelectSlotInputAction[1].performed += _ => m_ActionSlotsController.SelectActionSlot( 1 );
        m_SelectSlotInputAction[2].performed += _ => m_ActionSlotsController.SelectActionSlot( 2 );
        m_SelectSlotInputAction[3].performed += _ => m_ActionSlotsController.SelectActionSlot( 3 );
        m_SelectSlotInputAction[4].performed += _ => m_ActionSlotsController.SelectActionSlot( 4 );
        m_SelectSlotInputAction[5].performed += _ => m_ActionSlotsController.SelectActionSlot( 5 );
    }

    private void UnitializeSelectSlotAction()
    {
        m_SelectSlotInputAction[0].performed -= _ => m_ActionSlotsController.SelectActionSlot( 0 );
        m_SelectSlotInputAction[1].performed -= _ => m_ActionSlotsController.SelectActionSlot( 1 );
        m_SelectSlotInputAction[2].performed -= _ => m_ActionSlotsController.SelectActionSlot( 2 );
        m_SelectSlotInputAction[3].performed -= _ => m_ActionSlotsController.SelectActionSlot( 3 );
        m_SelectSlotInputAction[4].performed -= _ => m_ActionSlotsController.SelectActionSlot( 4 );
        m_SelectSlotInputAction[5].performed -= _ => m_ActionSlotsController.SelectActionSlot( 5 );
    }

    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void EnablePlayerInput() => m_PlayerInput.enabled = true;
    private void DisablePlayerInput() => m_PlayerInput.enabled = false;
}


