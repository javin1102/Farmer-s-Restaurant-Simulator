public class UIFarmItem : UIStoreItem
{
    private UIFarmStoreController m_FarmStoreController;

    protected override void BuyItem()
    {
        m_FarmStoreController.SpawnItem( m_ItemData );
    }

    private new void  Start()
    {
        base.Start();
        m_FarmStoreController = UIFarmStoreController.Instance as UIFarmStoreController;
    }


}
