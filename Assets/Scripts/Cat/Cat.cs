using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using Cinemachine;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class Cat : MonoBehaviour
{
    public GameObject CatCamera;
    public int CatId;
    private int copycatId;
    public BehaviorTree CatBehaviour;
    public Animator catanim;
    public NavMeshAgent navmesh;
    float speed;
    float defaultspeed;
        
    private int WalkSpeedHash;
    private int AlertHash;
    private int EatingHash;

    public CatData catData=new CatData();

    private void Start()
    {
        if (string.IsNullOrEmpty(catData.id))
        {
            catData.id = CatId.ToString();
            SaveData.current.cats.Add(catData);
        }

        CinemachineVirtualCamera catcam = GetComponentInChildren<CinemachineVirtualCamera>();
        CatBehaviour = GetComponentInChildren<BehaviorTree>();
        navmesh = GetComponentInChildren<NavMeshAgent>();
        catanim = GetComponentInChildren<Animator>();
        CatCamera = catcam.gameObject;
        CatCamera.SetActive(false);

        defaultspeed = navmesh.speed;

        WalkSpeedHash = Animator.StringToHash("WalkSpeed");
        AlertHash = Animator.StringToHash("Alert");
        EatingHash = Animator.StringToHash("Eating");
    }

    private void Update()
    {
        catData.position = transform.position;
        catData.rotation = transform.rotation;
        var alert = (SharedBool)CatBehaviour.GetVariable("IsAlert");
        var eating = (SharedBool)CatBehaviour.GetVariable("IsEating");
        speed = navmesh.velocity.magnitude;
        catanim.SetFloat(WalkSpeedHash, speed / defaultspeed);
        catanim.SetBool(AlertHash, alert.Value);
        catanim.SetBool(EatingHash, eating.Value);
    }

    private void OnAnimatorMove()
    {
        transform.position = navmesh.nextPosition;
    }

    protected virtual void OnEnable()
    {

        GameEventManager.AddListener<CatCaughtEvent>(OnCatCaughtEvent);
        GameEventManager.AddListener<WrongCatEvent>(OnWrongCatEvent);
        GameEventManager.AddListener<RightCatEvent>(OnRightCatEvent);
        GameEventManager.AddListener<PosterCompletedEvent>(OnPosterCompleted);
        GameEventManager.AddListener<LoadEvent>(OnLoadEvent);
    }

    protected virtual void OnDisable()
    {

        GameEventManager.RemoveListener<CatCaughtEvent>(OnCatCaughtEvent);
        GameEventManager.RemoveListener<WrongCatEvent>(OnWrongCatEvent);
        GameEventManager.RemoveListener<RightCatEvent>(OnRightCatEvent);
        GameEventManager.RemoveListener<PosterCompletedEvent>(OnPosterCompleted);
        GameEventManager.RemoveListener<LoadEvent>(OnLoadEvent);
    }


    public virtual void OnCatCaughtEvent(CatCaughtEvent e)
    {
        if (e.CatObj == CatId)
        {
            copycatId = e.CatObj;
            CatCamera.SetActive(true);
            CatBehaviour.SetVariableValue("IsTrapped", true);
        }        
    }

    public virtual void OnWrongCatEvent(WrongCatEvent e)
    {
        if (e.CatObj == CatId)
        {
            CatBehaviour.SendEvent("WrongCat");
        }
          Invoke("Delay", 0.5f);
    }
    public virtual void OnRightCatEvent(RightCatEvent e)
    {
        Invoke("Delay", 1f);
    }

    public void Delay()
    {
        CatCamera.SetActive(false);
    }

    public void Delay2()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnPosterCompleted(PosterCompletedEvent e)
    {
        if (e.Posterobject.CatPrefab.CatId == CatId)
        {
            if (copycatId == CatId)
            {               
                Invoke("Delay2", 3f);                
            }
        }
    }

    void OnLoadEvent(LoadEvent e)
    {
        int length = SaveData.current.cats.Count;
        for (int i = 0; i < length; i++)
        {
            if (SaveData.current.cats[i].id == catData.id)
            {
                catData = SaveData.current.cats[i]; //edit so when scene is loaded it checks cat ids and moves them to their positions
                gameObject.transform.position = catData.position;
                gameObject.transform.rotation = catData.rotation;
                Debug.Log("Data Loaded");
            }
        }
    }

}
