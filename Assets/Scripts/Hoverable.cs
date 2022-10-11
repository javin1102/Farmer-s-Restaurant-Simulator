using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoverable : MonoBehaviour
{
    private int m_Layer;
    private bool m_IsHoverable;
    public bool IsHoverable { get => m_IsHoverable; set => m_IsHoverable = value; }
    private void Start()
    {
        m_Layer = gameObject.layer;
        m_IsHoverable = true;
    }
    public void HoverEnter()
    {
        if ( !m_IsHoverable ) return;
        gameObject.layer = Utils.OutlineLayer;
        transform.SetLayer( Utils.OutlineLayer );
    }
    public void HoverExit()
    {
        gameObject.layer = m_Layer;
        transform.SetLayer( m_Layer );
    }


}
