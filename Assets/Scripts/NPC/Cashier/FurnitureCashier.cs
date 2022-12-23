using System;

namespace NPC.Cashier
{
    public class FurnitureCashier : Cashier
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
            m_UIManager.ShowActionHelperPrimary("Left", "Toko Furnitur");
        }

        protected override void OnInteract()
        {
            m_PlayerAction.ToggleFurnitureStoreUI?.Invoke(m_SpawnedItemTf);
        }
    }
}


