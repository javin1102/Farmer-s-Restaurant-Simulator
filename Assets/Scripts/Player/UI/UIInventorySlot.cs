using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class UIInventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeReference] private ItemSlotData m_ItemSlotData;
    [SerializeField] private Image m_IconImage;
    [SerializeField] private TMP_Text m_QuantityText;
    private InventoryController m_InventoryController;
    public ItemSlotData ItemSlotData { get => m_ItemSlotData; set => m_ItemSlotData = value; }
    private void OnEnable()
    {
        //m_ItemSlotData = new();
        //print( m_ItemSlotData );
        m_InventoryController = transform.root.GetComponent<InventoryController>();
        m_InventoryController.OnStoreExistingItem += UpdateUI;
    }

    private void OnDisable()
    {
        m_InventoryController.OnStoreExistingItem -= UpdateUI;
    }
    public void UpdateUI()
    {
        if ( m_IconImage == null || m_QuantityText == null ) return;
        m_IconImage.sprite = m_ItemSlotData.itemData.icon;
        m_QuantityText.text = m_ItemSlotData.quantity.ToString();
    }


    public void SetReference( Image iconImage, TMP_Text quantityText )
    {
        m_IconImage = iconImage;
        m_QuantityText = quantityText;
    }
    public void OnDrop( PointerEventData eventData )
    {
        Transform targetContentTf = eventData.pointerDrag.transform;
        UIInventorySlot OriginUIInventorySlot = targetContentTf.parent.GetComponent<UIInventorySlot>();
        ItemSlotData originSlotData = OriginUIInventorySlot.m_ItemSlotData;
        if ( m_ItemSlotData != null )
        {
            print( "Test" );
            //ItemSlotData temp = m_ItemSlotData;
            (OriginUIInventorySlot.m_ItemSlotData, m_ItemSlotData) = (m_ItemSlotData, OriginUIInventorySlot.m_ItemSlotData);


        }
        else
        {
            m_ItemSlotData = originSlotData;
            originSlotData = null;
        }

        //If there is content, change the origin content to this content
        if ( transform.childCount == 1 )
        {
            Transform contentTf = transform.GetChild( 0 );
            RectTransform contentRectTf = contentTf.GetComponent<RectTransform>();
            contentTf.SetParent( targetContentTf.parent );
            contentRectTf.anchoredPosition = Vector2.zero;
            OriginUIInventorySlot.SetReference( m_IconImage, m_QuantityText );

        }
        else
        {
            OriginUIInventorySlot.SetReference( null, null );
        }

        //Set this slot content to target content
        targetContentTf.SetParent( transform );
        Image targetImage = targetContentTf.transform.GetChild( 0 ).GetComponent<Image>();
        TMP_Text targetText = targetContentTf.transform.GetChild( 1 ).GetComponent<TMP_Text>();
        SetReference( targetImage, targetText );
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

}
