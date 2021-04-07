using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;
using System;

public class UIDisplay : MonoBehaviour
{
    //for all text to be displayed in the ui
    public Text MoneyDisplay;
    public GameObject CollectedSlots;
    public GameObject RButton;
    public TimeManager time;
    public Text Clock;
    public Text Weekday;
    public Text CurrentDay;
    public Text AMorPM;
    private float currentTime;

    private void Start()
    {
        MoneyDisplay.text = GameManager.MoneyInPurse.ToString() + "€"; //switched the currency after the number, as we usually write it like that in the EU
        currentTime = time.TimeOfDay;
        TimeUpdater(currentTime - (currentTime % 0.25f));
    }

    private void FixedUpdate()
    {
        currentTime = time.TimeOfDay;
        if (currentTime % 0.25f <= 0.01f) //this line makes the clock only update every 15 ungame minutes
        {
            TimeUpdater(currentTime);
        }
    }

    public void TimeUpdater(float time)
    {
        if (time <= 13)//AM
        {
            Clock.text = TimeSpan.FromMinutes(time).ToString(@"mm\:ss");
            AMorPM.text = "AM";
        }
        else//PM
        {
            Clock.text = TimeSpan.FromMinutes(time - 12f).ToString(@"mm\:ss");
            AMorPM.text = "PM";
        }
    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<MoneyUpdate>(OnMoneyUpdate);
        GameEventManager.AddListener<PosterAcceptedEvent>(OnPosterUpdate);
        GameEventManager.AddListener<DayPassedEvent>(OnDayPassed);
        GameEventManager.AddListener<CommunityBoardLook>(OnLookUpdate);
        GameEventManager.AddListener<CommunityBoardLeave>(OnLeaveUpdate);
    }


    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<MoneyUpdate>(OnMoneyUpdate);
        GameEventManager.RemoveListener<PosterAcceptedEvent>(OnPosterUpdate);
        GameEventManager.RemoveListener<DayPassedEvent>(OnDayPassed);
        GameEventManager.RemoveListener<CommunityBoardLook>(OnLookUpdate);
        GameEventManager.RemoveListener<CommunityBoardLeave>(OnLeaveUpdate);
    }

    private void OnLookUpdate(CommunityBoardLook e)
    {
        GetComponent<Canvas>().enabled = false;
    }
    private void OnLeaveUpdate(CommunityBoardLeave e)
    {
        GetComponent<Canvas>().enabled = true;
    }
    
    private void OnDayPassed(DayPassedEvent e)
    {
        int day = 1 + e.NumberofDays;
        switch (day%7)
        {
            case 0:
                Weekday.text = "Friday";
                break;
            case 1:
                Weekday.text = "Saturday";
                break;
            case 2:
                Weekday.text = "Sunday";
                break;
            case 3:
                Weekday.text = "Monday";
                break;
            case 4:
                Weekday.text = "Tuesday";
                break;
            case 5:
                Weekday.text = "Wednesday";
                break;
            case 6:
                Weekday.text = "Thursday";
                break;
            default:
                Weekday.text = "??"; //the forbidden weekday
                break;                               
        }

        CurrentDay.text = "Day " + day.ToString();
    }

    public virtual void OnMoneyUpdate(MoneyUpdate e)
    {
        MoneyDisplay.text = GameManager.MoneyInPurse.ToString() + "€";
    }

    public virtual void OnPosterUpdate(PosterAcceptedEvent e)
    {
        //takes number of all free slots and searches for the first slot that has no child, in that we want to move the poster
        bool noSpace = true;
        RButton.SetActive(true);
        int children = CollectedSlots.transform.childCount;

        for (int i =0; i < children; i++)
        {
            if(CollectedSlots.transform.GetChild(i).transform.childCount <= 0)
            {                
                e.PosterEntity.transform.SetParent(CollectedSlots.transform.GetChild(i).transform, false);
                noSpace = false;
                break;
            }
        }
        if (noSpace)
        {
            Debug.Log("Complete the other missions first"); //this message should be shown ingame too
        }
    }
}
