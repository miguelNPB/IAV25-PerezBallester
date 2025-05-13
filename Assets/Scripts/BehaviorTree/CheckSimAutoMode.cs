using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting;

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
        if (sim.playerMoving || sim.distracted || sim.thundered)
            return TaskStatus.Failure;
        else
            return TaskStatus.Success;
    }
}
