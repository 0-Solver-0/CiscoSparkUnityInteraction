using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class WindowsHandle : Singleton<WindowsHandle>, IPointerDownHandler
{

    

    public List<GameObject> Windows;
    public List<string> Titles;
    public List<string> Keys;
    
    void Start() {
        for (int i = 0; i < Windows.Count;i++ )
            Windows[i].SetActive(false);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            OpenWindowFrom();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllWindow();
        }
    }

    void OpenWindowFrom() {

        for (int i = 0; i < Keys.Count; i++) {
            if (Input.GetKeyDown(Keys[i]))
            {
                if (!Windows[i].activeSelf)
                {
                    Windows[i].GetComponent<WindowsData>().SetTitel(Titles[i]);
                  //  Windows[i].GetComponent<RectTransform>().localPosition = Vector3.zero;
                    Windows[i].SetActive(true);
                }
                return;
            }
        }
    }

    void CloseAllWindow() {

        for (int i = 0; i < Keys.Count; i++)
        {
                if (Windows[i].activeSelf)
                {
                    Windows[i].SetActive(false);
                }
        }
        /*
        if (TooltipHandle.GetInstance().IsActive())
        {
            TooltipHandle.GetInstance().Deactivate();
        }
         */
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Windows[0].activeSelf)
        {
            Windows[0].GetComponent<WindowsData>().SetTitel(Titles[0]);
            Windows[0].GetComponent<RectTransform>().localPosition = Vector3.zero;
            Windows[0].SetActive(true);
        }
    }
    private int GetIndexWindow(string name)
    {
        for (int i = 0; i < Windows.Count; i++)
            if (Windows[i].name == name)
            {
                return i;
            }
        return -1;

    }
    public bool ChangeWindowTitle(string name,string newTitle  ) {
        int index = GetIndexWindow(name);
        if (index != -1)
        {
            Titles[index] = newTitle;
            return true;
        }
        return false;
    }
    public GameObject GetWindowFrom(string name)
    {
        int index = GetIndexWindow(name);
        if (index != -1)
            return Windows[index];
        return null;
    }
}
