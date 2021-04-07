using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class ShopPanel : MonoBehaviour
{
    public GameObject AskPanel;
    public GameObject ShopUI;
    bool looking = false;
    public GameObject shopCam;
    public GameObject playerCam;
    public GameObject playerObj;
    public Transform standHere;


    private void Start()
    {
        ShopUI.SetActive(false);
        AskPanel.SetActive(false);
    }
    private void Update()
    {
        if (looking)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                LeaveShop();
            }
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AskPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
            AskPanel.SetActive(false);        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (ShopUI.activeSelf == false)
                {
                    ShopUI.SetActive(true);
                    StartCoroutine(Delay(1f));
                    AskPanel.SetActive(false);
                    GameEventManager.Raise(new UIOpened(ShopUI.activeSelf, gameObject));

                    shopCam.SetActive(true);
                    playerCam.SetActive(false);                   
                }
            }
        }           
    }

    public void LeaveShop()
    {
        playerCam.SetActive(true);
        shopCam.SetActive(false);
        ShopUI.SetActive(false);
        looking = false;
        GameEventManager.Raise(new UIOpened(ShopUI.activeSelf, gameObject));
    }

    IEnumerator Delay( float counter)
    {
        float timeElapsed = 0;
        Vector3 plPos = playerObj.transform.position;
        Quaternion plRot = playerObj.transform.rotation;

        while (timeElapsed < counter)
        {
            float percentage = (timeElapsed / (counter / 100)) / 100;
            playerObj.transform.position = Vector3.Lerp(plPos, standHere.position, percentage);
            playerObj.transform.rotation = Quaternion.Lerp(plRot, standHere.rotation, percentage);
            timeElapsed += Time.deltaTime;
            yield return null;
        }        
        looking = true;
    }
}
