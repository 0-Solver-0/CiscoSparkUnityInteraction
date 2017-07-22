using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WindowsData : MonoBehaviour {

    
    public void SetTitel(string title)
    {
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = title;
    }

}
