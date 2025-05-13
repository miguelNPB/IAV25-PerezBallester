using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckSharedSmartObjectGroup : Action
{
    public SharedGameObjectList allSims;
    public SharedGameObject simGameObject;
    public SharedGameObject smartObjectGroupGameObject;
    public SharedFloat minRemainingActivityTimeToReserveSim;

    private SmartObjectGroup smartObjectGroup;
    private SimComponent sim;
    private SimComponent otherSim;

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

        otherSim = null;
        float bestScore = -99999;
        if (sim.currentActivity != null)
        {
            foreach (GameObject simGO in allSims.Value)
            {
                SimComponent candidateSim = simGO.GetComponent<SimComponent>();
                if (candidateSim != sim && (candidateSim.currentActivity == null || candidateSim.currentActivity.timer < minRemainingActivityTimeToReserveSim.Value))
                {
                    // comprobar si el candidato tiene algo de prioridad en la necesidad que suple la actividad
                    if (candidateSim.getNecessityDepletion(sim.currentActivity.necessity) < 0.85f && sim.currentActivity.getActivityScore(candidateSim) > bestScore)
                    {
                        otherSim = candidateSim;
                        bestScore = sim.currentActivity.getActivityScore(candidateSim);
                    }
                }
            }
        }
        

        if (otherSim != null && sim.currentActivity != null)
        {
            runningActivity = true;

            sim.GetComponent<SimPersonality>().ExitFunMode();
            sim.currentActivity.reserveActivity(sim);

            if (otherSim.currentActivity == null)
            {
                otherSim.gameObject.GetComponent<SimPersonality>().ExitFunMode();
                sim.currentActivity.reserveActivity(otherSim);
            }
            else
            {
                otherSim.queuedActivity = sim.currentActivity;
            }
        }
        else
            runningActivity = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (!runningActivity || (sim.currentActivity == null && sim.queuedActivity == null))
            return TaskStatus.Failure;

        ActivityStatus aStatus = sim.currentActivity.getActivityStatus();
        runningActivity = aStatus != ActivityStatus.CLEAR;

        if (aStatus == ActivityStatus.CLEAR)
        {
            sim.currentActivity = null;
            otherSim.currentActivity = null;
        }

        return TaskStatus.Running;
    }
}
