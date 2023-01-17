using UnityEngine;
namespace NPC.Pesticide
{
    [RequireComponent(typeof(PesticideSystem))]
    public class NPCPesticide : NPCManager, IInteractable
    {
        private PesticideSystem m_PesticideSystem;
        private UIManager m_UIManager;
        private Hoverable m_Hoverable;
        private readonly int m_PesticideCost = 20;
        void Start()
        {
            m_PesticideSystem = GetComponent<PesticideSystem>();
            m_Hoverable = GetComponent<Hoverable>();
            m_UIManager = UIManager.Instance;
            m_Hoverable.OnHoverEnter += ShowPrimaryHelper;
            m_Hoverable.OnHoverExit += HideHelper;
        }

        private void Update()
        {
            m_Hoverable.IsHoverable = !m_PesticideSystem.isAlreadyPesticide;
        }

        private void OnDestroy()
        {
            m_Hoverable.OnHoverEnter -= ShowPrimaryHelper;
            m_Hoverable.OnHoverExit -= HideHelper;
        }

        private void ShowPrimaryHelper()
        {
            if (m_PesticideSystem.isAlreadyPesticide) return;
            m_UIManager.ShowActionHelperPrimary("Left", $"Berikan pestisida : <sprite=0><color=yellow>{m_PesticideCost}</color>");
        }

        private void HideHelper() => m_UIManager.HideActionHelper();

        public void Interact(PlayerAction playerAction)
        {
            if (!m_PesticideSystem.TriggerPesticide())
            {
                print("Pestisida telah diberikan");
                return;
            }

            playerAction.Coins -= m_PesticideCost;
        }
    }
}

