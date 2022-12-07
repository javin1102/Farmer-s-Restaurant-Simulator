public class UIFarmItem : UIStoreItem
{
    private UIFarmStoreController m_FarmStoreController;

    private new void Start()
    {
        base.Start();
        m_FarmStoreController = UIFarmStoreController.Instance as UIFarmStoreController;
    }

    protected override void BuyItem()
    {
        if (PlayerAction.Coins < m_ItemData.buyPrice) return;
        PlayerAction.Coins -= m_ItemData.buyPrice;
        m_FarmStoreController.SpawnItem(m_ItemData);
    }
}
