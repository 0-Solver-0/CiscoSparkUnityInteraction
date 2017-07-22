using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class WindowsDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{


    private Vector2 offset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
        this.transform.position = eventData.position - offset;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        this.transform.position = eventData.position - offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
    }
}
