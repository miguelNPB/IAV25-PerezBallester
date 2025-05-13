using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckSimAutoMode : Action
{
    public SharedGameObject simGameObject;

    private SimComponent sim;
    public override void OnStart()
    {
        sim = simGameObject.Value.GetComponent<SimComponent>();
        
    }

    public override TaskStatus OnUpdate()
    {
        if (sim.playerMoving || sim.thundered)
            return TaskStatus.Failure;
        else
            return TaskStatus.Success;
    }
}
