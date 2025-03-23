using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.pointerDrag = null; // Prevents dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Do nothing (blocks dragging)
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Do nothing
    }
}
