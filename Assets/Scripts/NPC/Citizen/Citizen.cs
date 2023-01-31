using UnityEngine;
using System.Collections;
namespace NPC.Citizen
{
    [RequireComponent(typeof(Hoverable))]
    public class Citizen : NPCManager, IInteractable
    {

        public Food ServedFood { get => m_ServedFood; set => m_ServedFood = value; }
        public bool TravelBackwards { get => m_TravelBackwards; set => m_TravelBackwards = value; }
        public Waypoint CurrentWaypoint { get => m_CurrentWaypoint; set => m_CurrentWaypoint = value; }

        public float m_CoinDelay = 15f;

        [SerializeField] private Waypoint m_CurrentWaypoint;
        private Food m_ServedFood;
        private UIManager m_UIManager;
        private Hoverable m_Hoverable;
        [SerializeField] private bool m_TravelBackwards;
        private new void Awake()
        {
            base.Awake();
            m_UIManager = UIManager.Instance;
            m_Hoverable = GetComponent<Hoverable>();

        }
        private void ShowHelper()
        {
            m_UIManager.ShowActionHelperPrimary("Left", "Ambil tip");
        }



        private void OnEnable()
        {
            m_Hoverable.IsHoverable = false;
            ChangeState(new IdleState());
            m_Hoverable.OnHoverEnter += ShowHelper;
            m_Hoverable.OnHoverExit += HideHelper;
        }

        private void HideHelper()
        {
            m_UIManager.HideActionHelper();
        }

        private void OnDisable()
        {
            m_Hoverable.OnHoverEnter -= ShowHelper;
            m_Hoverable.OnHoverExit -= HideHelper;
        }

        private void Update()
        {
            m_CurrentState.OnUpdateState(this);
        }

        public Waypoint DetermineNextWaypoint()
            => m_TravelBackwards == true ? m_CurrentWaypoint.previousWaypoint : m_CurrentWaypoint.nextWayPoint;


        IEnumerator CoinTipDrop()
        {
            int rand = Random.Range(1, 5);
            if (rand < 3)
            {
                // prefab CoinTipCanvas
                m_Hoverable.IsHoverable = true;
                this.transform.GetChild(2).gameObject.SetActive(true);
                yield return new WaitForSeconds(m_CoinDelay);
                this.transform.GetChild(2).gameObject.SetActive(false);
                m_Hoverable.IsHoverable = false;
            }

            else
            {
                m_Hoverable.IsHoverable = false;
                yield return null;
            }
        }

        public void Interact(PlayerAction m_PlayerAction)
        {
            if (this.transform.GetChild(2).gameObject.activeSelf == true)
            {
                int tippedCoins = Random.Range(3, 15);
                PlayerAction.Instance.IncreaseCoins(tippedCoins);
                m_UIManager.NotificationQueue.Enqueue($"<color=yellow>+{tippedCoins}</color> Koin");
                this.transform.GetChild(2).gameObject.SetActive(false);
                m_Hoverable.IsHoverable = false;
            }
        }
    }
}

