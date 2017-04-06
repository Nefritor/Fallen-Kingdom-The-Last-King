﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class ItemDatabase : MonoBehaviour
{
    private List<Item> database = new List<Item>();
    private JsonData itemData;

    private void Start()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));        
        ConstructItemDatabase();
    }

    public Item FetchItemByID(int id)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].ID == id)
            {
                return database[i];
            }
        }
        return null;
    }

    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            database.Add(new Item((int)itemData[i]["id"], itemData[i]["title"].ToString(),
                itemData[i]["type"].ToString(), (int)itemData[i]["level"],
                itemData[i]["description"].ToString(), itemData[i]["slug"].ToString()));
        }
    }        
}

public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public int Level { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }

    public Item(int id, string title, string type, int level, string description, string slug)
    {
        this.ID = id;
        this.Title = title;
        this.Type = type;
        this.Level = level;
        this.Description = description;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);
    }
    public Item()
    {
        this.ID = -1;
    }
}
