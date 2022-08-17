using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIInventoryController : MonoBehaviour
{
    [SerializeField] private GameObject m_ItemSlotContentPrefab;
    private readonly List<UIInventorySlot> m_UIInventorySlots = new();
    private InventoryController m_InventoryController;
    private void OnEnable()
    {
        GetComponentsInChildren( true, m_UIInventorySlots );
        m_InventoryController = transform.root.GetComponent<InventoryController>();
        m_InventoryController.OnStoreNewItem += StoreNewItem;
    }
    private void OnDisable()
    {
        m_InventoryController.OnStoreNewItem -= StoreNewItem;
    }

    private void StoreNewItem( ItemSlotData data )
    {
        
        foreach ( UIInventorySlot slot in m_UIInventorySlots )
        {
            if ( slot.ItemSlotData == null )
            {
                slot.ItemSlotData = data;
                GameObject go = Instantiate( m_ItemSlotContentPrefab, slot.transform );
                go.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                Image iconImage = go.transform.GetChild( 0 ).GetComponent<Image>();
                TMPro.TMP_Text quantityText = go.transform.GetChild( 1 ).GetComponent<TMPro.TMP_Text>();
                slot.SetReference( iconImage, quantityText );
                slot.UpdateUI();
                break;
            }
        }
    }
}
