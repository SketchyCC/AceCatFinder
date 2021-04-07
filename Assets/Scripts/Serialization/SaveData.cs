using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    private static SaveData _current;
    public static SaveData current
    {
        get
        {
            if (_current == null)
            {
                _current = new SaveData();
            }
            return _current;
        }
        set
        {
            _current = value;
            Debug.Log("save data set");
        }
    }

    public PlayerData player;
    public float Money;
    public List<CatData> cats=new List<CatData>();
    public int DaysPassed;
    public float CurrentTime;
    public float EnergyBoostDuration;

    public List<PosterData> posterData;
    public List<PosterData> AcceptedPosterData;


    public Inventory inventoryData;
    public Inventory equipmentData;

    public bool[] ClothingUnlockData=new bool[7];
}
