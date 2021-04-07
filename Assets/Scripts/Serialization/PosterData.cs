using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PosterData
{
    public int Id;
    public int PosterProgressenum;
    public bool Unlocked;

    public PosterData(int _Id, int _progress, bool _Unlocked)
    {
        Id = _Id;
        PosterProgressenum = _progress;
        Unlocked = _Unlocked;
    }
}
