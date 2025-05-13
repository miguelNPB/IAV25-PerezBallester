using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class GoldComponent : MonoBehaviour
{
    public float timeActive;

    private List<SimComponent> attractedSims;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Sim")
        {
            SimComponent sim = other.GetComponent<SimComponent>();

            if (!attractedSims.Contains(sim))
            {
                if (sim.currentActivity != null && sim.currentActivity.activityStatus != ActivityStatus.WAITING)
                    return;

                sim.distracted = true;
                sim.GetComponent<SimPersonality>().ExitFunMode();
                sim.GetComponent<NavMeshAgent>().SetDestination(transform.position);

                attractedSims.Add(sim);
            }
        }
    }
    private void CheckAutoDestroy()
    {
        if (timeActive < 0)
        {
            for (int i = 0; i < attractedSims.Count; i++)
            {
                attractedSims[i].distracted = false;
                attractedSims[i].currentActivity?.RestoreNavMeshRoute();
            }

            Destroy(gameObject);
        }
    }

    private void Start()
    {
        attractedSims = new List<SimComponent>();
    }
    void Update()
    {
        timeActive -= Time.deltaTime;
        CheckAutoDestroy();
    }
}
