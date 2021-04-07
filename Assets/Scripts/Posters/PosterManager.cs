using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class PosterManager : MonoBehaviour
{
    int MaxCatPoster = 3;
    List<PosterObject> AcceptedPosters = new List<PosterObject>();

    public GameObject[] SpawnPoints;

    private void Start()
    {
        SaveData.current.AcceptedPosterData = new List<PosterData>();
    }
    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<PosterAcceptedEvent>(OnPosterAccepted);
        GameEventManager.AddListener<RightCatEvent>(OnRightCatEvent);
    }

    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<PosterAcceptedEvent>(OnPosterAccepted);
        GameEventManager.RemoveListener<RightCatEvent>(OnRightCatEvent);
    }

    public virtual void OnPosterAccepted(PosterAcceptedEvent e)
    {
        Debug.Log(AcceptedPosters.Count);
        if (AcceptedPosters.Count < MaxCatPoster)
        {
            AcceptedPosters.Add(e.Posterobject); //adds poster to player's roster if they don't already have 3 posters
            PosterData temp = new PosterData(e.Posterobject.CatPrefab.CatId, (int)e.Posterobject.posterProgress, e.Posterobject.IsUnlocked);
            Debug.Log(SaveData.current.AcceptedPosterData);
            SaveData.current.AcceptedPosterData.Add(temp);
            Debug.Log("Poster Accepted");
        }
        Debug.Log(AcceptedPosters.Count);
        //GameObject Catspawn = Instantiate(e.Posterobject.CatPrefab.gameObject); //player is deactivated during cat's spawning so this will be done when poster spawns. 
    }

    public virtual void OnRightCatEvent(RightCatEvent e)
    {
        if (AcceptedPosters != null)
        {
            if (AcceptedPosters.Count > 0)
            {
                for (int i = 0; i < AcceptedPosters.Count; i++)
                {
                    Debug.Log(AcceptedPosters[i].CatPrefab.CatId);
                    if (AcceptedPosters[i].CatPrefab.CatId == e.CatObj)
                    {
                        GameEventManager.Raise(new PosterCompletedEvent(AcceptedPosters[i]));
                        GameManager.AddMoney(AcceptedPosters[i].MoneyAward);
                        GameEventManager.Raise(new MoneyUpdate());
                        AcceptedPosters[i].posterProgress = PosterProgress.Complete;
                        AcceptedPosters.Remove(AcceptedPosters[i]);
                        for (int j = 0; j < SaveData.current.AcceptedPosterData.Count; j++)
                        {
                            if (SaveData.current.AcceptedPosterData[j].Id == e.CatObj)
                            {
                                SaveData.current.AcceptedPosterData.Remove(SaveData.current.AcceptedPosterData[j]);
                                break;

                            }
                        }
                        Debug.Log(AcceptedPosters.Count);

                        return;
                    }
                    else
                    {
                        GameEventManager.Raise(new WrongCatEvent(e.CatObj,true));
                        GameEventManager.Raise(new UIOpened(false, gameObject));
                    }
                }

            }
            else
            {
                GameEventManager.Raise(new WrongCatEvent(e.CatObj,true));
                GameEventManager.Raise(new UIOpened(false, gameObject));
            }
        }
    }
}

