using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using System;
using Cinemachine;

public class Communityboard : MonoBehaviour
{
    public GameObject Boardcam;
    public GameObject Playercam;
    
    public GameObject[] BoardSlots;
    private int currentCat;

    public GameObject posterPrefab;
    public PosterObject[] posterSO;   
    private Transform camOrigin;
    private bool zoom = false;
    private bool looking = false;
    private int layerMask = 1 << 8;
    private Camera cam;

    float camwidth;
    float camoffset;

    private void Start()
    {
        cam = Camera.main;
        camOrigin = Boardcam.transform;
        camwidth = Boardcam.GetComponent<CinemachineFollowZoom>().m_Width;
        camoffset= Boardcam.GetComponent<CinemachineCameraOffset>().m_Offset.x;
        int length=posterSO.Length;
        for (int i = 0; i < length; i++)
        {
            if (posterSO[i].posterProgress == PosterProgress.Inactive|| posterSO[i].posterProgress == PosterProgress.Posted)
            {                
                SpawnPoster(posterPrefab, posterSO[i]);                 
                return;
            }
            else if (posterSO[i].posterProgress == PosterProgress.In_Progress)
            {
                posterPrefab.GetComponent<PosterDisplay>().poster = posterSO[i];
                GameObject poster = Instantiate(posterPrefab) as GameObject; //requires a clone of a poster gameobject
                GameEventManager.Raise(new MissingPosterReleasedEvent(poster.GetComponent<PosterDisplay>().poster)); //unfortunately this event partially handles cat spawning
                GameEventManager.Raise(new PosterAcceptedEvent(posterSO[i], poster)); 
                //GameObject cat = Instantiate(posterSO[i].CatPrefab.gameObject) as GameObject; //instantiate cat for in progress posters

                return;
            } 
        }
    }

    private void Update()
    {
        if (looking)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !zoom)
            {
                Leaveboard();
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            for (int i = 0; i < posterSO.Length; i++)
            {
                posterSO[i].posterProgress = PosterProgress.Inactive;
            }
        }
    }

    public void SpawnPoster(GameObject PosterPrefab, PosterObject posterObj)
    {
        for (int i = 0; i < BoardSlots.Length; i++)
        {
            if (BoardSlots[i].transform.childCount <= 0) 
            {
                PosterPrefab.GetComponent<PosterDisplay>().poster = posterObj;
                GameObject poster=Instantiate(PosterPrefab) as GameObject;
                posterObj.posterProgress = PosterProgress.Posted;
                poster.transform.SetParent(BoardSlots[i].transform,false);
                GameEventManager.Raise(new MissingPosterReleasedEvent(poster.GetComponent<PosterDisplay>().poster));
                break;
            }
        }
    }
    public void Leaveboard()
    {
        GameEventManager.Raise(new CommunityBoardLeave());
        GameEventManager.Raise(new GenericButtonPressedEvent());
        Playercam.SetActive(true);
        Boardcam.SetActive(false);
        looking = false;
        cam.cullingMask = ~0;
    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<CommunityBoardLook>(OnLookingUpdate);
        GameEventManager.AddListener<DayPassedEvent>(OnDayPassed);
        GameEventManager.AddListener<PosterLookingEvent>(OnPosterUpdate);
        GameEventManager.AddListener<PosterAcceptedEvent>(OnAcceptedUpdate);
        GameEventManager.AddListener<PosterCompletedEvent>(OnCompletedEvent);
    }

    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<CommunityBoardLook>(OnLookingUpdate);
        GameEventManager.RemoveListener<DayPassedEvent>(OnDayPassed);
        GameEventManager.RemoveListener<PosterLookingEvent>(OnPosterUpdate);
        GameEventManager.RemoveListener<PosterAcceptedEvent>(OnAcceptedUpdate);
        GameEventManager.RemoveListener<PosterCompletedEvent>(OnCompletedEvent);
    }

    private void OnLookingUpdate(CommunityBoardLook e)
    {
        looking = true;
        cam.cullingMask = ~layerMask;
    }

    private void OnCompletedEvent(PosterCompletedEvent e)
    {
        currentCat++;
        if (currentCat >= posterSO.Length)return;  
        
        if (posterSO[currentCat].posterProgress == PosterProgress.Inactive)
        {
            SpawnPoster(posterPrefab, posterSO[currentCat]);
            
            return;
        }
        else
        {
            OnCompletedEvent(e);
        }
    }

    public virtual void OnDayPassed(DayPassedEvent e)
    {
        //SpawnPoster(posterPrefab);

        //switch (e.NumberofDays)
        //{
        //    case 2:
                
        //        SpawnPoster(posterPrefab[2]);
        //        break;
        //    case 5:
        //        SpawnPoster(posterPrefab[4]);
        //        break; //and etc
        //}

        //unused code to spawn posters on set day.
    }

    private void OnPosterUpdate(PosterLookingEvent e)
    {
        if (zoom) //when poster is declined
        {
            Boardcam.GetComponent<CinemachineVirtualCamera>().m_LookAt = gameObject.transform;
            Boardcam.GetComponent<CinemachineFollowZoom>().m_Width = camwidth;
            Boardcam.GetComponent<CinemachineCameraOffset>().m_Offset.x -= camoffset;
            Boardcam.GetComponent<CinemachineCameraOffset>().m_Offset.x -= 0.5f;
            zoom = false;
        }
        else if (!zoom)
        {            
            Boardcam.GetComponent<CinemachineVirtualCamera>().m_LookAt = e.posterSlot.transform;
            Boardcam.GetComponent<CinemachineFollowZoom>().m_Width -= 12.5f;
            Boardcam.GetComponent < CinemachineCameraOffset>().m_Offset.x += 0.5f;
            zoom = true;
        }                    
    }

    private void OnAcceptedUpdate(PosterAcceptedEvent e)
    {
        Boardcam.GetComponent<CinemachineVirtualCamera>().m_LookAt = gameObject.transform;
        Boardcam.GetComponent<CinemachineFollowZoom>().m_Width = camwidth;
        Boardcam.GetComponent<CinemachineCameraOffset>().m_Offset.x -= 0.5f;
        Boardcam.GetComponent<CinemachineCameraOffset>().m_Offset.x -= camoffset;
        zoom = false;
    }


}
