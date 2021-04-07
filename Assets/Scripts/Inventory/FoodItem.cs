using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Food Item", menuName ="Inventory System/Items/Food")]
public class FoodItem : ItemObject
{
    public void Awake()
    {
        type = ItemType.Food;
    }
}
