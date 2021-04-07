using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool IsNewGame; 


    public static int CatFoundCount = 0;
    public static float MoneyInPurse=100f;
    public PosterObject[] SetPosters;

    List<PosterData> posterData;

    private void Awake()
    {
        Debug.Log("On awake called");
        int length = SetPosters.Length;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (IsNewGame)
            {
                MoneyInPurse = 0f;
                SaveData.current.posterData = new List<PosterData>();
                for (int i = 0; i < length; i++)
                {
                    SetPosters[i].posterProgress = PosterProgress.Inactive;
                    PosterData temp = new PosterData(SetPosters[i].CatPrefab.CatId, (int)SetPosters[i].posterProgress, SetPosters[i].IsUnlocked); //gunther gives an error :(
                    SaveData.current.posterData.Add(temp);
                }
            }
            else if (!IsNewGame)
            {
                for (int i = 0; i < length; i++)
                {
                    SetInGamePosterData(SetPosters[i]);

                }
                MoneyInPurse = SaveData.current.Money;
            }
            posterData = SaveData.current.posterData;
            SaveData.current.Money = MoneyInPurse;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("in active scene 0");
            if (SaveData.current.posterData != null) //load from save file while in menu
            {
                Debug.Log("poster data is not null");
                for (int i = 0; i < length; i++)
                {
                    SetInGamePosterData(SetPosters[i]);
                    Debug.Log("setting poster data");
                }
            }

        }
    }


    public virtual void OnPosterReleased(MissingPosterReleasedEvent e)
    {
        SetPosterData(e.Posterobject);
    }

    public virtual void OnPosterAccepted(PosterAcceptedEvent e)
    {
        SetPosterData(e.Posterobject);
    }

    public virtual void OnPosterCompleted(PosterCompletedEvent e)
    {
        CatFoundCount += 1;
        int length = SetPosters.Length;
        for (int i = 0; i < length; i++)
        {
            if (SetPosters[i] == e.Posterobject)
            {
                SetPosters[i].posterProgress = PosterProgress.Complete;
                SetPosters[i].IsUnlocked = true;
                break;
            }
        }
        SetPosterData(e.Posterobject);
    }

    public virtual void OnPosterFailed(PosterFailedEvent e)
    {
        int length = SetPosters.Length;
        for (int i = 0; i < length; i++)
        {
            if (SetPosters[i] == e.Posterobject)
            {
                SetPosters[i].posterProgress = PosterProgress.Failed;
                break;
            }
        }
        SetPosterData(e.Posterobject);
    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<MissingPosterReleasedEvent>(OnPosterReleased);
        GameEventManager.AddListener<PosterAcceptedEvent>(OnPosterAccepted);
        GameEventManager.AddListener<PosterCompletedEvent>(OnPosterCompleted);
        GameEventManager.AddListener<PosterFailedEvent>(OnPosterFailed);
        GameEventManager.AddListener<LoadEvent>(OnLoadEvent);
    }
    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<MissingPosterReleasedEvent>(OnPosterReleased);
        GameEventManager.RemoveListener<PosterAcceptedEvent>(OnPosterAccepted);
        GameEventManager.RemoveListener<PosterCompletedEvent>(OnPosterCompleted);
        GameEventManager.RemoveListener<PosterFailedEvent>(OnPosterFailed);
        GameEventManager.RemoveListener<LoadEvent>(OnLoadEvent);
    }

    void OnLoadEvent(LoadEvent e)
    {
        MoneyInPurse = SaveData.current.Money;
        posterData = SaveData.current.posterData;
        int length = SaveData.current.posterData.Count;
        for (int i = 0; i < length; i++)
        {
            SetPosters[i].posterProgress = (PosterProgress)posterData[i].PosterProgressenum;
            SetPosters[i].IsUnlocked = posterData[i].Unlocked;
        }
    }

    public static void AddMoney(float MoneytoAdd)
    {
        MoneyInPurse += MoneytoAdd;
        SaveData.current.Money = MoneyInPurse;
        GameEventManager.Raise(new MoneyUpdate());
    }

    void SetPosterData(PosterObject obj) //setting save data for posters
    {
        if (SaveData.current.posterData != null)
        {
            posterData = SaveData.current.posterData;
            int length = SaveData.current.posterData.Count;
            for (int i = 0; i < length; i++)
            {
                if (obj.CatPrefab.CatId == SaveData.current.posterData[i].Id)
                {
                    posterData[i].PosterProgressenum = (int)obj.posterProgress;
                    posterData[i].Unlocked = obj.IsUnlocked;
                    break;
                }
            }
        }

    }

    void SetInGamePosterData(PosterObject obj) //loading from save data
    {
        if (SaveData.current.posterData !=null)
        {
            int length = SaveData.current.posterData.Count;
            for (int i = 0; i < length; i++)
            {
                if (obj.CatPrefab.CatId == SaveData.current.posterData[i].Id)
                {
                    obj.posterProgress = (PosterProgress)SaveData.current.posterData[i].PosterProgressenum;
                    obj.IsUnlocked = SaveData.current.posterData[i].Unlocked;
                    break;
                }
            }
        }

    }
}
