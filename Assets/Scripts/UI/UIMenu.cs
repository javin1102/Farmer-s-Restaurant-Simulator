using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    public UnityAction SelectAction { get => m_SelectAction; set => m_SelectAction =  value ; }
    private Toggle m_Toggle;
    private UnityAction m_SelectAction;


    private void Start()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener( Select );
    }

    private void Select( bool arg0 )
    {
        if ( arg0 == false ) return;
        m_Toggle.isOn = true;
        m_SelectAction?.Invoke();
    }
}
