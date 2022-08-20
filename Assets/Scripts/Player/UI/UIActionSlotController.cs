using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActionSlotController : MonoBehaviour
{
    private InventoryController m_InventoryController;

    private void OnEnable()
    {
        m_InventoryController = transform.root.GetComponent<InventoryController>();
        m_InventoryController.OnDisableInventoryUI += Enable;
        m_InventoryController.OnEnableInventoryUI += Disable;
    }

    private void OnDisable()
    {
        m_InventoryController.OnDisableInventoryUI -= Enable;
        m_InventoryController.OnEnableInventoryUI -= Disable;
    }
    private void Enable() => gameObject.SetActive( true );
    private void Disable() => gameObject.SetActive( false );

}
