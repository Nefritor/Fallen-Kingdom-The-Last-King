using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    ItemDatabase database;
    GameObject inventoryPanel, slotSimplePanel, inventoryQuickPanel, slotQuickPanel, selectedSlot, tempSelectedSlot, inventoryInfo;
    public GameObject inventorySimpleSlot, inventorySimpleItem, inventoryQuickSlot, inventoryQuickItem, labelQuickItem;
    Text itemTitle, itemType, itemLevel, itemDescription;

    int simpleSlotAmount, quickSlotAmount, idItem, tempIdItem, emptySlot, quickEId, quickQId;
    public Player player;
    public List<Item> items = new List<Item>();
    public List<Item> quickItems = new List<Item>();
    public List<GameObject> simpleSlots = new List<GameObject>();
    public List<GameObject> quickSlots = new List<GameObject>();
    float scale, tempScale, useProccess;
    public bool isScrolling, isShowingInfo, isUsing, isFirstNull;

    private void Start()
    {
        quickEId = -1;
        quickQId = -1;
        isShowingInfo = false;
        isScrolling = false;
        tempIdItem = 1;
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
            quickItems.Add(new Item());
            quickSlots.Add(Instantiate(inventoryQuickSlot));
            quickSlots[i].transform.SetParent(slotQuickPanel.transform);
        }
        GameObject itemObj = Instantiate(inventoryQuickItem);
        itemObj.transform.SetParent(quickSlots[0].transform);
        itemObj.transform.position = quickSlots[0].transform.position;
        itemObj.GetComponent<Image>().sprite = new Item().Sprite;
        itemObj.name = new Item().Title;

        itemObj = Instantiate(inventoryQuickItem);
        itemObj.transform.SetParent(quickSlots[1].transform);
        itemObj.transform.position = quickSlots[1].transform.position;
        itemObj.GetComponent<Image>().sprite = new Item().Sprite;
        itemObj.name = new Item().Title;

        itemObj = Instantiate(labelQuickItem);
        itemObj.transform.GetChild(0).GetComponent<Text>().text = "Q";
        itemObj.transform.SetParent(quickSlots[0].transform);
        itemObj.transform.position = new Vector2(quickSlots[0].transform.position.x - 13, quickSlots[0].transform.position.y - 13);

        itemObj = Instantiate(labelQuickItem);
        itemObj.transform.GetChild(0).GetComponent<Text>().text = "E";
        itemObj.transform.SetParent(quickSlots[1].transform);
        itemObj.transform.position = new Vector2(quickSlots[1].transform.position.x - 13, quickSlots[1].transform.position.y - 13);

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

    public void SetQuickItem(KeyCode key, int id)
    {
        if (key == KeyCode.Q)
        {
            if (id == quickEId)
            {
                Debug.Log("Этот предмет уже забинден на кнопку E");
            } else { 
            quickQId = id;
            quickItems[0] = items[id];
                if (quickSlots[0].transform.childCount != 0)
                {
                    quickSlots[0].transform.GetChild(0).GetComponent<Image>().sprite = items[id].Sprite;
                    quickSlots[0].transform.GetChild(0).name = items[id].Title;
                }
            }
        } else if (key == KeyCode.E)
        {
            if (id == quickQId)
            {
                Debug.Log("Этот предмет уже забинден на кнопку Q");
            }
            else
            {
                quickEId = id;
                quickItems[1] = items[id];
                if (quickSlots[1].transform.childCount != 0)
                {
                    quickSlots[1].transform.GetChild(0).GetComponent<Image>().sprite = items[id].Sprite;
                    quickSlots[1].transform.GetChild(0).name = items[id].Title;
                }
            }
        }
    }

    public void AddItem(int id)
    {
        if (items[0].ID == -1)
        {
            idItem = 0;
            tempIdItem = 1;
            scale = 0.7f;
            tempScale = 0.9f;
            isScrolling = true;
        }
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
            if (invItemId < quickEId)
            {
                quickEId--;
            } else if (invItemId == quickEId)
            {
                quickEId = -1;
                quickSlots[1].transform.GetChild(0).GetComponent<Image>().sprite = new Item().Sprite;
                quickSlots[1].transform.GetChild(0).name = new Item().Title;
            }
            if (invItemId < quickQId)
            {
                quickQId--;
            }
            else if (invItemId == quickQId)
            {
                quickQId = -1;
                quickSlots[0].transform.GetChild(0).GetComponent<Image>().sprite = new Item().Sprite;
                quickSlots[0].transform.GetChild(0).name = new Item().Title;
            }
            items[invItemId] = new Item();
            Destroy(simpleSlots[invItemId].transform.GetChild(0).gameObject);            
            for (int i = invItemId; i < simpleSlots.Count - 1; i++)
            {
                Debug.Log(i);
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
            if (items[0].ID == -1)
            {
                tempScale = 0.9f;
                isFirstNull = true;
            }
        }
    }

    void ItemSelectingListener()
    {
        float posY = inventoryPanel.transform.localPosition.y;
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

        // Use Quick item
        if (Input.GetKeyDown(KeyCode.E) && !player.isInventory)
        {
            Use(quickEId);
            isUsing = true;
            DropItem(quickEId, isUsing);
        } else if (Input.GetKeyDown(KeyCode.Q) && !player.isInventory)
        {
            Use(quickQId);
            isUsing = true;
            DropItem(quickQId, isUsing);
        }

        // Set Quick Item
        if (Input.GetKeyDown(KeyCode.E) && player.isInventory)
        {
            SetQuickItem(KeyCode.E, idItem);
        } else if (Input.GetKeyDown(KeyCode.Q) && player.isInventory)
        {
            SetQuickItem(KeyCode.Q, idItem);
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
                if (items[idItem + 1].ID == -1 && idItem != 0)
                {
                    scale = 0.7f;
                    tempScale = 0.9f;
                    tempIdItem = idItem;
                    idItem -= 1;
                    isScrolling = true;
                }
            }
        }
        // Dropping item
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
                if (items[idItem + 1].ID == -1 && idItem != 0)
                {
                    scale = 0.7f;
                    tempScale = 0.9f;
                    tempIdItem = idItem;
                    idItem -= 1;
                    isScrolling = true;
                }
            }
        }
        else if (idItem != -1)
        {
            useProccess -= -(Mathf.Pow(useProccess - 1, 2) - 1.15f) * 0.04f;
            useProccess = Mathf.Clamp(useProccess, 0, 1);
            Image im = simpleSlots[idItem].GetComponent<Image>();
            im.fillAmount = 1 - useProccess;
            im.color = new Color(1, 1, 1);
        }
        
        Image tempIm = simpleSlots[tempIdItem].GetComponent<Image>();
        tempIm.fillAmount = 1;
        tempIm.color = new Color(1, 1, 1);

        idItem = Mathf.Clamp(idItem, 0, items.Count - 1);

        GameObject selectedSlot = slotSimplePanel.transform.GetChild(idItem).gameObject;
        GameObject tempSelectedSlot = slotSimplePanel.transform.GetChild(tempIdItem).gameObject;
        GameObject firstSlot = slotSimplePanel.transform.GetChild(0).gameObject;

        if (isFirstNull)
        {
            tempScale -= -(Mathf.Pow(tempScale - 0.825f, 2) - 0.015f) * 2;
            tempScale = Mathf.Clamp(tempScale, 0.75f, 0.9f);
            firstSlot.transform.localScale = new Vector2(tempScale, tempScale);
            if (tempScale <= 0.75f)
            {
                isFirstNull = false;
            }
        }

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
