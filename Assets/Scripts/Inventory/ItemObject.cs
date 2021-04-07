using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Net,
    Food,
    Trap,
    Consumable    
}

public class ItemObject:ScriptableObject
{
    public Sprite uiDisplay;
    public GameObject characterDisplay;
    public bool stackable;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public Item data = new Item();
    public float price;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }

}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id=-1;
    public float Price;
    public Item()
    {
        Name = "";
        Id = -1;
        Price = 0;
    }
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.data.Id;
        Price = item.price;
    }
}