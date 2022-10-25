using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIIngredientTooltipDisabler : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private UIIngredientTooltip m_Tooltip;

    public void OnPointerEnter( PointerEventData eventData )
    {
        m_Tooltip.gameObject.SetActive( false );
    }
}
