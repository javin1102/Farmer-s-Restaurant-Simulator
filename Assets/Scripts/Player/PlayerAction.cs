using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent( typeof( ItemSlotsController ) )]
public class PlayerAction : MonoBehaviour
{
    private ItemSlotsController m_ItemController;
    private PlayerInput m_PlayerInput;
    private InputAction m_MainInputAction;
    private InputAction m_StoreInputAction;
    private InputAction m_SelectSlotInputAction;

    private Camera m_Cam;
    private readonly float m_RaycastDistance = 5f;

    private void Awake()
    {
        m_Cam = Camera.main;

        m_ItemController = GetComponent<ItemSlotsController>();
        m_PlayerInput = GetComponent<PlayerInput>();

        m_MainInputAction = m_PlayerInput.actions[Utils.MAIN_ACTION];
        m_StoreInputAction = m_PlayerInput.actions[Utils.STORE_ACTION];
        m_SelectSlotInputAction = m_PlayerInput.actions[Utils.ITEM_SELECTION_ACTION];
    }
    private void Start()
    {
        m_ItemController.InstantiateAndAssignItemRefToSlot();
        m_ItemController.SelectSlot( 0 );
    }
    private void OnEnable()
    {
        m_SelectSlotInputAction.performed += m_ItemController.ItemSlotSelection;
    }

    private void OnDisable()
    {
        m_SelectSlotInputAction.performed -= m_ItemController.ItemSlotSelection;
    }
    private void Update()
    {
        Ray ray = m_Cam.ViewportPointToRay( new Vector3( 0.5f, 0.5f, 0f ) );
        Physics.Raycast( ray, out RaycastHit hitInfo, m_RaycastDistance, Utils.FarmGroundMask | Utils.RaycastableMask | Utils.RestaurantGroundMask );
        if ( hitInfo.collider != null )
        {
            if ( m_ItemController.CurrentSelectedSlot.ItemRef != null ) TryPerformSelectedItemRaycastAction( hitInfo );

            if ( m_StoreInputAction.triggered && hitInfo.collider.CompareTag( Utils.PROP_TAG ) ) m_ItemController.Store( hitInfo.collider.GetComponent<Item>() );
        }


        if ( m_MainInputAction.triggered && m_ItemController.CurrentSelectedSlot.ItemRef != null )
        {
            m_ItemController.CurrentSelectedSlot.ItemRef.MainAction();
        }
    }

    private void TryPerformSelectedItemRaycastAction( RaycastHit hitInfo )
    {
        m_ItemController.CurrentSelectedSlot.ItemRef.ItemRaycastAction?.PerformRaycastAction( hitInfo );
    }



}


