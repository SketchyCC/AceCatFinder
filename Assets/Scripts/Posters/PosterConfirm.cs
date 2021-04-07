using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using System;

public class PosterConfirm : MonoBehaviour
{
    public GameObject Askpanel;
    private GameObject poster;

    void Start()
    {
        Askpanel.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<PosterLookingEvent>(OnPosterUpdate);
    }
    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<PosterLookingEvent>(OnPosterUpdate);
    }

    private void OnPosterUpdate(PosterLookingEvent e)
    {
        Askpanel.SetActive(true);
        poster = e.posterSlot;
    }

    public void Answer()
    {                
        Askpanel.SetActive(false);
    }
}