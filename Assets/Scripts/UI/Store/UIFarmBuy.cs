using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFarmBuy : MonoBehaviour
{
    private UIFarmStoreController m_FarmStoreController;
    [SerializeField] private Toggle m_SeedToggle, m_CropToggle;
    [SerializeField] private GameObject m_UIItemPrefab;
    [SerializeField] private GameObject m_Content;
    private readonly List<UIFarmItem> m_SeedItems = new(), m_CropItems = new();
    void OnEnable()
    {
        m_FarmStoreController = UIFarmStoreController.Instance as UIFarmStoreController;
    }
    private void Start()
    {

        m_FarmStoreController.SeedsData.ForEach(InstantiateSeedUIItem);
        m_FarmStoreController.CropsData.ForEach(InstantiateCropUIItem);
        m_SeedToggle.onValueChanged.AddListener(SeedsFilter);
        m_CropToggle.onValueChanged.AddListener(CropsFilter);
        m_SeedItems.ForEach(EnableUIItem);
    }

    private void CropsFilter(bool arg0)
    {
        if (arg0 == false) return;
        m_SeedItems.ForEach(DisableUIItem);
        m_CropItems.ForEach(EnableUIItem);
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

    void InstantiateCropUIItem(ItemData cropData)
    {
        UIFarmItem uiItem = Instantiate(m_UIItemPrefab, m_Content.transform).GetComponent<UIFarmItem>();
        uiItem.ItemData = cropData;
        m_CropItems.Add(uiItem);
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
