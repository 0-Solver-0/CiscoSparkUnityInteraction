using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BotClick : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.parent.gameObject.GetComponent<Bot_Grid>().OnClickCell(this.gameObject);
    }
}
