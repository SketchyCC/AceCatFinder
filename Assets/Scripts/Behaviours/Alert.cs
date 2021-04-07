using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
public class Alert : Action
{
    MovementScript movement;
    public SharedGameObject Player;
    float rotSpeed = 1.2f;
    public SharedFloat playerdistance;
    public SharedBool IsAlert;

    public override void OnStart()
    {
        NavMeshAgent navmesh = gameObject.GetComponent<NavMeshAgent>();
        navmesh.ResetPath();
        IsAlert.Value = true;
    }
    public override TaskStatus OnUpdate()
    {
        LookatHuman();
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, Player.Value.transform.position - gameObject.transform.position, out hit))
        {

            if (hit.transform.tag == "Player" && hit.distance < playerdistance.Value / 3)
            {
                Debug.DrawRay(gameObject.transform.position, Player.Value.transform.position - gameObject.transform.position, Color.red);
                return TaskStatus.Running;
            }
            //The cat will go back into wandering if you gave it enough space
            else if (hit.transform.tag == "Player" && hit.distance > playerdistance.Value)
            {
                
                return TaskStatus.Success; //move to wander action
            }
            else
            {
                Debug.DrawRay(gameObject.transform.position, Player.Value.transform.position - gameObject.transform.position, Color.green);
                return TaskStatus.Running;
            }
        }

        return TaskStatus.Failure;

    }

    void LookatHuman()
    {
        Vector3 iSeeHuman = Player.Value.transform.position - gameObject.transform.position;

        float singleStep = rotSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, iSeeHuman, singleStep, 0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
    }
    public override void OnEnd()
    {
        Debug.Log("OnEnd called");
        IsAlert.Value = false;
    }
    public override void OnBehaviorComplete()
    {
        Debug.Log("OnBehaviourComplete called");
    }
}