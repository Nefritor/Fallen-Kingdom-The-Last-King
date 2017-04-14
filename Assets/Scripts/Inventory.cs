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
    public bool isScrolling, isShowingInfo, isUsing;

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
        itemTitle = inventoryPanel.transform.FindChild("Item Title").GetComponent<Text>();
        itemType = inventoryPanel.transform.FindChild("Item Type").GetComponent<Text>();
        itemLevel = inventoryPanel.transform.FindChild("Item Level").GetComponent<Text>();
        itemDescription = inventoryPanel.transform.FindChild("Item Description").GetComponent<Text>();
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
        float posY = inventoryPanel.transform.localPosition.y;
        if (items[idItem].ID == -1)
        {
            scale = 0.7f;
            tempScale = 0.9f;
            tempIdItem = idItem;
            idItem -= 1;
            isScrolling = true;
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && !isScrolling && idItem != items.Count - 1 && !isShowingInfo && player.isInventory && items[idItem + 1].ID != -1 && useProccess == 0)
        {
            scale = 0.7f;
            tempScale = 0.9f;
            tempIdItem = idItem;
            idItem += 1;
            isScrolling = true;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && !isScrolling && idItem != 0 && !isShowingInfo && player.isInventory && useProccess == 0)
        {
            scale = 0.7f;
            tempScale = 0.9f;
            tempIdItem = idItem;
            idItem -= 1;
            isScrolling = true;
        }

        // Showing Item Info
        if (Input.GetButton("Sneak") && player.isInventory)
        {
            isShowingInfo = true;
            posY += -(Mathf.Pow(posY, 2) - 1300) * 0.005f;
            posY = Mathf.Clamp(posY, -33, 33);
            inventoryPanel.transform.localPosition = new Vector2(inventoryPanel.transform.localPosition.x, posY);
        }
        else if (!Input.GetButton("Sneak"))
        {
            posY -= -(Mathf.Pow(posY, 2) - 1300) * 0.005f;
            posY = Mathf.Clamp(posY, -33, 33);
            inventoryPanel.transform.localPosition = new Vector2(inventoryPanel.transform.localPosition.x, posY);
            isShowingInfo = false;
        }

        // Using item
        if (Input.GetButton("Attack") && !isScrolling && !isShowingInfo && player.isInventory)
        {
            useProccess += -(Mathf.Pow(useProccess - 1, 2) - 1.15f) * 0.02f;
            Image im = simpleSlots[idItem].GetComponent<Image>();
            im.fillAmount = 1 - useProccess;
            if (useProccess >= 1)
            {
                useProccess = 0;
                Use(idItem);
                isUsing = true;
                DropItem(idItem, isUsing);
            }
        }// Dropping item
        else if (Input.GetButton("Attack") && !isScrolling && isShowingInfo && player.isInventory)
        {
            useProccess += -(Mathf.Pow(useProccess - 1, 2) - 1.15f) * 0.02f;
            Image im = simpleSlots[idItem].GetComponent<Image>();
            im.fillAmount = 1 - useProccess;
            im.color = new Color(im.color.r, 1 - Mathf.Pow(useProccess, 0.3f), 1 - Mathf.Pow(useProccess, 0.3f));
            if (useProccess >= 1)
            {
                useProccess = 0;
                isUsing = false;
                DropItem(idItem, isUsing);
            }
        }
        else
        {
            useProccess -= -(Mathf.Pow(useProccess - 1, 2) - 1.15f) * 0.04f;
            useProccess = Mathf.Clamp(useProccess, 0, 1);
            Image im = simpleSlots[idItem].GetComponent<Image>();
            im.fillAmount = 1 - useProccess;
            im.color = new Color(1, 1, 1);
        }

        idItem = Mathf.Clamp(idItem, 0, items.Count - 1);

        GameObject selectedSlot = slotSimplePanel.transform.GetChild(idItem).gameObject;
        GameObject tempSelectedSlot = slotSimplePanel.transform.GetChild(tempIdItem).gameObject;

        if (isScrolling)
        {
            scale += -(Mathf.Pow(scale - 0.825f, 2) - 0.015f) * 2;
            scale = Mathf.Clamp(scale, 0.75f, 0.9f);
            selectedSlot.transform.localScale = new Vector2(scale, scale);

            tempScale -= -(Mathf.Pow(tempScale - 0.825f, 2) - 0.015f) * 2;
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
