using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IfSeeObject : Conditional
{
    public SharedGameObject Player;
    public SharedFloat playerdistance;

    public override TaskStatus OnUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, Player.Value.transform.position - gameObject.transform.position, out hit))
        {
            if (hit.transform.tag == "Player" && hit.distance < playerdistance.Value)
            {
                Debug.DrawRay(gameObject.transform.position, Player.Value.transform.position - gameObject.transform.position, Color.red);
                return TaskStatus.Success;
            }
            else
            {
                Debug.DrawRay(gameObject.transform.position, Player.Value.transform.position - gameObject.transform.position, Color.green);
                return TaskStatus.Failure;
            }
        }
        return TaskStatus.Running;
    }
}