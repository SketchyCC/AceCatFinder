using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameEvents;

public class CatFSM : MonoBehaviour
{
    public int Id; 
    //wander, flee, hide
    public GameObject hidingspot;
    public NavMeshAgent navmesh;

    public float playerdistance=10f;

    public GameObject playerTransform;
    public float wanderRadius;
    public float wanderTimer;

    CatBaseState currentcatstate;
    public readonly WanderingState wanderstate=new WanderingState();
    public readonly AlertState alertstate = new AlertState();
    public readonly FleeState fleestate = new FleeState();
    public readonly HideState hidestate = new HideState();
    public readonly TrappedState trappedstate = new TrappedState();

    public float CatSpeed;    
    public float Cathidingtime = 5f;
    public bool CorrectCat = true; //cat is always correct for now until you later can select the cat to be wrong at the identification phase

    private void OnEnable()
    {
        navmesh = GetComponent<NavMeshAgent>();
        GameEventManager.AddListener<WrongCatEvent>(OnWrongCatEvent);
    }

    private void OnDisable()
    {
        GameEventManager.RemoveListener<WrongCatEvent>(OnWrongCatEvent);
    }
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player");
        CatSpeed = navmesh.speed;
        TransitionState(wanderstate);
    }


    void Update()
    {
        currentcatstate.UpdateState(this);
        currentcatstate.CheckState(this);
    }

    public void TransitionState(CatBaseState state)
    {
        currentcatstate = state;
        currentcatstate.EnterState(this);

    }

    public virtual void OnWrongCatEvent(WrongCatEvent e)
    {
        //if (e.CatObj.Id == Id)
        //{
        //    TransitionState(fleestate);
        //}
    }
}
