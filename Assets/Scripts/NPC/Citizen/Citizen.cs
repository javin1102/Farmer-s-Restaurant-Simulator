using UnityEngine;
using System.Collections;
namespace NPC.Citizen
{

    public class Citizen : NPCManager, IInteractable
    {

        public Food ServedFood { get => m_ServedFood; set => m_ServedFood = value; }
        public bool TravelBackwards { get => m_TravelBackwards; set => m_TravelBackwards = value; }
        public Waypoint CurrentWaypoint { get => m_CurrentWaypoint; set => m_CurrentWaypoint = value; }

        public float m_CoinDelay = 15f;

        [SerializeField] private Waypoint m_CurrentWaypoint;
        private Food m_ServedFood;
        private UIManager m_UIManager;
        [SerializeField] private bool m_TravelBackwards;
        private void Start()
        {
            m_UIManager = UIManager.Instance;

        }

        private void OnEnable()
        {
            ChangeState(new IdleState());
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
                this.transform.GetChild(2).gameObject.SetActive(true);
                yield return new WaitForSeconds(m_CoinDelay);
                this.transform.GetChild(2).gameObject.SetActive(false);
            }
            else yield return null;
        }

        public void Interact(PlayerAction m_PlayerAction)
        {
            if (this.transform.GetChild(2).gameObject.activeSelf == true)
            {
                int tippedCoins = Random.Range(5, 40);
                PlayerAction.Instance.IncreaseCoins(tippedCoins);
                m_UIManager.NotificationQueue.Enqueue($"<color=yellow>+{tippedCoins}</color> Koin");
                this.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }
}

