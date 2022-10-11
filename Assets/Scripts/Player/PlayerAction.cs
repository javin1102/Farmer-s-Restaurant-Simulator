using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

public class PlayerAction : MonoBehaviour
{
    public event UnityAction OnPerformItemMainAction;
    public UnityAction OnEnableUI { get => m_OnEnableUI; set => m_OnEnableUI = value; }
    public UnityAction OnDisableUI { get => m_OnDisableUI; set => m_OnDisableUI = value; }
    public UnityAction ToggleInventoryUI { get => m_ToggleInventoryUI; set => m_ToggleInventoryUI = value; }
    public UnityAction ToggleUI { get => m_ToggleUI; set => m_ToggleUI = value; }
    public bool IsUIOpen { get => m_IsUIOpen; set => m_IsUIOpen = value; }

    //Event Listener
    private event UnityAction m_OnEnableUI;
    private event UnityAction m_OnDisableUI;
    private UnityAction m_ToggleInventoryUI;
    private UnityAction m_ToggleUI;

    //Player Input Action
    private PlayerInput m_PlayerInput;
    private InputAction m_MainInputAction;
    private InputAction m_StoreInputAction;
    private InputAction m_DropInputAction;
    private InputAction m_InventoryAction;
    private InputAction m_MoveAction;
    private InputAction m_JumpAction;
    private InputAction m_RotationAction;
    private InputAction m_AltAction;
    private InputAction m_OpenUIAction;
    private readonly InputAction[] m_SelectSlotInputAction = new InputAction[6];


    private Camera m_Cam;
    private readonly float m_RaycastDistance = 5f;
    private ActionSlotsController m_ActionSlotsController;
    private ItemDatabase m_ItemDatabase;
    private bool m_IsUIOpen;
    private Hoverable m_Hovered;
    private void Awake()
    {
        //LockCursor();
        m_Cam = Camera.main;
        m_PlayerInput = GetComponent<PlayerInput>();
        m_ActionSlotsController = GetComponent<ActionSlotsController>();
        m_ItemDatabase = GetComponent<ItemDatabase>();
    }
    private void Start()
    {
        m_ActionSlotsController.SelectActionSlot( 0 );
        LockCursor();
    }

    private void OnEnable()
    {
        InitializeInputAction();
        OnEnableUI += EnterCursorMode;
        OnDisableUI += ExitCursorMode;
        m_MainInputAction.performed += PerformEquippedItemMainAction;
        m_DropInputAction.performed += DropItem;
        m_InventoryAction.performed += InvokeToggleInventoryUI;
        m_OpenUIAction.performed += InvokeToggleUI;
        m_AltAction.started += EnterCursorMode;
        m_AltAction.canceled += ExitCursorMode;
    }

    private void OnDisable()
    {
        UnitializeSelectSlotAction();
        OnEnableUI -= EnterCursorMode;
        OnDisableUI -= ExitCursorMode;
        m_InventoryAction.performed -= InvokeToggleInventoryUI;
        m_OpenUIAction.performed -= InvokeToggleUI;
        m_MainInputAction.performed -= PerformEquippedItemMainAction;
        m_DropInputAction.performed -= DropItem;
        m_AltAction.started -= EnterCursorMode;
        m_AltAction.canceled -= ExitCursorMode;
    }


    private void Update()
    {
        Ray ray = m_Cam.ViewportPointToRay( new Vector3( 0.5f, 0.5f, 0f ) );
        Physics.Raycast( ray, out RaycastHit hitInfo, m_RaycastDistance, ~Utils.PlayerMask );
        if ( hitInfo.collider != null )
        {
            if ( m_ActionSlotsController.CurrEquippedItem != null ) TryPerformSelectedItemRaycastAction( hitInfo );
            if ( hitInfo.collider.TryGetComponent( out Hoverable hover ) )
            {
                //if ( m_Hovered == hover ) return;
                if ( m_Hovered != null && m_Hovered != hover ) m_Hovered.HoverExit();
                m_Hovered = hover;
                m_Hovered.HoverEnter();
            }
            else
            {
                if ( m_Hovered != null )
                {
                    m_Hovered.HoverExit();
                    m_Hovered = null;
                }
            }
            if ( m_MainInputAction.triggered && hitInfo.collider.TryGetComponent( out IInteractable hit ) ) hit.Interact();
            if ( m_StoreInputAction.triggered && hitInfo.collider.TryGetComponent( out Item raycastedItem ) )
            {
                Debug.Log( "Store" );
                if ( Store( raycastedItem ) ) return;

                //TODO::Handle Inventory is full
                Debug.Log( "Inventory is full" );

            }
        }
        else
        {
            if ( m_Hovered == null ) return;
            m_Hovered.HoverExit();
            m_Hovered = null;
        }

    }

