public class UIFurnitureItem : UIStoreItem
{

    private UIFurnitureStoreController m_StoreController;

    private new void Start()
    {
        base.Start();
        m_StoreController = UIFurnitureStoreController.Instance as UIFurnitureStoreController;
    }

    protected override void BuyItem()
    {
        if (PlayerAction.Coins < m_ItemData.buyPrice) return;
        PlayerAction.Coins -= m_ItemData.buyPrice;
        m_StoreController.SpawnItem(m_ItemData);
    }
}
