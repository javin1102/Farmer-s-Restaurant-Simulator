namespace NPC.Cashier
{
    public class FarmCashier : Cashier
    {
        private void Start()
        {
            m_Hoverable.OnHoverEnter += ShowHelper;
            m_Hoverable.OnHoverExit += HideHelper;
        }

        private void OnDestroy()
        {
            m_Hoverable.OnHoverEnter -= ShowHelper;
            m_Hoverable.OnHoverExit -= HideHelper;
        }

        private void HideHelper()
        {
            m_UIManager.HideActionHelper();
        }

        private void ShowHelper()
        {
            m_UIManager.ShowActionHelperPrimary("Left", "Toko Tanaman");
        }
        protected override void OnInteract()
        {
            m_PlayerAction.ToggleSeedStoreUI?.Invoke(m_SpawnedItemTf);
        }
    }
}

