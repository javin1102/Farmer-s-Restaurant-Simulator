using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryController : MonoBehaviour
{
    private InventoryController m_InventoryController;

    private void OnEnable()
    {
        m_InventoryController = transform.root.GetComponent<InventoryController>();
        m_InventoryController.InvokeEnableInventoryUIEvent();
    }

    private void OnDisable()
    {
        m_InventoryController.InvokeDisableInventoryUIEvent();
    }

}
