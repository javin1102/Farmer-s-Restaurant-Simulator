using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFurnitureItem : MonoBehaviour
{
    public FurnitureData FurnitureData { get => m_FurnitureData; set => m_FurnitureData = value; }
    private FurnitureData m_FurnitureData;
    private UIFurnitureStoreController m_StoreController;
    [SerializeField] private TMP_Text m_NameText, m_PriceText;
    [SerializeField] private Image m_Icon;
    [SerializeField] private Button m_BuyButton;

    private void Start()
    {
        m_StoreController = UIFurnitureStoreController.Instance as UIFurnitureStoreController;
        UpdateUI();
        m_BuyButton.onClick.AddListener( BuyItem );
    }

    private void BuyItem()
    {
        m_StoreController.SpawnItem( m_FurnitureData );
    }

    public void UpdateUI()
    {
        m_NameText.text = m_FurnitureData.id;
        m_PriceText.text = $"<sprite=0> {m_FurnitureData.buyPrice}";
        m_Icon.sprite = m_FurnitureData.icon;
    }
}
