using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsPlayerSneaking : Conditional
{
    public SharedGameObject Player;
    MovementScript movementscript;

    public override TaskStatus OnUpdate()
    {
        if (movementscript.isSneaking)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

    public override void OnStart()
    {
        movementscript = Player.Value.GetComponentInChildren<MovementScript>();
    }
}