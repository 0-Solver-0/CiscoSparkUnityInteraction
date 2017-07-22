using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class WindowsClose : MonoBehaviour, IPointerDownHandler
{
    public GameObject Window;

    public void OnPointerDown(PointerEventData eventData)
    {      
        if (Window.activeSelf && Window!=null)
        {
            Window.SetActive(false);
        }
    }

}
