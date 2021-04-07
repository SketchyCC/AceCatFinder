using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using Cinemachine;

public class NetCatching : MonoBehaviour
{
    public float rotSpeed = 0.5f;
    bool holdingNet = false;
    public int CatId { get; set; }
    public float TimeSwing = 0.5f;
    
    public Transform catInFront;
    public GameObject playerCam;
    private bool lookingAtBoard = false;
    private bool bagIsOpen = true;
    private bool swingDelay = false;
    private bool changedCam = false;
    private Transform Player;
    private StareAtCat catDetector;
    public List<Transform> removedCats;

    Cat catscript;

    private bool actualCall = false;

    private void Start()
    {
        catDetector = gameObject.GetComponentInChildren<StareAtCat>();
        Player = gameObject.GetComponentInParent<MovementScript>().transform;
    }

    private void Update()
    {
        if (lookingAtBoard || bagIsOpen)
        { return; }
        else {
            if (holdingNet && catInFront != null)
            {
                LookAtCat();
            }
            if (Input.GetMouseButton(0))
            {
                holdingNet = true;
            }
            if (Input.GetMouseButtonUp(0) && !swingDelay)
            {
                StartCoroutine(SwingDelay());
                holdingNet = false;
                swingDelay = true;
                GameEventManager.Raise(new SwingEvent());                
            }
            if(changedCam && catInFront == null)
            {
                SwitchToPlayer();
            }
        }                
    }

    private void LookAtCat() 
    {
        //as long as the player holds down mouse button, the player shall rotate towards a cat in front of them        
        if (!removedCats.Contains(catInFront)) 
        {
            Vector3 iSeeCat = catInFront.position - Player.position;
            iSeeCat.y = 0.0f;
            float singleStep = rotSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(Player.forward, iSeeCat, singleStep, 0.0f);
            Player.rotation = Quaternion.LookRotation(newDirection);

            if (catInFront != null)
            {
                //Vector3 betterAngle = (Player.position + catInFront.transform.position) / 2;

                playerCam.GetComponent<CinemachineFreeLook>().m_Follow = GetComponent<Transform>();
                playerCam.GetComponent<CinemachineFreeLook>().m_LookAt = catInFront;
                changedCam = true;
            }
        }
        else
        {
            SwitchToPlayer();
        }
        
    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<CommunityBoardLook>(OnLookUpdate);
        GameEventManager.AddListener<CommunityBoardLeave>(OnLeaveUpdate);
        GameEventManager.AddListener<UIOpened>(OnUIUpdate);
        GameEventManager.AddListener<BagOpenEvent>(OnBagUpdate);
        GameEventManager.AddListener<WrongCatEvent>(OnCaughtEvent);
        GameEventManager.AddListener<RightCatEvent>(OnBCaughtEvent);
    }

    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<CommunityBoardLook>(OnLookUpdate);
        GameEventManager.RemoveListener<CommunityBoardLeave>(OnLeaveUpdate);
        GameEventManager.RemoveListener<UIOpened>(OnUIUpdate);
        GameEventManager.RemoveListener<BagOpenEvent>(OnBagUpdate);
        GameEventManager.RemoveListener<WrongCatEvent>(OnCaughtEvent);
        GameEventManager.RemoveListener<RightCatEvent>(OnBCaughtEvent);
    }

    private void OnBagUpdate(BagOpenEvent e)
    {
        if (!e.openBag)
        {
            StartCoroutine(BagDelay(false));
        }
        else
        {
            bagIsOpen = e.openBag;
        }
    }

    private void OnLeaveUpdate(CommunityBoardLeave e)
    {
        lookingAtBoard = false;
    }

    private void OnLookUpdate(CommunityBoardLook e)
    {
        lookingAtBoard = true;
    }
    private void OnUIUpdate(UIOpened e)
    {    
        if(e.UIOrigin.name == "AskCatPanel" && e.UIisopened)
        {
            StartCoroutine(LookDelay(e.UIisopened));
        }
        else { lookingAtBoard = e.UIisopened; }        
    }

    //For the not triggering leave and enter at the same time (board)
    IEnumerator LookDelay(bool wait) {
        yield return new WaitForSeconds(0.3f);
        lookingAtBoard = wait;
    }
    IEnumerator BagDelay(bool wait)
    {
        yield return new WaitForSeconds(0.95f);
        bagIsOpen = wait;
    }
    IEnumerator SwingDelay()
    {
        yield return new WaitForSeconds(TimeSwing);
        if(catscript != null && !removedCats.Contains(catscript.transform))
        {
            GameEventManager.Raise(new CatCaughtEvent(catscript.CatId));
        }
        else { swingDelay = false; }        
        catscript = null;
    }
    IEnumerator SwingDelay2()
    {
        yield return new WaitForSeconds(TimeSwing);
        swingDelay = false;
    }

    private void OnCaughtEvent(WrongCatEvent e)
    {
        SwitchToPlayer();
        StartCoroutine(SwingDelay2());
    }
    private void OnBCaughtEvent(RightCatEvent e)
    {
        removedCats.Add(catInFront); 
        SwitchToPlayer();
        actualCall = true;
        StartCoroutine(RemovedCat(e.CatObj));
        StartCoroutine(SwingDelay2());
    }

    IEnumerator RemovedCat(int CatToRemove) 
    {        
        if(actualCall)
        {
            actualCall = false;
            yield return new WaitForSeconds(2.5f);
            if (catInFront != null)
            {
                catDetector.ReadyToRemoveCat(CatToRemove);
                SwitchToPlayer();
            }
        }
    }

    void SwitchToPlayer()
    {
        playerCam.GetComponent<CinemachineFreeLook>().m_Follow = Player;
        playerCam.GetComponent<CinemachineFreeLook>().m_LookAt = Player;        
    }

    private void OnTriggerStay(Collider other)
    {
        if (lookingAtBoard || bagIsOpen)
        { return; }
        else { 
            if (other.gameObject.tag == "Cat")
            {
                catscript = other.GetComponentInParent<Cat>();                
            }
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cat" && catscript == other.GetComponentInParent<Cat>())
        {
            catscript = null;
        }
    }
}
