using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingState : CatBaseState
{
    float timer;
    float radius;

    float wandertime;

    public override void CheckState(CatFSM Cat)
    {
        RaycastHit hit;
        if (Physics.Raycast(Cat.transform.position, Cat.playerTransform.transform.position - Cat.transform.position, out hit))
        {
            if (hit.transform.tag == "Player"&&hit.distance<Cat.playerdistance*2)
            {
                Debug.DrawRay(Cat.transform.position, Cat.playerTransform.transform.position - Cat.transform.position, Color.red);
                Cat.TransitionState(Cat.alertstate);
            }
            else
            {
                Debug.DrawRay(Cat.transform.position, Cat.playerTransform.transform.position - Cat.transform.position, Color.green);
            }
        }
    }

    public override void EnterState(CatFSM Cat)
    {
        Cat.navmesh.speed = Cat.CatSpeed;
        timer = Cat.wanderTimer;
        wandertime = Cat.wanderTimer;
        radius = Cat.wanderRadius;
        Debug.Log("Meow (wander state entered)");
    }

    public override void UpdateState(CatFSM Cat)
    {
        timer += Time.deltaTime;
        if (timer >= wandertime)
        {
            Vector3 newPos = RandomNavSphere(Cat.transform.position, radius, -1);
            Cat.navmesh.SetDestination(newPos);
            timer = 0;
        }
        //Debug.Log("Meow meow meow");
    }
    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
