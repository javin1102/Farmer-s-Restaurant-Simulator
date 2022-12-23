using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFarmBuy : MonoBehaviour
{
    private UIFarmStoreController m_FarmStoreController;
    [SerializeField] private Toggle m_SeedToggle;
    [SerializeField] private GameObject m_UIItemPrefab;
    [SerializeField] private GameObject m_Content;
    [SerializeField] private TMPro.TMP_Text m_CoinText;
    private readonly List<UIFarmItem> m_SeedItems = new(), m_CropItems = new();
    private PlayerAction m_PlayerAction;
    void OnEnable()
    {
        m_FarmStoreController = UIFarmStoreController.Instance as UIFarmStoreController;
    }
    private void Start()
    {
        m_PlayerAction = PlayerAction.Instance;
        m_FarmStoreController.SeedsData.ForEach(InstantiateSeedUIItem);
        m_SeedToggle.onValueChanged.AddListener(SeedsFilter);
        m_SeedItems.ForEach(EnableUIItem);
    }
    void Update()
    {
        m_CoinText.text = $"Koin Anda :<indent=55%><sprite=0><color=yellow>{m_PlayerAction.Coins}</color>";
    }


    private void SeedsFilter(bool arg0)
    {
        if (arg0 == false) return;
        m_CropItems.ForEach(DisableUIItem);
        m_SeedItems.ForEach(EnableUIItem);
    }

    void InstantiateSeedUIItem(SeedData seedData)
    {
        UIFarmItem uiItem = Instantiate(m_UIItemPrefab, m_Content.transform).GetComponent<UIFarmItem>();
        uiItem.ItemData = seedData;
        m_SeedItems.Add(uiItem);
        uiItem.gameObject.SetActive(false);
    }


    private void DisableUIItem(UIFarmItem obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void EnableUIItem(UIFarmItem obj)
    {
        obj.gameObject.SetActive(true);
    }

}
