using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;
using Cinemachine;

public class PlayerAction : MonoBehaviour
{
    public event UnityAction OnPerformItemMainAction;
    public UnityAction OnEnableUI { get => m_OnEnableUI; set => m_OnEnableUI = value; }
    public UnityAction OnDisableUI { get => m_OnDisableUI; set => m_OnDisableUI = value; }

    //Event Listener
    private event UnityAction m_OnEnableUI;
    private event UnityAction m_OnDisableUI;

    //Player Input Action
    public InputAction InventoryAction { get => m_InventoryAction; }
    private PlayerInput m_PlayerInput;
    private InputAction m_MainInputAction;
    private InputAction m_StoreInputAction;
    private InputAction m_DropInputAction;
    private InputAction m_InventoryAction;
    private InputAction m_MoveAction;
    private InputAction m_JumpAction;
    private InputAction m_RotationAction;

    private readonly InputAction[] m_SelectSlotInputAction = new InputAction[6];

    private Camera m_Cam;
    private readonly float m_RaycastDistance = 5f;
    private ActionSlotsController m_ActionSlotsController;
    private InventoryController m_InventoryController;

    private void Awake()
    {
        //LockCursor();
        m_Cam = Camera.main;
        m_PlayerInput = GetComponent<PlayerInput>();
        m_ActionSlotsController = GetComponent<ActionSlotsController>();
        m_InventoryController = GetComponent<InventoryController>();
    }
    private void Start()
    {
        m_ActionSlotsController.SelectActionSlot( 0 );
        LockCursor();
    }

    private void OnEnable()
    {
        InitializeInputAction();
        OnEnableUI += UnlockCursor;
        OnEnableUI += DisablePlayerInput;
        OnDisableUI += LockCursor;
        OnDisableUI += EnablePlayerInput;
        m_MainInputAction.performed += PerformMainAction;
        m_DropInputAction.performed += DropItem;
        m_InventoryAction.performed += _ => m_InventoryController.ToggleUI();

    }


    private void OnDisable()
    {
        UnitializeSelectSlotAction();
        OnEnableUI -= UnlockCursor;
        OnDisableUI -= LockCursor;
        OnEnableUI -= DisablePlayerInput;
        OnDisableUI -= EnablePlayerInput;
        m_InventoryAction.performed -= _ => m_InventoryController.ToggleUI();
        m_MainInputAction.performed -= PerformMainAction;
        m_DropInputAction.performed -= DropItem;
    }

    private void DropItem( InputAction.CallbackContext obj )
    {
        m_ActionSlotsController.Drop();
    }
    private void Update()
    {
        Ray ray = m_Cam.ViewportPointToRay( new Vector3( 0.5f, 0.5f, 0f ) );
        Physics.Raycast( ray, out RaycastHit hitInfo, m_RaycastDistance, Utils.FarmGroundMask | Utils.RaycastableMask | Utils.RestaurantGroundMask );
        if ( hitInfo.collider != null )
        {
            if ( m_ActionSlotsController.CurrEquippedItem != null ) TryPerformSelectedItemRaycastAction( hitInfo );

            if ( m_StoreInputAction.triggered && hitInfo.collider.TryGetComponent( out Item raycastedItem ) )
            {
                if ( Store( raycastedItem ) ) return;

                //TODO::Handle Inventory is full
                Debug.Log( "Inventory is full" );

            }
        }


    }

    public bool Store( Item item )
    {
        if ( m_InventoryController.Store( item ) ) return true;
        if ( m_ActionSlotsController.Store( item ) ) return true;
        return false;
    }

    private void InitializeInputAction()
    {
        m_MainInputAction = m_PlayerInput.actions[Utils.MAIN_ACTION];
        m_StoreInputAction = m_PlayerInput.actions[Utils.STORE_ACTION];
        m_DropInputAction = m_PlayerInput.actions[Utils.DROP_ACTION];
        m_InventoryAction = m_PlayerInput.actions[Utils.INVENTORY_ACTION];
        m_MoveAction = m_PlayerInput.actions[Utils.MOVE_ACTION];
        m_JumpAction = m_PlayerInput.actions[Utils.JUMP_ACTION];
        m_RotationAction = m_PlayerInput.actions[Utils.PLAYER_ROTATION_ACTION];
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
    private void EnablePlayerInput()
    {
        m_MoveAction.Enable();
        m_JumpAction.Enable();
        m_RotationAction.Enable();
        EnableSelectSlotInputAction();
        m_MainInputAction.Enable();
        m_DropInputAction.Enable();
        m_StoreInputAction.Enable();
    }
    private void DisablePlayerInput()
    {
        m_MoveAction.Disable();
        m_JumpAction.Disable();
        m_RotationAction.Disable();
        DisableSelectSlotInputAction();
        m_MainInputAction.Disable();
        m_DropInputAction.Disable();
        m_StoreInputAction.Enable();
    }

    private void EnableSelectSlotInputAction()
    {
        m_SelectSlotInputAction[0].Enable();
        m_SelectSlotInputAction[1].Enable();
        m_SelectSlotInputAction[2].Enable();
        m_SelectSlotInputAction[3].Enable();
        m_SelectSlotInputAction[4].Enable();
        m_SelectSlotInputAction[5].Enable();
    }



    private void DisableSelectSlotInputAction()
    {
        m_SelectSlotInputAction[0].Disable();
        m_SelectSlotInputAction[1].Disable();
        m_SelectSlotInputAction[2].Disable();
        m_SelectSlotInputAction[3].Disable();
        m_SelectSlotInputAction[4].Disable();
        m_SelectSlotInputAction[5].Disable();
    }
}


