using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : CatBaseState
{
    float rotSpeed = 1.2f;
    public override void CheckState(CatFSM Cat)
    {
        //Checks wheather the player is currently sneaking and if not, cat will flee
        MovementScript movement = Cat.playerTransform.GetComponent<MovementScript>();
        if (!movement.isSneaking)
        {
            Cat.TransitionState(Cat.fleestate);
        }

        RaycastHit hit;
        if (Physics.Raycast(Cat.transform.position, Cat.playerTransform.transform.position - Cat.transform.position, out hit))
        {

            if (hit.transform.tag == "Player" && hit.distance < Cat.playerdistance /2)
            {
                Debug.DrawRay(Cat.transform.position, Cat.playerTransform.transform.position - Cat.transform.position, Color.red);
                Cat.TransitionState(Cat.fleestate);
            }
            //The cat will go back into wandering if you gave it enough space
            else if(hit.transform.tag == "Player" && hit.distance > Cat.playerdistance * 4)
            {
                Cat.TransitionState(Cat.wanderstate);
            }
            else
            {
                Debug.DrawRay(Cat.transform.position, Cat.playerTransform.transform.position - Cat.transform.position, Color.green);
            }
        }
    }

    public override void EnterState(CatFSM Cat)
    {
        Debug.Log("Cat is alert");
        Cat.navmesh.speed = 0;
    }

    public override void UpdateState(CatFSM Cat)
    {
        //Something that the cat rotates towards the direction of the player would be good
        Vector3 iSeeHuman = Cat.playerTransform.transform.position - Cat.transform.position;

        float singleStep = rotSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(Cat.transform.forward, iSeeHuman, singleStep, 0.0f);
        Cat.transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
