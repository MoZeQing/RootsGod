using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour,IDragHandler, IPointerDownHandler
{
    public RectTransform dragTarget;

    public void OnDrag(PointerEventData eventData)
    {
        dragTarget.anchoredPosition += eventData.delta;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragTarget.SetAsLastSibling();
    }
}
