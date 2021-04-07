using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using Cinemachine;
using BehaviorDesigner.Runtime;
using UnityEngine.Events;


public class WrongCatCaught : MonoBehaviour
{

    // public GameObject[] catcams;
   
    public CinemachineVirtualCamera virtualcam;

    public UnityEvent shock;    

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<WrongCatEvent>(OnWrongCatEvent);
    }
    protected virtual void OnDisable()   
    {
        GameEventManager.RemoveListener<WrongCatEvent>(OnWrongCatEvent);
    }

    public virtual void OnWrongCatEvent(WrongCatEvent e)
    {
        virtualcam. Invoke("shockevent", 1 );
        GameEventManager.Raise(new UIOpened(false, gameObject));
    }

    private void shockevent()
    {
        shock.Invoke();
    }

}
