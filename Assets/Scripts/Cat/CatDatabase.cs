using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

[CreateAssetMenu(fileName = "New Cat Database", menuName = "Cat Database")]
public class CatDatabase : ScriptableObject,ISerializationCallbackReceiver
{
    //public CatFSM[] CatObjects;
    public Cat[] CatObjects;

    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < CatObjects.Length; i++)
        {
            if (CatObjects[i].CatId != i)
            CatObjects[i].CatId = i;
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
