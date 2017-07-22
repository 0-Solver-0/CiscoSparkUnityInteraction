using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class Bot_Grid : MonoBehaviour
{

    // Add Filtre to the Grid.
    public GameObject Bots;
    public GameObject IWS_Slot;
    public GameObject IWS_Item;

    public bool MultiSelection = true;

    void Start()
    {

        if (Bots == null)
        {
            Debug.LogWarning("Bots Game Object Name is messing");
            return;
        }
        foreach (GameObject bot in Bots.GetComponent<BotConfig>().Bots)
        {
            GameObject goSlot = Instantiate(IWS_Slot);
            GameObject goBot = Instantiate(IWS_Item);
            goBot.transform.SetParent(goSlot.transform);
            goSlot.transform.SetParent(this.transform);
            goBot.GetComponent<Image>().sprite = bot.GetComponent<SpriteRenderer>().sprite;
            goSlot.name = bot.name;
            goBot.name = bot.name;
            goBot.transform.position = Vector2.zero;
        }
    }

    public void OnClickCell(GameObject go)
    {
        int index;
        if (!MultiSelection && BotSelected.CurrentItemCount() == 1)
        {
            index = BotSelected.GetCurrentItemIndex(go.name);
            if (index != -1)
            {
                changeSlotBg(go, "Slot", new Color(1f, 1f, 1f, 1f));
                BotSelected.removeCurrentItem(index);
            }
            return;
        }
        // pupulate with the gameObject presented in the game
        index = BotSelected.addCurrentItem(go);
        if (index == -1)
            changeSlotBg(go, "CurrentSlot", new Color(1f, 0.92f, 0.016f, 1f));
        else
        {
            changeSlotBg(go, "Slot", new Color(1f, 1f, 1f, 1f));
            BotSelected.removeCurrentItem(index);
        }

    }
    private void changeSlotBg(GameObject current, string slug, Color color)
    {
        current.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/" + slug);
        current.GetComponent<Image>().color = color;
    }
}