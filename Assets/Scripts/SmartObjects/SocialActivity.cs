using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SocialActivity : Activity
{
    [Header("Ajustes social activity")]
    public AnimationClip secondSimAnimation;
    private SimComponent secondSim;
    private bool firstSimWaiting;

    private void Start()
    {
        firstSimWaiting = false;
        smartObject = transform.GetComponentInParent<SmartObject>();
        audioSource = GetComponent<AudioSource>();
        activityStatus = ActivityStatus.CLEAR;
        if (particleSystem)
            particleSystem.Stop();
        progressBar = GetComponentInChildren<ProgressBar>();
    }
    public override void reserveActivity(SimComponent sim)
    {
        if (!firstSimWaiting)
        {
            smartObject.Occupy();
            activityStatus = ActivityStatus.WAITING;
            this.sim = sim;
            sim.GetComponent<NavMeshAgent>().SetDestination(transform.position);
            progressBar.SetupProgressBar();
            firstSimWaiting = true;
        }
        else
        {
            secondSim = sim;
            secondSim.queuedActivity = this;
            secondSim.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        }
    }
    public override void UpdateActivity()
    {
        if (activityStatus == ActivityStatus.WAITING && secondSim != null)
        {
            if (Vector3.Distance(sim.transform.position, transform.position) < 0.5f && Vector3.Distance(secondSim.transform.position, transform.position) < 0.5f)
            {
                activityStatus = ActivityStatus.RUNNING;
                timer = activityTime;

                // play animacion 1 y 2 a los sims
                ExternalAnimationController simAnimator = sim.GetComponent<ExternalAnimationController>();
                simAnimator.PlayExternalAnimation(animation);
                simAnimator = secondSim.GetComponent<ExternalAnimationController>();
                simAnimator.PlayExternalAnimation(secondSimAnimation);

                if (audioSFX)
                {
                    audioSource.clip = audioSFX;
                    audioSource.Play();
                }
                if (particleSystem)
                    particleSystem.Play();

                sim.GetComponent<NavMeshAgent>().updateRotation = false;
                secondSim.GetComponent<NavMeshAgent>().updateRotation = false;

                sim.transform.rotation = transform.rotation;
                secondSim.transform.rotation = transform.rotation;

                progressBar.StartProgressBar(activityTime);
            }
        } 
        else if (activityStatus == ActivityStatus.RUNNING)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                sim.restoreNecessity(necessity, restore);
                secondSim.restoreNecessity(necessity, restore);

                sim.GetComponent<ExternalAnimationController>().ManuallyDestroyAnimation();
                secondSim.GetComponent<ExternalAnimationController>().ManuallyDestroyAnimation();

                smartObject.Deoccupy();

                if (audioSFX)
                    audioSource.Stop();

                if (particleSystem)
                    particleSystem.Stop();

                sim.GetComponent<NavMeshAgent>().updateRotation = true;
                secondSim.GetComponent<NavMeshAgent>().updateRotation = true;

                if (sim.enableAutoModeOnActivityDone)
                {
                    sim.playerMoving = false;
                    sim.enableAutoModeOnActivityDone = false;
                }
                if (secondSim.enableAutoModeOnActivityDone)
                {
                    secondSim.playerMoving = false;
                    secondSim.enableAutoModeOnActivityDone = false;
                }

                secondSim.queuedActivity = null;
                secondSim.currentActivity = null;

                sim.currentActivity = null;
                sim.queuedActivity = null;

                firstSimWaiting = false;

                sim.playerMoving = false;
                secondSim.playerMoving = false;

                foreach (GameObject GO in toggleGameObjects)
                {
                    GO.SetActive(!GO.activeSelf);
                }

                activityStatus = ActivityStatus.CLEAR;
            }
        }
    }
}
