using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BotSelected  {

    private static List<GameObject> CurrentItems = new List<GameObject>();
    private static string Position ="";
    
    public static int GetCurrentItemIndex(string name)
    {
        for (int i = 0; i < CurrentItems.Count; i++)
            if (CurrentItems[i].name == name)
                return i;
        return -1;
    }
    public static int addCurrentItem(GameObject go)
    {
        int index = GetCurrentItemIndex(go.name);
        if (index == -1)
            CurrentItems.Add(go);
        return index;
    }
    public static void removeCurrentItem(int index)
    {
        CurrentItems.RemoveAt(index);
    }
    public static int CurrentItemCount()
    {
        return CurrentItems.Count;
    }
    public static List<GameObject> GetCurrentAll()
    {
        return CurrentItems;
    }
    public static void SetPostion(string response)
    {
        Position = response;
        // notify
    }
    public static string GetPostion()
    {
        return Position;
    }
}
