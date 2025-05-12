using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckQueuedActivity : Action
{
    public SharedGameObject simGameobject;

    private SimComponent sim;
    private bool runningActivity;
    public override float GetPriority()
    {
        runningActivity = false;
        sim = simGameobject.Value.GetComponent<SimComponent>();


        return sim.queuedActivity != null ? 10 : 0;
    }

    public override void OnStart()
    {
        runningActivity = true;
        sim.currentActivity = sim.queuedActivity;
        sim.queuedActivity = null;

        sim.GetComponent<SimPersonality>().ExitFunMode();

        sim.currentActivity.reserveActivity(sim);
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
