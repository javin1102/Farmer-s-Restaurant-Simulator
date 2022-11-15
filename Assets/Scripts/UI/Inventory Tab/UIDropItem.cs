using TMPro;
using UnityEngine;

public class UIDropItem : MonoBehaviour
{
    public Item Item { get => m_Item; set => m_Item =  value ; }
    private Item m_Item;
    [SerializeField] private TMP_Text m_QuantityText;

    private void Start()
    {
        m_Item = transform.parent.GetComponent<Item>();
        if ( m_Item.DropQuantity <= 1 ) gameObject.SetActive( false );
        else
        {
            m_QuantityText.text = $"{m_Item.DropQuantity}";
        }
        m_Item.OnDrop += EnableUI;
    }

    private void EnableUI() => gameObject.SetActive( true );
}
