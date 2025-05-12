using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum ActivityStatus { WAITING, RUNNING, CLEAR };
public class Activity : MonoBehaviour
{
    [Header("Funcionalidades obligatorias")]
    public Necessity necessity;
    [Range(0.0f,1f)]
    public float restore;
    public float activityTime;
    public AnimationClip animation;
    [Header("Efectos (opcionales)")]
    public AudioClip audioSFX;
    public ParticleSystem particleSystem;
    public List<GameObject> toggleGameObjects;

    public ActivityStatus activityStatus;
    protected SmartObject smartObject;
    protected SimComponent sim;
    [HideInInspector]
    public float timer;

    protected AudioSource audioSource;
    protected ProgressBar progressBar;
    public virtual float getDistanceToActivity(SimComponent sim)
    {
        float distance = 0f;

        Vector3 simPos = sim.GetComponent<Transform>().position;

        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, simPos, NavMesh.AllAreas, path))
        {
            for (int i = 1; i < path.corners.Length; i++)
            {
                distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
        }

        return distance;
    }
    public virtual float getActivityScore(SimComponent sim)
    {
        float depletion = sim.getNecessityDepletion(necessity);

        float score = depletion + restore;
        if (score > 1)
            score = 0.25f - (score - 1);
        else
            score = score - 0.75f;

        return score;
    }

    public virtual void reserveActivity(SimComponent sim)
    {
        smartObject.Occupy();
        activityStatus = ActivityStatus.WAITING;
        this.sim = sim;
        sim.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        progressBar.SetupProgressBar();
    }

    public virtual ActivityStatus getActivityStatus()
    {
        return activityStatus;
    }

    public virtual void UpdateActivity()
    {
        if (activityStatus == ActivityStatus.WAITING)
        {
            if (Vector3.Distance(sim.transform.position, transform.position) < 0.5f)
            {
                ExternalAnimationController simAnimator = sim.GetComponent<ExternalAnimationController>();
                simAnimator.PlayExternalAnimation(animation);
                timer = activityTime;

                if (audioSFX)
                {
                    audioSource.clip = audioSFX;
                    audioSource.Play();
                }
                if (particleSystem)
                    particleSystem.Play();

                sim.GetComponent<NavMeshAgent>().updateRotation = false;
                sim.transform.rotation = transform.rotation;

                progressBar.StartProgressBar(activityTime);

                foreach (GameObject GO in toggleGameObjects)
                {
                    GO.SetActive(!GO.activeSelf);
                }

                activityStatus = ActivityStatus.RUNNING;
            }
        }
        else if (activityStatus == ActivityStatus.RUNNING)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                sim.restoreNecessity(necessity, restore);
                sim.currentActivity = null;

                sim.GetComponent<ExternalAnimationController>().ManuallyDestroyAnimation();
                smartObject.Deoccupy();

                if (audioSFX)
                    audioSource.Stop();

                if (particleSystem)
                    particleSystem.Stop();

                sim.GetComponent<NavMeshAgent>().updateRotation = true;

                if (sim.enableAutoModeOnActivityDone)
                {
                    sim.playerMoving = false;
                    sim.enableAutoModeOnActivityDone = false;
                }

                foreach (GameObject GO in toggleGameObjects)
                {
                    GO.SetActive(!GO.activeSelf);
                }
                activityStatus = ActivityStatus.CLEAR;
            }
        }
    }
    private void Update()
    {
        UpdateActivity();
    }

    private void Start()
    {
        smartObject = transform.GetComponentInParent<SmartObject>();
        audioSource = GetComponent<AudioSource>();
        activityStatus = ActivityStatus.CLEAR;
        if (particleSystem)
            particleSystem.Stop();
        progressBar = GetComponentInChildren<ProgressBar>();
    }
}
