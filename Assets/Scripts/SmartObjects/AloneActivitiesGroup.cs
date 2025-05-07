using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AloneActivitiesGroup : SmartObjectGroup
{
    public override Activity GetBestSmartObjectActivity(SimComponent sim)
    {
        float bestTime = 9999f;
        float bestDistance = 9999f;
        Activity best = null;

        foreach (SmartObject so in smartObjects)
        {
            Activity activity = so.GetBestActivity(sim);

            if (activity != null)
            {
                float distance = activity.getDistanceToActivity(sim);
                if (activity.activityTime < bestTime && distance < bestDistance)
                {
                    best = activity;
                    bestDistance = distance;
                    bestTime = activity.activityTime;
                }
            }
        }

        return best;
    }

}
