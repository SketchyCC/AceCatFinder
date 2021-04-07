using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trap Item", menuName = "Inventory System/Items/Trap")]
public class TrapItem : ItemObject
{
    public void Awake()
    {
        type = ItemType.Trap;
    }
}
