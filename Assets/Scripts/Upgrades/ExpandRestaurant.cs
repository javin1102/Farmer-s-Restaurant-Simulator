namespace Upgrades
{
    public class ExpandRestaurant : Upgrade
    {
        private RestaurantManager m_Restaurant;
        protected override int m_CurrentLevel { get => m_PlayerUpgrades.RestaurantExpandLevel; set => m_PlayerUpgrades.RestaurantExpandLevel = value; }

        protected override int m_MaxLevel => m_PlayerUpgrades.RESTAURANT_EXPAND_MAX_LEVEL;

        private void Start()
        {
            m_Restaurant = RestaurantManager.Instance;
        }
        protected override void UpgradeFeature()
        {
            m_Restaurant.ExpandRestaurant();
        }
    }
}

