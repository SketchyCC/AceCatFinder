using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using Cinemachine;
using BehaviorDesigner.Runtime;
using UnityEngine.Events;


public class Meowwringcaught : MonoBehaviour
{
    public CinemachineVirtualCamera catcam;
    public AudioClip hiss;
    public AudioClip angrymeow;

    private AudioSource hissAudioSource1;   


    void Start()
    {
        hissAudioSource1 = gameObject.GetComponentInParent<AudioSource>();
        
    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<WrongCatEvent>(OnWrongCatEvent);
        GameEventManager.AddListener<CatCaughtEvent>(OnCatCaughtEvent);
       
    }
    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<WrongCatEvent>(OnWrongCatEvent); 
        GameEventManager.RemoveListener<CatCaughtEvent>(OnCatCaughtEvent);
    }

    public virtual void OnWrongCatEvent(WrongCatEvent e)
    {
        hissAudioSource1.PlayOneShot(hiss, 0.1F);

        hissAudioSource1.PlayOneShot(angrymeow, 0.2F);

        catcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 5;
        Invoke("Delay", 0.3f);
    }

    public virtual void OnCatCaughtEvent(CatCaughtEvent e)
    {
        catcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }

    public void Delay()
    {
        catcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }

}
