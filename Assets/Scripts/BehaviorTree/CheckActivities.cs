using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckActivities : Action
{
    public SharedInt priority;
    public SharedGameObjectList availableActivities;
    public override float GetPriority()
    {
        return priority.Value;
    }

    /*
    private getBestActivity()
    {

    }

    public override TaskStatus OnUpdate()
    {
        
    }
    */
}
