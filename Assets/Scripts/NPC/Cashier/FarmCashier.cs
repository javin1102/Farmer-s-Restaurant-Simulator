namespace NPC.Cashier
{
    public class FarmCashier : Cashier
    {
        protected override void OnInteract()
        {
            m_PlayerAction.ToggleSeedStoreUI?.Invoke( m_SpawnedItemTf );
        }
    }
}

