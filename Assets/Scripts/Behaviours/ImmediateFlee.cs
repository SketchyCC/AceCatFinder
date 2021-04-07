using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class ImmediateFlee : Action
{
    NavMeshAgent navmesh;
    public SharedGameObject Player;
    public SharedFloat speed;
    public SharedFloat Playerdistance;

    public override void OnStart()
    {
        navmesh = gameObject.GetComponent<NavMeshAgent>();
        navmesh.speed = speed.Value * 2;
        navmesh.ResetPath();
    }

    public override TaskStatus OnUpdate()
    {
        float distance = Vector3.Distance(transform.position, Player.Value.transform.position);

        if (distance < Playerdistance.Value*1.5f)
        {
            Vector3 dirToPlayer = 1 * (transform.position - Player.Value.transform.position);
            Vector3 newPos = transform.position + dirToPlayer;
            navmesh.SetDestination(newPos);

            return TaskStatus.Running;
        }
        else if (distance > Playerdistance.Value*1.5f)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}