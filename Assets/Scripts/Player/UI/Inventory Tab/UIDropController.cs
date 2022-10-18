using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDropController : MonoBehaviour
{
    [SerializeField] private Button m_IncreaseButton, m_DecreaseButton, m_DropButton;
    [SerializeField] private TMP_InputField m_InputField;
    private UIInventoryController m_UIInventoryController;
    private ItemDatabase m_ItemDatabase;    
    //private InventoryController m_InventoryController;
    private int m_Quantity;

    private void Start()
    {
        m_ItemDatabase = transform.root.GetComponent<ItemDatabase>();
        m_UIInventoryController = transform.parent.GetComponent<UIInventoryController>();
        m_IncreaseButton.onClick.AddListener( IncreaseQuantity );
        m_DecreaseButton.onClick.AddListener( DecreaseQuantity );
        m_InputField.onValueChanged.AddListener( ConvertText );
        m_DropButton.onClick.AddListener( Drop );
    }

    private void OnDisable()
    {
        m_InputField.text = "";
        m_Quantity = 0;
    }
    private void Drop()
    {
        m_ItemDatabase.Drop( m_UIInventoryController.SelectedItem.data, m_Quantity );
        gameObject.SetActive( false );
    }

    private void ConvertText( string arg0 )
    {

        Int32.TryParse( arg0, out m_Quantity );

        if ( m_Quantity >= m_UIInventoryController.SelectedItem.quantity )
        {
            m_Quantity = m_UIInventoryController.SelectedItem.quantity;
            m_InputField.text = m_Quantity.ToString();
            return;
        }

        if ( m_Quantity <= 0 )
        {
            m_Quantity = 0;
            m_InputField.text = m_Quantity.ToString();
        }
    }

    private void IncreaseQuantity()
    {
        if ( m_Quantity >= m_UIInventoryController.SelectedItem.quantity ) {
            m_Quantity = m_UIInventoryController.SelectedItem.quantity;
            m_InputField.text = m_Quantity.ToString();
            return;
        }

        m_Quantity += 1;
        m_InputField.text = m_Quantity.ToString();
    }

    private void DecreaseQuantity()
    {
        if ( m_Quantity <= 0 ) return;
        m_Quantity -= 1;
        m_InputField.text = m_Quantity.ToString();
    }
}
