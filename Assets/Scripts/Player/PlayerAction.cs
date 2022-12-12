using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using SimpleJSON;
using System;

public class PlayerAction : MonoBehaviour
{
    public event UnityAction OnPerformItemMainAction;
    public UnityAction OnEnableOtherUI { get => m_OnEnableUI; set => m_OnEnableUI = value; }
    public UnityAction OnDisableOtherUI { get => m_OnDisableUI; set => m_OnDisableUI = value; }
    public UnityAction ToggleInventoryUI { get => m_ToggleInventoryUI; set => m_ToggleInventoryUI = value; }
    public UnityAction ToggleMiscUI { get => m_ToggleMiscUI; set => m_ToggleMiscUI = value; }
    public UnityAction<Transform> ToggleFurnitureStoreUI { get => m_ToggleFurnitureStoreUI; set => m_ToggleFurnitureStoreUI = value; }
    public UnityAction<Transform> ToggleSeedStoreUI { get => m_ToggleSeedStoreUI; set => m_ToggleSeedStoreUI = value; }
    public UnityAction OnDecreaseItemDatabase { get => m_OnDropInventory; set => m_OnDropInventory = value; }
    public bool IsOtherUIOpen { get => m_IsOtherUIOpen; set => m_IsOtherUIOpen = value; }
    public InputAction InventoryAction { get => m_InventoryAction; }
    public UnityAction TogglePause { get => m_TogglePause; set => m_TogglePause = value; }
    public float ActionTime { get => m_ActionTimeRemaining; set => m_ActionTimeRemaining = value; }
    public float DefaultActionTime { get => m_DefaultActionTime; set => m_DefaultActionTime = value; }
    public Item CurrEquippedItem { get => m_ActionSlotsController.CurrEquippedItem; }
    public static int Coins { get => m_Coins; set => m_Coins = value; }
    public static PlayerAction Instance { get => m_Instance; }
    public PlayerUpgrades PlayerUpgrades { get => m_PlayerUpgrades; }
    public UnityAction OnLoadSucceeded { get => m_OnLoadSucceeded; set => m_OnLoadSucceeded = value; } //Make sure subscribed script is on the same scene

    //Event Listener
    private event UnityAction m_OnEnableUI;
    private event UnityAction m_OnDisableUI;
    private UnityAction m_ToggleInventoryUI;
    private UnityAction m_ToggleMiscUI;
    private UnityAction<Transform> m_ToggleFurnitureStoreUI;
    private UnityAction<Transform> m_ToggleSeedStoreUI;
    private UnityAction m_OnDropInventory;
    private UnityAction m_TogglePause;
    private UnityAction m_OnLoadSucceeded;


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
    private InputAction m_OpenMiscUIAction;
    private InputAction m_PauseAction;
    private readonly InputAction[] m_SelectSlotInputAction = new InputAction[6];


    private Camera m_Cam;
    private readonly float m_RaycastDistance = 5f;
    private ActionSlotsController m_ActionSlotsController;
    private InventorySlotsController m_InventorySlotsController;
    private ItemDatabase m_ItemDatabase;
    private bool m_IsOtherUIOpen; //check if "other" ui is open
    private Hoverable m_Hovered;
    private UIManager m_UIManager;
    private float m_ActionTimeRemaining; //time passed for certain action to be done (ex: chopping tree)
    private float m_DefaultActionTime; //time needed for certain action to be done (ex: chopping tree)
    private static int m_Coins;
    private SaveManager m_SaveManager;
    private PlayerUpgrades m_PlayerUpgrades;
    private AudioSource m_AudioSource;
    private static PlayerAction m_Instance;
    private ResourcesLoader m_ResourcesLoader;

    private void Awake()
    {
        //LockCursor();
        if (m_Instance == null) m_Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        m_Cam = Camera.main;
        m_PlayerInput = GetComponent<PlayerInput>();
        m_ActionSlotsController = GetComponent<ActionSlotsController>();
        m_ItemDatabase = GetComponent<ItemDatabase>();
        m_InventorySlotsController = GetComponent<InventorySlotsController>();
        m_PlayerUpgrades = GetComponent<PlayerUpgrades>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        m_UIManager = UIManager.Instance;
        m_SaveManager = SaveManager.Instance;
        m_ResourcesLoader = ResourcesLoader.Instance;
        m_SaveManager.OnSave += SavePlayerData;
        m_SaveManager.LoadData(Utils.PLAYERDATA_FILENAME, LoadSucceeded, LoadFailed);
        m_ActionSlotsController.SelectActionSlot(0);
        LockCursor();
    }

