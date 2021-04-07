using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class Flee : Action
{
    public GameObject HidingSpots;
    public SharedFloat CatSpeed;
    NavMeshAgent navMesh;
    int childNumber;
    public SharedGameObject Player;
    public override void OnStart()
    {
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = CatSpeed.Value*2;
        navMesh.ResetPath();

        HidingSpots = GameObject.FindGameObjectWithTag("HidingSpot");
        //Here the hidings spot with the most distance to the player gets picked for the cat to run towards
        Transform allHideSpots = HidingSpots.transform;
        int spots = allHideSpots.childCount;
        float mostDistance = 0;
        for (int i = 0; i < spots; i++)
        {
            float distance = Vector3.Distance(allHideSpots.GetChild(i).transform.position, Player.Value.transform.position);
            //gets the distance between the hidingspot and the Player
            if (distance < 0)
            {
                distance = distance * -1;
            }

            if (distance > mostDistance)
            {
                mostDistance = distance;
                childNumber = i;
            }
        }

        Transform bestHideSpot = allHideSpots.GetChild(childNumber).transform;

        navMesh.SetDestination(bestHideSpot.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (navMesh.remainingDistance < navMesh.stoppingDistance)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}