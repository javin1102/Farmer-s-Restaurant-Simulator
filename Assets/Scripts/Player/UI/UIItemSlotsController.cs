using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIItemSlotsController : MonoBehaviour
{
    [SerializeField] protected GameObject m_SlotsParentGO;
    [SerializeField] protected UIItemSlot[] m_UIItemSlots;
    protected virtual void Awake()
    {
        m_UIItemSlots = m_SlotsParentGO.GetComponentsInChildren<UIItemSlot>( true );
    }
}
