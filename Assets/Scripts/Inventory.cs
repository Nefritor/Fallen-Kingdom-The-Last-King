using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    ItemDatabase database;
    GameObject inventoryPanel, slotSimplePanel, inventoryQuickPanel, slotQuickPanel, selectedSlot, tempSelectedSlot, inventoryInfo;
    public GameObject inventorySimpleSlot, inventorySimpleItem, inventoryQuickSlot, inventoryQuickItem;
    Text itemTitle, itemType, itemLevel, itemDescription;

    int simpleSlotAmount, quickSlotAmount, idItem, tempIdItem;
    public List<Item> items = new List<Item>();
    public List<GameObject> simpleSlots = new List<GameObject>();
    public List<GameObject> quickSlots = new List<GameObject>();
    float scale, tempScale;
    bool isScrolling, isShowingInfo;

    private void Start()
    {
        isShowingInfo = false;
        isScrolling = false;
        tempIdItem = 0;
        idItem = 0;
        database = GetComponent<ItemDatabase>();
        simpleSlotAmount = 10;
        quickSlotAmount = 2;
        inventoryPanel = GameObject.Find("Inventory Panel");
        inventoryInfo = GameObject.Find("Inventory Info");
        itemTitle = inventoryInfo.transform.FindChild("Item Title").GetComponent<Text>();
        itemType = inventoryInfo.transform.FindChild("Item Type").GetComponent<Text>();
        itemLevel = inventoryInfo.transform.FindChild("Item Level").GetComponent<Text>();
        itemDescription = inventoryInfo.transform.FindChild("Item Description").GetComponent<Text>();
        slotSimplePanel = inventoryPanel.transform.FindChild("Simple Slots").gameObject;
        slotQuickPanel = inventoryPanel.transform.FindChild("Quick Slots").gameObject;
        for (int i = 0; i < simpleSlotAmount; i++)
        {
            items.Add(new Item());
            simpleSlots.Add(Instantiate(inventorySimpleSlot));
            simpleSlots[i].transform.SetParent(slotSimplePanel.transform);
        }
        for (int i = 0; i < quickSlotAmount; i++)
        {
            quickSlots.Add(Instantiate(inventoryQuickSlot));
            quickSlots[i].transform.SetParent(slotQuickPanel.transform);
        }
        AddItem(0);
        AddItem(1);
        AddItem(0);
        AddItem(0);
        slotSimplePanel.transform.GetChild(0).gameObject.transform.localScale = new Vector2(0.9f, 0.9f);
        Debug.Log(items[0].Title);
    }

    public void AddItem(int id)
    {
        Item itemToAdd = database.FetchItemByID(id);
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == -1)
            {
                items[i] = itemToAdd;
                GameObject itemObj = Instantiate(inventorySimpleItem);
                itemObj.transform.SetParent(simpleSlots[i].transform);
                itemObj.transform.position = Vector2.zero;
                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                itemObj.name = itemToAdd.Title;
                break;
            }
        }
    }

    private void Update()
    {
        ItemSelectingListener();
    }

    void ItemSelectingListener()
    {
        float panelX = inventoryInfo.transform.localPosition.x;
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && !isScrolling && idItem != items.Count - 1 && !isShowingInfo)
        {
            scale = 0.7f;
            tempScale = 0.9f;
            tempIdItem = idItem;
            idItem += 1;
            isScrolling = true;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && !isScrolling && idItem != 0 && !isShowingInfo)
        {
            scale = 0.7f;
            tempScale = 0.9f;
            tempIdItem = idItem;
            idItem -= 1;
            isScrolling = true;
        }

        Debug.Log(isShowingInfo);
        if (Input.GetButton("Sneak"))
        {
            isShowingInfo = true;
            panelX += -(Mathf.Pow(panelX - 152.2f, 2) - 5128f) * 0.002f;
            panelX = Mathf.Clamp(panelX, 80.6f, 223.8f);
            inventoryInfo.transform.localPosition = new Vector2(panelX, inventoryInfo.transform.localPosition.y);
        }
        else if (!Input.GetButton("Sneak"))
        {
            panelX -= -(Mathf.Pow(panelX - 152.2f, 2) - 5128f) * 0.002f;
            panelX = Mathf.Clamp(panelX, 80.6f, 223.8f);
            inventoryInfo.transform.localPosition = new Vector2(panelX, inventoryInfo.transform.localPosition.y);
            if (panelX <= 81.1f)
            {
                isShowingInfo = false;
            }
        }

        idItem = Mathf.Clamp(idItem, 0, items.Count - 1);
        
        GameObject selectedSlot = slotSimplePanel.transform.GetChild(idItem).gameObject;
        GameObject tempSelectedSlot = slotSimplePanel.transform.GetChild(tempIdItem).gameObject;

        if (isScrolling)
        {
            scale += -(Mathf.Pow(scale - 0.825f, 2) - 0.01f) * 2;
            scale = Mathf.Clamp(scale, 0.75f, 0.9f);
            selectedSlot.transform.localScale = new Vector2(scale, scale);

            tempScale -= -(Mathf.Pow(tempScale - 0.825f, 2) - 0.01f) * 2;
            tempScale = Mathf.Clamp(tempScale, 0.75f, 0.9f);
            tempSelectedSlot.transform.localScale = new Vector2(tempScale, tempScale);
            if (scale >= 0.9f && tempScale <= 0.75f)
            {
                isScrolling = false;
            }
        }

        if (isShowingInfo)
        {
            itemTitle.text = items[idItem].Title;
            string itemTypeString = "";
            switch (items[idItem].Type)
            {
                case "meal":
                    itemTypeString = "Еда";
                    break;
                case "drink":
                    itemTypeString = "Питьё";
                    break;
                case "rune":
                    itemTypeString = "Руна";
                    break;
                case "stone":
                    itemTypeString = "Точильный камень";
                    break;
            }
            itemType.text = itemTypeString;
            itemLevel.text = "Уровень предмета: " + items[idItem].Level;
            itemDescription.text = items[idItem].Description;
         }
    }
}
