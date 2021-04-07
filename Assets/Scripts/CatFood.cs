using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;

public class CatFood : MonoBehaviour
{

    public float FoodDuration;

    private void OnTriggerEnter(Collider other)
    {
        Cat catobj = other.gameObject.GetComponentInParent<Cat>();
        if (catobj)
        {
            var EatingBool = (SharedBool)catobj.CatBehaviour.GetVariable("IsEating");
            if (EatingBool != null)
            {
                if (EatingBool.Value == false)
                {
                    catobj.CatBehaviour.SendEvent("ObjectOfInterest");
                    catobj.CatBehaviour.SetVariableValue("IsEating", true);
                    NavMeshAgent navmesh = other.GetComponentInParent<NavMeshAgent>();
                    navmesh.SetDestination(gameObject.transform.position);
                    StartCoroutine(RemoveCatFood(catobj));
                }
            }
        }
    }


    IEnumerator RemoveCatFood(Cat catobj)
    {
        yield return new WaitForSeconds(FoodDuration+2f); //added some time for the cat to walk to the food
        catobj.CatBehaviour.SetVariableValue("IsEating", false);
        catobj.CatBehaviour.SendEvent("RestartTree");

        Destroy(transform.parent.gameObject);
    }
}