    private void OnEnable()
    {
        InitializeInputAction();
        OnEnableOtherUI += EnterCursorMode;
        OnDisableOtherUI += ExitCursorMode;
        m_MainInputAction.performed += PerformEquippedItemMainAction;
        m_DropInputAction.performed += DropItem;
        m_InventoryAction.performed += InvokeToggleInventoryUI;
        m_OpenMiscUIAction.performed += InvokeToggleMiscUI;
        m_AltAction.started += EnterCursorMode;
        m_AltAction.canceled += ExitCursorMode;
        m_OnDropInventory += m_InventorySlotsController.CheckItem;
        m_PauseAction.performed += InvokeTogglePauseUI;
    }


    private void OnDisable()
    {
        UnitializeSelectSlotAction();
        m_OnDropInventory -= m_InventorySlotsController.CheckItem;
        OnEnableOtherUI -= EnterCursorMode;
        OnDisableOtherUI -= ExitCursorMode;
        m_InventoryAction.performed -= InvokeToggleInventoryUI;
        m_OpenMiscUIAction.performed -= InvokeToggleMiscUI;
        m_MainInputAction.performed -= PerformEquippedItemMainAction;
        m_DropInputAction.performed -= DropItem;
        m_AltAction.started -= EnterCursorMode;
        m_AltAction.canceled -= ExitCursorMode;
        m_PauseAction.performed -= InvokeTogglePauseUI;
    }


    private void Update()
    {
        Ray ray = m_Cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Physics.Raycast(ray, out RaycastHit hitInfo, m_RaycastDistance, ~Utils.PlayerMask);
        if (hitInfo.collider != null)
        {
            if (m_ActionSlotsController.CurrEquippedItem != null)
            {
                TryPerformSelectedItemRaycastAction(hitInfo);
            }

            if (hitInfo.collider.TryGetComponent(out IActionTime actionTime))
            {
                if (m_MainInputAction.IsPressed()) actionTime.OnHoldMainAction(this);
                else actionTime.OnReleaseMainAction(this);
            }
            else
            {
                m_ActionTimeRemaining = 0;
                m_DefaultActionTime = 0;
            }
            if (hitInfo.collider.TryGetComponent(out Hoverable hover))
            {
                //if ( m_Hovered == hover ) return;
                if (m_Hovered != null && m_Hovered != hover)
                {
                    m_Hovered.HoverExit();
                    m_ActionTimeRemaining = 0;
                    m_DefaultActionTime = 0;
                }
                m_Hovered = hover;
                m_Hovered.HoverEnter();
            }
            else
            {
                if (m_Hovered != null)
                {
                    m_Hovered.HoverExit();
                    m_ActionTimeRemaining = 0;
                    m_DefaultActionTime = 0;
                    m_Hovered = null;
                }
            }
            if (m_MainInputAction.triggered && hitInfo.collider.TryGetComponent(out IInteractable hit)) hit.Interact(this);
            if (m_StoreInputAction.triggered && hitInfo.collider.TryGetComponent(out Item raycastedItem))
            {
                if (Store(raycastedItem))
                {

                }
                else
                {
                    //TODO::Handle Inventory is full
                    Debug.Log("Inventory is full");
                }
            }
        }
        else
        {
            m_UIManager.HideActionHelper();
            m_ActionTimeRemaining = 0;
            m_DefaultActionTime = 0;
            if (m_Hovered == null) return;
            m_Hovered.HoverExit();
            m_Hovered = null;

        }

        if (ActionTime <= 0) ActionTime = 0;
    }


    private async void SavePlayerData()
    {
        JSONObject rootObject = new();
        (JSONArray inventory, JSONArray actionSlots) arrs = m_ItemDatabase.ToJSON();
        rootObject.Add("inventory", arrs.inventory);
        rootObject.Add("actionSlots", arrs.actionSlots);
        rootObject.Add("coins", m_Coins);
        rootObject.Add("upgrades", new SerializablePlayerUpgradesData(m_PlayerUpgrades).Serialize());
        await m_SaveManager.SaveData(rootObject.ToString(), Utils.PLAYERDATA_FILENAME);
    }

