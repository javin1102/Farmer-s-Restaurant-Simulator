namespace Upgrades
{
    public class ChefQuantity : Upgrade
    {
        // [SerializeField] private RestaurantUpgradesChannel m_RestaurantUpgradesChannel;

        private RestaurantManager m_RestaurantManager;
        protected override int m_CurrentLevel { get => m_PlayerUpgrades.ChefQuantityLevel; set => m_PlayerUpgrades.ChefQuantityLevel = value; }

        protected override int m_MaxLevel => 3;
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