    public bool Store( Item item )
    {
        if ( m_ItemDatabase.Store( item.Data ) )
        {
            Destroy( item.gameObject );
            return true;
        }
        return false;
    }
    public void InvokeToggleInventoryUI() => m_ToggleInventoryUI?.Invoke();
    public void InvokeToggleUI() => m_ToggleUI?.Invoke();
    private void DropItem( InputAction.CallbackContext obj ) {
        if ( m_ActionSlotsController.CurrEquippedItem == null ) return;
        m_ItemDatabase.Drop( m_ActionSlotsController.CurrEquippedItem.Data, 1 );
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
        m_AltAction = m_PlayerInput.actions[Utils.ALT_ACTION];
        m_OpenUIAction = m_PlayerInput.actions[Utils.OPENUI_ACTION];

        for ( int i = 0; i < 6; i++ )
            m_SelectSlotInputAction[i] = m_PlayerInput.actions[Utils.SELECT_SLOT_ACTION[i]];
        InitializeSelectSlotAction();
    }
    private void PerformEquippedItemMainAction( InputAction.CallbackContext context )
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
        m_SelectSlotInputAction[0].performed += SelectActionSlot_1;
        m_SelectSlotInputAction[1].performed += SelectActionSlot_2;
        m_SelectSlotInputAction[2].performed += SelectActionSlot_3;
        m_SelectSlotInputAction[3].performed += SelectActionSlot_4;
        m_SelectSlotInputAction[4].performed += SelectActionSlot_5;
        m_SelectSlotInputAction[5].performed += SelectActionSlot_6;
    }
    private void UnitializeSelectSlotAction()
    {
        m_SelectSlotInputAction[0].performed -= SelectActionSlot_1;
        m_SelectSlotInputAction[1].performed -= SelectActionSlot_2;
        m_SelectSlotInputAction[2].performed -= SelectActionSlot_3;
        m_SelectSlotInputAction[3].performed -= SelectActionSlot_4;
        m_SelectSlotInputAction[4].performed -= SelectActionSlot_5;
        m_SelectSlotInputAction[5].performed -= SelectActionSlot_6;
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
    private void EnterCursorMode()
    {
        UnlockCursor();
        DisablePlayerInput();
    }
    private void ExitCursorMode()
    {
        if ( m_IsUIOpen ) return;
        LockCursor();
        EnablePlayerInput();
    }
    private void InvokeToggleInventoryUI( InputAction.CallbackContext obj ) => m_ToggleInventoryUI?.Invoke();
    private void InvokeToggleUI( InputAction.CallbackContext obj ) => m_ToggleUI?.Invoke();

    private void ExitCursorMode( InputAction.CallbackContext obj ) => ExitCursorMode();
    private void EnterCursorMode( InputAction.CallbackContext obj ) => EnterCursorMode();
    private void SelectActionSlot_1( InputAction.CallbackContext obj ) => m_ActionSlotsController.SelectActionSlot_1();
    private void SelectActionSlot_2( InputAction.CallbackContext obj ) => m_ActionSlotsController.SelectActionSlot_2();
    private void SelectActionSlot_3( InputAction.CallbackContext obj ) => m_ActionSlotsController.SelectActionSlot_3();
    private void SelectActionSlot_4( InputAction.CallbackContext obj ) => m_ActionSlotsController.SelectActionSlot_4();
    private void SelectActionSlot_5( InputAction.CallbackContext obj ) => m_ActionSlotsController.SelectActionSlot_5();
    private void SelectActionSlot_6( InputAction.CallbackContext obj ) => m_ActionSlotsController.SelectActionSlot_6();
}