    private void LoadSucceeded(JSONNode jsonNode)
    {
        m_ItemDatabase.OnLoadSucceded(jsonNode);
        m_Coins = jsonNode["coins"];
        SerializablePlayerUpgradesData upgradesData = new(jsonNode["upgrades"]);
        m_PlayerUpgrades.Set(upgradesData.chefQuantityLevel, upgradesData.restaurantExpandLevel, upgradesData.inventoryLevel);
        // OnLoadSucceeded?.Invoke();
    }

    private void LoadFailed()
    {
        m_ItemDatabase.OnLoadFailed();
        m_Coins = 100;
        m_PlayerUpgrades.Reset();
    }



    public bool Store(Item item, int quantity = 1)
    {
        if (m_ItemDatabase.Store(item.Data, quantity))
        {
            Destroy(item.gameObject);
            return true;
        }
        return false;
    }
    public void InvokeToggleInventoryUI() => m_ToggleInventoryUI?.Invoke();
    public void InvokeToggleMiscUI() => m_ToggleMiscUI?.Invoke();
    private void DropItem(InputAction.CallbackContext obj)
    {
        if (m_ActionSlotsController.CurrEquippedItem == null) return;
        m_ItemDatabase.Decrease(m_ActionSlotsController.CurrEquippedItem.Data, 1, ItemDatabaseAction.DROP);
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
        m_OpenMiscUIAction = m_PlayerInput.actions[Utils.OPENUI_ACTION];
        m_PauseAction = m_PlayerInput.actions[Utils.PAUSE_ACTION];

        for (int i = 0; i < 6; i++)
            m_SelectSlotInputAction[i] = m_PlayerInput.actions[Utils.SELECT_SLOT_ACTION[i]];
        InitializeSelectSlotAction();
    }
    private void PerformEquippedItemMainAction(InputAction.CallbackContext context)
    {
        if (m_ActionSlotsController.CurrEquippedItem != null)
        {
            m_ActionSlotsController.CurrEquippedItem.MainAction();
            OnPerformItemMainAction?.Invoke();
        }
    }
    private void TryPerformSelectedItemRaycastAction(RaycastHit hitInfo)
    {
        m_ActionSlotsController.CurrEquippedItem.ItemRaycastAction?.PerformRaycastAction(hitInfo);
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
    public void EnterCursorMode()
    {
        UnlockCursor();
        DisablePlayerInput();
    }
    public void ExitCursorMode()
    {
        if (m_IsOtherUIOpen) return;
        LockCursor();
        EnablePlayerInput();
    }

    public void PlayAudio(string audioClipName)
    {
        m_AudioSource.clip = m_ResourcesLoader.AudioClips[audioClipName];
        m_AudioSource.Play();
    }
    private void InvokeToggleInventoryUI(InputAction.CallbackContext obj) => m_ToggleInventoryUI?.Invoke();
    private void InvokeToggleMiscUI(InputAction.CallbackContext obj) => m_ToggleMiscUI?.Invoke();
    private void InvokeTogglePauseUI(InputAction.CallbackContext obj) => m_TogglePause?.Invoke();

    private void ExitCursorMode(InputAction.CallbackContext obj) => ExitCursorMode();
    private void EnterCursorMode(InputAction.CallbackContext obj) => EnterCursorMode();
    private void SelectActionSlot_1(InputAction.CallbackContext obj) => m_ActionSlotsController.SelectActionSlot_1();
    private void SelectActionSlot_2(InputAction.CallbackContext obj) => m_ActionSlotsController.SelectActionSlot_2();
    private void SelectActionSlot_3(InputAction.CallbackContext obj) => m_ActionSlotsController.SelectActionSlot_3();
    private void SelectActionSlot_4(InputAction.CallbackContext obj) => m_ActionSlotsController.SelectActionSlot_4();
    private void SelectActionSlot_5(InputAction.CallbackContext obj) => m_ActionSlotsController.SelectActionSlot_5();
    private void SelectActionSlot_6(InputAction.CallbackContext obj) => m_ActionSlotsController.SelectActionSlot_6();
}

