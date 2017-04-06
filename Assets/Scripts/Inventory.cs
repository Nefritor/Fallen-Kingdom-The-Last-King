using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    ItemDatabase database;
    GameObject inventoryPanel, slotSimplePanel, inventoryQuickPanel, slotQuickPanel, selectedSlot, tempSelectedSlot;
    public GameObject inventorySimpleSlot, inventorySimpleItem, inventoryQuickSlot, inventoryQuickItem;

    int simpleSlotAmount, quickSlotAmount, idItem, tempIdItem;
    public List<Item> items = new List<Item>();
    public List<GameObject> simpleSlots = new List<GameObject>();
    public List<GameObject> quickSlots = new List<GameObject>();
    float scale, tempScale;
    bool isScalling;

    private void Start()
    {
        isScalling = false;
        tempIdItem = 0;
        idItem = 0;
        database = GetComponent<ItemDatabase>();
        simpleSlotAmount = 10;
        quickSlotAmount = 2;
        inventoryPanel = GameObject.Find("Inventory Panel");
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
        AddItem(0);
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
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && !isScalling && idItem != items.Count - 1)
        {
            scale = 0.7f;
            tempScale = 0.9f;
            tempIdItem = idItem;
            idItem += 1;
            isScalling = true;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && !isScalling && idItem != 0)
        {
            scale = 0.7f;
            tempScale = 0.9f;
            tempIdItem = idItem;
            idItem -= 1;
            isScalling = true;
        }
        idItem = Mathf.Clamp(idItem, 0, items.Count - 1);
        Debug.Log(isScalling);
        
        GameObject selectedSlot = slotSimplePanel.transform.GetChild(idItem).gameObject;
        GameObject tempSelectedSlot = slotSimplePanel.transform.GetChild(tempIdItem).gameObject;

        if (isScalling)
        {
            scale += -(Mathf.Pow(scale - 0.825f, 2) - 0.01f) * 2;
            scale = Mathf.Clamp(scale, 0.75f, 0.9f);
            selectedSlot.transform.localScale = new Vector2(scale, scale);

            tempScale -= -(Mathf.Pow(tempScale - 0.825f, 2) - 0.01f) * 2;
            tempScale = Mathf.Clamp(tempScale, 0.75f, 0.9f);
            tempSelectedSlot.transform.localScale = new Vector2(tempScale, tempScale);
            if (scale >= 0.9f && tempScale <= 0.75f)
            {
                isScalling = false;
            }
        }
    }
}
