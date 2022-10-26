namespace NPC.Cashier
{
    public class FurnitureCashier : Cashier
    {
        protected override void OnInteract()
        {
            m_PlayerAction.ToggleFurnitureStoreUI?.Invoke( m_SpawnedItemTf );
        }
    }
}


