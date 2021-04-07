using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class Wander : Action
{
    public float radius;
    public SharedFloat Speed;
    public NavMeshAgent navmesh;
    public override void OnStart()
    {
        navmesh = GetComponent<NavMeshAgent>();
        Vector3 newPos = RandomNavSphere(gameObject.transform.position, radius, -1);
        navmesh.SetDestination(newPos);
        navmesh.speed = Speed.Value;
    }
    public override TaskStatus OnUpdate()
    {
        if (navmesh.remainingDistance < navmesh.stoppingDistance)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Success; //it gets stuck on the navmesh so changed to success after setting the destination
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, dist, layermask);

        return navHit.position;
    }
}