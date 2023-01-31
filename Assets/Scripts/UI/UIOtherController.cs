using UnityEngine;

public class UIOtherController : MonoBehaviour
{
    private PlayerAction m_PlayerAction;
    [SerializeField] private UIMiscController m_UIMiscController;
    [SerializeField] private UIFurnitureStoreController m_FurnitureStoreController;
    [SerializeField] private UIFarmStoreController m_SeedStoreController;
    [SerializeField] private UIPauseController m_PauseController;
    [SerializeField]
    private UIHelperController m_HelperController;
    private void Awake()
    {
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
    }
    private void OnEnable()
    {
        m_PlayerAction.ToggleHelperUI += m_HelperController.ToggleUI;
        m_PlayerAction.ToggleMiscUI += m_UIMiscController.ToggleUI;
        m_PlayerAction.ToggleInventoryUI += m_UIMiscController.ToggleInventoryUI;
        m_PlayerAction.ToggleFurnitureStoreUI += m_FurnitureStoreController.ToggleUI;
        m_PlayerAction.ToggleSeedStoreUI += m_SeedStoreController.ToggleUI;
        m_PlayerAction.TogglePause += ToglePauseUI;
    }



    private void ToglePauseUI()
    {
        if (m_PlayerAction.IsOtherUIOpen) return;
        m_PauseController.ToggleUI();
    }

    private void OnDisable()
    {
        m_PlayerAction.ToggleHelperUI -= m_HelperController.ToggleUI;
        m_PlayerAction.ToggleInventoryUI -= m_UIMiscController.ToggleInventoryUI;
        m_PlayerAction.ToggleMiscUI -= m_UIMiscController.ToggleUI;
        m_PlayerAction.ToggleFurnitureStoreUI -= m_FurnitureStoreController.ToggleUI;
        m_PlayerAction.ToggleSeedStoreUI -= m_SeedStoreController.ToggleUI;

    }

}
