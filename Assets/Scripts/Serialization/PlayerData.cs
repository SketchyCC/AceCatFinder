using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Vector3 position;
    public Quaternion rotation;
    bool isWalking;
    public float BoostCounter;
    public float TimeElapsed;
}

[System.Serializable]
public class PlayerInventoryData
{
    public int Id;
    public float Price;
    public int amount;
    public PlayerInventoryData(int _id, float _price, int _amount)
    {
        Id = _id;
        Price = _price;
        amount = _amount;
    }
}