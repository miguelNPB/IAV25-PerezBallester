using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class SmartObject : MonoBehaviour
{
    [SerializeField] protected List<Activity> activities;
    protected Activity runningActivity;
    [HideInInspector]
    public bool occupied;

    public bool IsTaskAvailable()
    {
        return occupied;
    }
    public Activity GetBestActivity(SimComponent sim)
    {
        if (occupied)
            return null;

        Activity best = null;
        float bestScore = -9999;
        foreach (Activity a in activities)
        {
            float score = a.getActivityScore(sim);
            if (score > bestScore)
            {
                bestScore = score;
                best = a;
            }
        }

        return best;
    }
    public void Occupy()
    {
        occupied = true;
    }

    public void Deoccupy()
    {
        occupied = false;
    }

    private void Start()
    {
        occupied = false;
    }
}
