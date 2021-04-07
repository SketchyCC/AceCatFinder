using System.Collections.Generic;
using UnityEngine;
using GameEvents;

[ExecuteAlways] //remove later
public class TimeManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    [SerializeField, Range(0, 24)] public float TimeOfDay;
    [SerializeField, Range(0, 24)] private float standOfSkybox;
    [SerializeField] public float MinutesPerDay; 
    [SerializeField] private float TimeFactor; //x seconds per 'hour'

    private List<PosterTimer> PosterTimers=new List<PosterTimer>();
    private int PosterCount=0;

    public GameObject ParkLights;
    public GameObject StreetLights;

    // for sleeptime
    public GameObject gedtimeui;
    bool sleeping = false;
    bool cansleep = false;


    private float NewDay=7f;
    private float StartOfDayOnStart = 7f;
    private float EndOfDay=24f;
    private float StartofDay = 4;
    private float startOfNight = 18f;

    private float startOfSkyboxday=6;
    private float endofskyboxDay=16;
    //blend skybox to night
    private float startOfSkyboxnight = 18;
    private float endofskyboxnight = 4; 
    //blend skybox to day

    private int DaysPassed;
    private float TimeAddition;

    //default is 1 second for each hour
    //about 7 minutes per day, 420 seconds, 420/24 seconds per hour
    private void Awake()
    {

        if (MinutesPerDay == 0)
        {
            MinutesPerDay = 1f; //if not set to other values, default to 1 minute day
        }
        TimeFactor = (MinutesPerDay * 60f) / 24;
        TimeOfDay = StartOfDayOnStart;

        TimeAddition = Time.fixedDeltaTime / TimeFactor;
        Debug.Log("Time manager Time Addition setting " + Time.deltaTime + " divided by " + TimeFactor);
        Debug.Log("Time Addition set to " + TimeAddition);

        if (GameManager.IsNewGame)
        {
            RenderSettings.skybox.SetFloat("_Blend", 0);
            DaysPassed = 0;
        }
        SaveData.current.DaysPassed = DaysPassed;
        SaveData.current.CurrentTime = TimeOfDay;
    }


    private void FixedUpdate()
    {
        if (Preset == null) {
            return; }
        if (Time.timeScale == 0f) {
            return; } //check if the game is paused
        if (Application.isPlaying)
        {
            if (TimeOfDay>= startOfNight)
            {
                if (TimeOfDay <= startOfNight + 1)
                {
                    ParkLights.SetActive(true);
                    StreetLights.SetActive(true);

                    cansleep = true;
                }
            }

            TimeOfDay += TimeAddition;
            if(TimeOfDay >= EndOfDay && !sleeping && cansleep) 
            {
                Sleeptime();
            }

            if (TimeOfDay > NewDay & TimeOfDay < EndOfDay - 0.1 )
            {
                 sleeping = false;         
                gedtimeui.SetActive(false);
            }


            if(TimeOfDay >= StartofDay)
            {
                if (TimeOfDay < StartofDay+1) 
                {
                    ParkLights.SetActive(false);
                    StreetLights.SetActive(false);
                }
            }
            TimeOfDay %= 24;

            UpdateLighting(TimeOfDay/24f);
            UpdateSkyboox(TimeOfDay);
        }


        // poster failed timecheck
        for (int i = 0; i < PosterCount; i++)   
        {
            PosterTimers[i].TimePassed += TimeAddition;
            if (PosterTimers[i].TimePassed >= PosterTimers[i].TimeLimit)
            {
                GameEventManager.Raise(new PosterFailedEvent(PosterTimers[i].Poster));
            }
        }
        SaveData.current.CurrentTime = TimeOfDay;
        SaveData.current.DaysPassed = DaysPassed;
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);
      //  RenderSettings.skybox.SetFloat("_Blend", timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }
    private void UpdateSkyboox(float dimeOfday) //  
    {// --->  startOfSkyboxday==6, endofskyboxDay==16, ----sunset--->  startOfSkyboxnight = 18; endofskyboxnight = 4; ----sunrise

        if (dimeOfday >= endofskyboxDay ) // 16 // blend to night
        {
            if(dimeOfday <= startOfSkyboxnight) // 18
            {
                RenderSettings.skybox.SetFloat("_Blend", dimeOfday / 2 - 8);
            }
        }

        if (dimeOfday >= endofskyboxnight) // 4 // blend to day
        {
            if (dimeOfday <= startOfSkyboxday) // 6
            {
                RenderSettings.skybox.SetFloat("_Blend", dimeOfday /(- 2) + 3);
            }
        }

    }

    private void Sleeptime()
    {
        float smoothfade = 1;
        gedtimeui.SetActive(false);
        gedtimeui.SetActive(true);

        EndDay();
        PauseTime();
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        while (smoothfade > 0)
        {
            Time.timeScale = Mathf.Lerp(0, 1, smoothfade);
            smoothfade -= Time.deltaTime;
        }
        sleeping = true;
    }

    public void wakeupbutton()
    {
        ResumeTime();
        Time.timeScale = 1;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }

    public void EndDay()
    {
        DaysPassed += 1;
        SaveData.current.DaysPassed = DaysPassed;
        GameEventManager.Raise(new DayPassedEvent(DaysPassed)); //currently unused event
        TimeOfDay = NewDay;
        ParkLights.SetActive(false);
        StreetLights.SetActive(false);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        Debug.Log(DaysPassed + " day(s) passed");
    }

    public void PauseTime()
    {
        GameEventManager.Raise(new TimeStoppedEvent(true));
        TimeAddition = 0;
    }

    public void ResumeTime()
    {
        GameEventManager.Raise(new TimeStoppedEvent(false));
        TimeAddition = Time.fixedDeltaTime / TimeFactor;
    }

    protected virtual void OnEnable() 
    {
        GameEventManager.AddListener<PosterAcceptedEvent>(OnPosterAccepted);
        GameEventManager.AddListener<CommunityBoardLook>(OnBoardLook);
        GameEventManager.AddListener<CommunityBoardLeave>(OnBoardLeave);
        GameEventManager.AddListener<CatCaughtEvent>(OnCatCaughtEvent);
        GameEventManager.AddListener<WrongCatEvent>(OnWrongCatEvent);
        GameEventManager.AddListener<LoadEvent>(OnLoadEvent);
        GameEventManager.AddListener<UIOpened>(OnUIOpened);
    }

    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<PosterAcceptedEvent>(OnPosterAccepted);
        GameEventManager.RemoveListener<CommunityBoardLook>(OnBoardLook);
        GameEventManager.RemoveListener<CommunityBoardLeave>(OnBoardLeave);
        GameEventManager.RemoveListener<CatCaughtEvent>(OnCatCaughtEvent);
        GameEventManager.RemoveListener<WrongCatEvent>(OnWrongCatEvent);
        GameEventManager.RemoveListener<LoadEvent>(OnLoadEvent);
        GameEventManager.RemoveListener<UIOpened>(OnUIOpened);
    }

    public virtual void OnUIOpened(UIOpened e)
    {
        if (e.UIisopened == true)
        {
            PauseTime();
        }
        else if (e.UIisopened == false)
        {
            ResumeTime();
        }
    }

    public virtual void OnLoadEvent(LoadEvent e)
    {
        TimeOfDay = SaveData.current.CurrentTime;

        if (TimeOfDay >= startOfNight)
        {
            ParkLights.SetActive(true);
            StreetLights.SetActive(true);
        }
        if (TimeOfDay >= startOfSkyboxnight) // 18
        {
            RenderSettings.skybox.SetFloat("_Blend",   1);
        }

        DaysPassed = SaveData.current.DaysPassed;
        ResumeTime();

    }

    public virtual void OnBoardLook(CommunityBoardLook e)
    {
        PauseTime();
    }

    public virtual void OnBoardLeave(CommunityBoardLeave e)
    {
        ResumeTime();
    }

    public virtual void OnCatCaughtEvent(CatCaughtEvent e)
    {
        PauseTime();
    }

    public virtual void OnWrongCatEvent(WrongCatEvent e) 
    {
        ResumeTime();
    }

    public virtual void OnPosterAccepted(PosterAcceptedEvent e)
    {
        PosterTimer temp = new PosterTimer(e.Posterobject);
        temp.TimeLimit *= EndOfDay-NewDay; //so time limit is now 24 minutes
        PosterTimers.Add(temp);
        PosterCount++;
    }
}

public class PosterTimer
{
    public PosterObject Poster;
    public int PosterId;
    public float TimeLimit;
    public float TimePassed;
    public PosterTimer(PosterObject obj)
    {
        Poster = obj;
        PosterId = obj.CatPrefab.CatId;
        TimeLimit = obj.TimeLimit;//will be set into days 
    }
}
