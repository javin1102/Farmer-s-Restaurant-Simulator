using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIExtraController : MonoBehaviour
{
    [SerializeField] protected Button m_IncreaseButton, m_DecreaseButton, m_ActionButton;
    [SerializeField] protected TMP_InputField m_InputField;
    protected UIInventoryController m_UIInventoryController;
    protected ItemDatabase m_ItemDatabase;
    protected PlayerAction m_PlayerAction;
    protected int m_Quantity;

    private void Start()
    {
        m_ItemDatabase = transform.root.GetComponent<ItemDatabase>();
        m_PlayerAction = transform.root.GetComponent<PlayerAction>();
        m_UIInventoryController = transform.parent.GetComponent<UIInventoryController>();
        m_IncreaseButton.onClick.AddListener( IncreaseQuantity );
        m_DecreaseButton.onClick.AddListener( DecreaseQuantity );
        m_InputField.onValueChanged.AddListener( ConvertText );
        m_InputField.onEndEdit.AddListener( Validate );
        m_ActionButton.onClick.AddListener( DoAction );
    }

    protected virtual void Validate( string arg0 )
    {
        if ( m_UIInventoryController.SelectedSlot == null ) return;
        if ( m_Quantity >= m_UIInventoryController.SelectedSlot.Slot.quantity )
        {
            m_Quantity = m_UIInventoryController.SelectedSlot.Slot.quantity;
            m_InputField.text = m_Quantity.ToString();
            return;
        }

        if ( m_Quantity <= 0 )
        {
            m_Quantity = 0;
            m_InputField.text = m_Quantity.ToString();
        }
    }
    protected void Update()
    {
        if ( m_UIInventoryController.SelectedSlot == null ) {
            gameObject.SetActive( false );
            return;
        } 
    }
    protected void OnDisable()
    {
        m_InputField.text = "";
        m_Quantity = 0;
    }

    protected void DoAction()
    {
        ButtonAction();
        if ( m_UIInventoryController.SelectedSlot.Slot.quantity <= 0 )
        {
            m_UIInventoryController.SelectedSlot.ResetSprite();
            m_UIInventoryController.SelectedSlot = null;
          
        }
        m_PlayerAction.OnDecreaseItemDatabase?.Invoke();
    }

    protected abstract void ButtonAction();
    

    private void ConvertText( string arg0 )
    {

        Int32.TryParse( arg0, out m_Quantity );
    }

    private void IncreaseQuantity()
    {
        if ( m_Quantity >= m_UIInventoryController.SelectedSlot.Slot.quantity )
        {
            m_Quantity = m_UIInventoryController.SelectedSlot.Slot.quantity;
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
