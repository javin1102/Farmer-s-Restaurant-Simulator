using TMPro;
using UnityEngine;

public class UIDropItem : MonoBehaviour
{
    public Item Item { get => m_Item; set => m_Item = value; }
    private Item m_Item;
    [SerializeField] private TMP_Text m_QuantityText;
    private PlayerAction m_PlayerAction;

    private void Start()
    {
        m_PlayerAction = PlayerAction.Instance;
        m_Item = transform.parent.GetComponent<Item>();
        UpdateUI();
    }

    void LateUpdate()
    {
        if (!m_Item.IsDropState)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
            Vector3 dir = (transform.position - m_PlayerAction.transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }

    }

    private void UpdateUI()
    {
        gameObject.SetActive(true);
        m_QuantityText.text = $"{m_Item.DropQuantity}";
    }
}
