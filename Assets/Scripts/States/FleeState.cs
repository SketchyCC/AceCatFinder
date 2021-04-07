using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : CatBaseState
{
    private int childNumber;
    float timer;
    float delay = 1;


    public override void CheckState(CatFSM Cat)
    {
        if(timer > delay)
        {
            if (Cat.navmesh.remainingDistance <= Cat.navmesh.stoppingDistance)
            {
                timer = 0;
                Cat.TransitionState(Cat.hidestate);
            }
        }
        
    }

    public override void EnterState(CatFSM Cat)
    {
        Debug.Log("Cat entered fleeing state");
        Cat.navmesh.speed = Cat.CatSpeed * 2;

        //Here the hidings spot with the most distance to the player gets picked for the cat to run towards
        Transform allHideSpots = Cat.hidingspot.transform;
        int spots = allHideSpots.childCount;
        float mostDistance = 0;
        for(int i = 0; i < spots; i++)
        {
            float distance = Vector3.Distance(allHideSpots.GetChild(i).transform.position, Cat.playerTransform.transform.position);
            //gets the distance between the hidingspot and the Player
            if(distance < 0)
            {
                distance = distance * -1;
            }

            if(distance > mostDistance)
            {
                mostDistance = distance;
                childNumber = i;
            } 
        }

        Transform bestHideSpot = allHideSpots.GetChild(childNumber).transform;

        Cat.navmesh.SetDestination(bestHideSpot.position);
    }

    public override void UpdateState(CatFSM Cat)
    {
        timer += Time.deltaTime;
    }


}
