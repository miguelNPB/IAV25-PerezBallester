using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetNecesityValues : Action
{
    public SharedGameObject simGameObject;
    public SharedInt hungerPriority;
    public SharedInt bladderPriority;
    public SharedInt socialPriority;
    public SharedInt sleepPriority;

    private int getPriorityFromValue(float value)
    {
        if (value > 75)
            return 0;
        else if (value > 50)
            return 1;
        else if (value > 25)
            return 2;
        else
            return 3;
    }
    public override void OnStart()
    {
        SimComponent sim = simGameObject.Value.GetComponent<SimComponent>();
        hungerPriority.SetValue(getPriorityFromValue(sim.hunger));
        bladderPriority.SetValue(getPriorityFromValue(sim.bladder));
        socialPriority.SetValue(getPriorityFromValue(sim.social));
        sleepPriority.SetValue(getPriorityFromValue(sim.sleep));
    }
}
