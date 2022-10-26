using UnityEngine;

public class UIFurnitureQuiz : UIStoreQuiz
{
    private UIFurnitureStoreController m_FurnitureStoreController;
    private void Start()
    {
        m_FurnitureStoreController = UIFurnitureStoreController.Instance as UIFurnitureStoreController;
    }

    protected override void SetReward()
    {
        m_FurnitureStoreController.SpawnItem( m_FurnitureStoreController.FurnituresData[Random.Range( 0, m_FurnitureStoreController.FurnituresData.Count )] );
    }
}
