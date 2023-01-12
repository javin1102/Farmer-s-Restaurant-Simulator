using UnityEngine;
using UnityEngine.Events;
namespace NPC.Funfact
{
    [RequireComponent(typeof(Hoverable))]
    public class NPCFunfact : NPCManager, IInteractable
    {
        public UnityAction UpdateFunfact { get; set; }
        private Hoverable m_Hoverable;
        private UIManager m_UIManager;
        void Start()
        {
            m_Hoverable = GetComponent<Hoverable>();
            m_UIManager = UIManager.Instance;
            m_Hoverable.OnHoverEnter += ShowPrimaryHelper;
            m_Hoverable.OnHoverExit += HideHelper;
            UpdateFunfact?.Invoke();
        }
        private void OnDestroy()
        {
            m_Hoverable.OnHoverEnter -= ShowPrimaryHelper;
            m_Hoverable.OnHoverExit -= HideHelper;
        }

        private void ShowPrimaryHelper()
        {
            m_UIManager.ShowActionHelperPrimary("Left", "Fakta Selanjutnya");
        }

        private void HideHelper() => m_UIManager.HideActionHelper();

        public void Interact(PlayerAction m_PlayerAction)
        {
            UpdateFunfact?.Invoke();
        }
    }
}

