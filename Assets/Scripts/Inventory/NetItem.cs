using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Net Item", menuName = "Inventory System/Items/Net")]
public class NetItem : ItemObject
{
    public void Awake()
    {
        type = ItemType.Net;
    }
}
