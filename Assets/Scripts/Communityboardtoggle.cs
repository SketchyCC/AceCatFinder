using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using GameEvents;
using System;

public class Communityboardtoggle : MonoBehaviour
{
    public GameObject UIpopup;
    public GameObject Boardcam;
    public GameObject Playercam;
    public GameObject PlayerObject; //haha a+ coding practice orz
    private bool looking = false;

    bool PlayerinRange=false;

    private void Start()
    {
        UIpopup.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<CommunityBoardLeave>(OnLeavingUpdate);
    }


    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<CommunityBoardLeave>(OnLeavingUpdate);
    }

    private void OnLeavingUpdate(CommunityBoardLeave e)
    {
        Invoke("Delay", 1.0f);
    }
    private void Delay()
    {
        looking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerinRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UIpopup.SetActive(false);
        PlayerinRange = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (PlayerinRange && !looking)
        {
            UIpopup.SetActive(true);

            if (Input.GetKeyUp(KeyCode.Q))
            {
                Playercam.SetActive(false);
                Boardcam.SetActive(true);
                UIpopup.SetActive(false);
                GameEventManager.Raise(new CommunityBoardLook());
                looking = true;
            }
        }

    }
}
