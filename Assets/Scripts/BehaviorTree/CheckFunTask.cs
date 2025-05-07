using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckFunTask : Action
{
    public SharedGameObject simGameObject;

    private SimComponent sim;
    private SimPersonality simPersonality;
    public override float GetPriority()
    {
        return 0.5f;
    }

    public override void OnStart()
    {
        sim = simGameObject.Value.GetComponent<SimComponent>();
        simPersonality = simGameObject.Value.GetComponent<SimPersonality>();
    }

    public override TaskStatus OnUpdate()
    {
        simPersonality.UpdateFunMode();

        return TaskStatus.Success;
    }
}
