using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Database", menuName = "Inventory System/Inventory Database")]
public class ItemDatabaseObject : ScriptableObject,ISerializationCallbackReceiver
{
    public ItemObject[] ItemsObjects;

    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < ItemsObjects.Length; i++)
        {
            if (ItemsObjects[i].data.Id != i)
                ItemsObjects[i].data.Id = i;
        }
    }

    public void OnAfterDeserialize()
    {
        UpdateID();
    }
    public void OnBeforeSerialize()
    {
    }
}
