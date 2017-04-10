using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    ItemDatabase database;
    GameObject inventoryPanel, slotSimplePanel, inventoryQuickPanel, slotQuickPanel, selectedSlot, tempSelectedSlot, inventoryInfo;
    public GameObject inventorySimpleSlot, inventorySimpleItem, inventoryQuickSlot, inventoryQuickItem;
    Text itemTitle, itemType, itemLevel, itemDescription;

    int simpleSlotAmount, quickSlotAmount, idItem, tempIdItem, emptySlot;
    public Player player;
    public List<Item> items = new List<Item>();
    public List<GameObject> simpleSlots = new List<GameObject>();
    public List<GameObject> quickSlots = new List<GameObject>();
    float scale, tempScale, useProccess;
    bool isScrolling, isShowingInfo, isUsed;

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
    }

    private void Update()
    {
        ItemSelectingListener();
        if (Input.GetKeyDown(KeyCode.I))
        {
            DropItem(0, false);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddItem(0);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddItem(1);
        }
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
                itemObj.transform.position = simpleSlots[i].transform.position;
                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                itemObj.name = itemToAdd.Title;
                break;
            }
        }
    }

    public void DropItem(int invItemId, bool isItemUsed)
    {
        if (simpleSlots[invItemId].transform.childCount != 0)
        {
            items[invItemId] = new Item();
            Destroy(simpleSlots[invItemId].transform.GetChild(0).gameObject);
            for (int i = invItemId; i < simpleSlots.Count - 1; i++)
            {
                if (simpleSlots[i + 1].transform.childCount != 0)
                {
                    GameObject tempSlot = simpleSlots[i + 1].transform.GetChild(0).gameObject;
                    Item tempItem = items[i + 1];
                    items[i + 1] = new Item();
                    items[i] = tempItem;
                    tempSlot.transform.SetParent(simpleSlots[i].transform, false);
                    tempSlot.transform.position = simpleSlots[i].transform.position;
                    tempSlot.GetComponent<Image>().sprite = items[i].Sprite;
                }
                else break;
            }
        }
    }

    void ItemSelectingListener()
    {
        float panelX = inventoryInfo.transform.localPosition.x;

        if (items[idItem].ID == -1)
        {
            scale = 0.7f;
            tempScale = 0.9f;
            tempIdItem = idItem;
            idItem -= 1;
            isScrolling = true;
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && !isScrolling && idItem != items.Count - 1 && !isShowingInfo && player.isInventory && items[idItem + 1].ID != -1)
        {
            scale = 0.7f;
            tempScale = 0.9f;
            tempIdItem = idItem;
            idItem += 1;
            isScrolling = true;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && !isScrolling && idItem != 0 && !isShowingInfo && player.isInventory)
        {
            scale = 0.7f;
            tempScale = 0.9f;
            tempIdItem = idItem;
            idItem -= 1;
            isScrolling = true;
        }

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

        // Using item
        if (Input.GetButton("Attack") && !isScrolling && !isShowingInfo && player.isInventory)
        {
            useProccess += 0.5f * Time.deltaTime;
            Debug.Log(useProccess);
            if (useProccess >= 1)
            {
                useProccess = 0;
                Use(idItem);
                isUsed = true;
                DropItem(idItem, isUsed);
            }
        }// Dropping item
        else if (Input.GetButton("Attack") && !isScrolling && isShowingInfo && player.isInventory)
        {
            useProccess += 0.5f * Time.deltaTime;
            Debug.Log(useProccess);
            if (useProccess >= 1)
            {
                useProccess = 0;
                isUsed = false;
                DropItem(idItem, isUsed);
            }
        }
        else
        {
            useProccess = 0;
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

    void Use(int itemId)
    {

    }
}
