using UnityEngine;

public class UIFarmQuiz : UIStoreQuiz
{
    private UIFarmStoreController m_FarmStoreController;
    private void Start()
    {
        m_FarmStoreController = UIFarmStoreController.Instance as UIFarmStoreController;
    }
    protected override void SetReward()
    {
        m_FarmStoreController.SpawnItem(m_FarmStoreController.SeedsData[Random.Range(0, m_FarmStoreController.SeedsData.Count)]);
    }
}
