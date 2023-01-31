namespace Upgrades
{
    public class ChefQuantity : Upgrade
    {
        private RestaurantManager m_RestaurantManager;
        protected override int m_CurrentLevel { get => m_PlayerUpgrades.ChefQuantityLevel; set => m_PlayerUpgrades.ChefQuantityLevel = value; }
        protected override int m_MaxLevel => m_PlayerUpgrades.CHEFQUANTITY_MAX_LEVEL;
        private void Start()
        {
            m_RestaurantManager = RestaurantManager.Instance;
        }
        protected override void UpgradeFeature()
        {
            m_RestaurantManager.AddChef();
        }


    }

}
