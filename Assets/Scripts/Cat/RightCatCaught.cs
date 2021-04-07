using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using Cinemachine;
using BehaviorDesigner.Runtime;


public class RightCatCaught : MonoBehaviour
{
    public int CatId;
    //public ParticleSystem TestEffect;
    public GameObject CatFoundParticles;

    void Start()
    {
        CatFoundParticles.GetComponent<UIParticleSystem>().enabled = true;
        //CatFoundParticles.Play();

    }
    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<PosterCompletedEvent>(OnPosterCompleted);
    }

    protected virtual void OnDisable()   
    {
        GameEventManager.RemoveListener<PosterCompletedEvent>(OnPosterCompleted);
    }

    public virtual void OnPosterCompleted(PosterCompletedEvent e)
    {
        CatFoundParticles.GetComponent<UIParticleSystem>().enabled = true;        

        CatFoundParticles.GetComponent<ParticleSystem>().Stop();
        CatFoundParticles.GetComponent<ParticleSystem>().Play();
    }

}
