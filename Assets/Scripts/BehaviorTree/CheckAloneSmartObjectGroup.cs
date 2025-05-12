using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckAloneSmartObjectGroup : Action
{
    public SharedGameObject simGameObject;
    public SharedGameObject smartObjectGroupGameObject;

    private SmartObjectGroup smartObjectGroup;
    private SimComponent sim;

    private bool runningActivity;
    public override float GetPriority()
    {
        runningActivity = false;
        sim = simGameObject.Value.GetComponent<SimComponent>();
        smartObjectGroup = smartObjectGroupGameObject.Value.GetComponent<SmartObjectGroup>();

        return sim.getPriority(smartObjectGroup.necessity);
    }

    public override void OnStart()
    {
        sim.currentActivity = smartObjectGroup.GetBestSmartObjectActivity(sim);

        if (sim.currentActivity != null)
        {
            runningActivity = true;
            sim.GetComponent<SimPersonality>().ExitFunMode();
            sim.currentActivity.reserveActivity(sim);
        }
        else
            runningActivity = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (!runningActivity || sim.currentActivity == null)
            return TaskStatus.Failure;

        ActivityStatus aStatus = sim.currentActivity.getActivityStatus();
        runningActivity = aStatus != ActivityStatus.CLEAR;

        if (aStatus == ActivityStatus.CLEAR)
            sim.currentActivity = null;

        return TaskStatus.Running;  
    }
}
