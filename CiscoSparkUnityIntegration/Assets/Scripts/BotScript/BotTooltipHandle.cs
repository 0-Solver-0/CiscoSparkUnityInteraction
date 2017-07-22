using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class BotTooltipHandle : Singleton<BotTooltipHandle>
{

    public GameObject Window;
    public bool isActive;
    public float closeAfter = 0f;

    void Start()
    {
        //Window = gameObject;
        Window.SetActive(false);
        isActive = false;
    }
    public void Activate(string data)
    {

        Window.transform.FindChild("TooltipText").GetComponent<Text>().text = data;
        isActive = true;
        Window.SetActive(isActive);
        
        //Window.transform.position = Input.mousePosition;
        if (closeAfter != 0f) {
            StartCoroutine(CloseAfter());
        }
    }
    public void Deactivate()
    {
        isActive = false;
        Window.SetActive(isActive);
    }
    public bool IsActive() { return isActive; }

    private IEnumerator CloseAfter()
    {
        yield return new WaitForSeconds(closeAfter);
        Deactivate();
        closeAfter = 0f;
    }
}
