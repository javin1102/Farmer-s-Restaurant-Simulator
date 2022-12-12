using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDraggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform m_RectTransform;
    private CanvasGroup m_CanvasGroup;
    private Canvas m_Canvas;
    public static bool isDragging = false;
    private void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_Canvas = GetComponent<Canvas>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        //Divide with canvas scale factor
        m_RectTransform.anchoredPosition += eventData.delta / m_Canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_CanvasGroup.blocksRaycasts = true;
        m_CanvasGroup.alpha = 1f;
        m_Canvas.sortingOrder = 5;
        m_RectTransform.anchoredPosition = Vector3.zero;
        isDragging = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        m_CanvasGroup.blocksRaycasts = false;
        m_CanvasGroup.alpha = .5f;
        m_Canvas.sortingOrder = 10;
    }
}
