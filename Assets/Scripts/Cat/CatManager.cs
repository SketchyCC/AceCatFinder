using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class CatManager : MonoBehaviour
{
    public GameObject[] allSpawns;  //ParkSpawn 0, CitySpawn 1, RiverSpawn 2, LitterSpawn 3    
    public GameObject[] strayCats;
   

    private void Start()
    {
        for(int i = 0; i < strayCats.Length; i++)
        {            
            int randomSpawn = Random.Range(0, allSpawns.Length);
            GameObject spawnedCats = Instantiate(strayCats[i], allSpawns[randomSpawn].transform) as GameObject;
        }
    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<MissingPosterReleasedEvent>(OnPosterReleased);
    }

    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<MissingPosterReleasedEvent>(OnPosterReleased);
    }

    public virtual void OnPosterReleased(MissingPosterReleasedEvent e)
    {
        if (e.Posterobject.LocationHint=="Puma Park")
        {
            Transform spawn = allSpawns[0].transform;
            GameObject CatSpawn = Instantiate(e.Posterobject.CatPrefab.gameObject, spawn);
        }
         else if (e.Posterobject.LocationHint == "At the Cafe")
        {
            Transform spawn = allSpawns[1].transform;
            GameObject CatSpawn = Instantiate(e.Posterobject.CatPrefab.gameObject, spawn);
        }
        else if (e.Posterobject.LocationHint == "Clawfish River")
        {
            Transform spawn = allSpawns[2].transform;
            GameObject CatSpawn = Instantiate(e.Posterobject.CatPrefab.gameObject, spawn);
        }
        else if (e.Posterobject.LocationHint == "Litter Alley")
        {
            Transform spawn = allSpawns[3].transform;
            GameObject CatSpawn = Instantiate(e.Posterobject.CatPrefab.gameObject, spawn);
        }
        else if (e.Posterobject.LocationHint == "Down Clawmark Lane")
        {
            Transform spawn = allSpawns[4].transform;
            GameObject CatSpawn = Instantiate(e.Posterobject.CatPrefab.gameObject, spawn);
        }
        else if (e.Posterobject.LocationHint == "At Market Street")
        {
            Transform spawn = allSpawns[5].transform;
            GameObject CatSpawn = Instantiate(e.Posterobject.CatPrefab.gameObject, spawn);
        }
        else { //Fallback if the position isn't set
            Transform spawn = allSpawns[6].transform;
            GameObject CatSpawn = Instantiate(e.Posterobject.CatPrefab.gameObject, spawn);
        }
    }
}
