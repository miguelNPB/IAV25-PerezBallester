using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class SmartObject : MonoBehaviour
{
    public List<Activity> activities;
    [SerializeField] private GameObject blockedSign;
    protected Activity runningActivity;
    [HideInInspector]
    public bool occupied;
    [HideInInspector]
    public bool thundered;
    private float thunderTimer;

    public Activity GetBestActivity(SimComponent sim)
    {
        if (occupied || thundered)
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

    public void Thunder(float time)
    {
        thundered = true;
        thunderTimer = time;
        blockedSign.SetActive(true);
    }
    private void Dethunder()
    {
        thundered = false;
        blockedSign.SetActive(false);
    }

    private void Start()
    {
        occupied = false;
        thundered = false;
        blockedSign.SetActive(false);
    }
    public void Update()
    {
        if (thundered)
        {
            thunderTimer -= Time.deltaTime;

            if (thunderTimer < 0)
                Dethunder();
        }
    }
}
