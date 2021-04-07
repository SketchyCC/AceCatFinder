using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideState : CatBaseState
{
    float timehidden = 0;
    float hidingtime;
    public override void CheckState(CatFSM Cat)
    {
        if (timehidden >= hidingtime)
        {
            Cat.TransitionState(Cat.wanderstate);
        }
    }

    public override void EnterState(CatFSM Cat)
    {
        timehidden=0;
        Debug.Log("Cat entered hiding state");
        Cat.navmesh.speed = 0;
        hidingtime = Cat.Cathidingtime;
    }

    public override void UpdateState(CatFSM Cat)
    {
        timehidden += Time.deltaTime;
    }
}
