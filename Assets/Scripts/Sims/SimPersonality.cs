using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimPersonality : MonoBehaviour
{
    [Header("Depletion Rates de necesidades")]
    public float hungerDepletionTime;
    public float bladderDepletionTime;
    public float socialDepletionTime;
    public float sleepDepletionTime;
    [Header("Ajustes de modo ocio")]
    public List<GameObject> ocioAnimationProps;
    public List<Transform> ocioSpots;
    public float timeInSpot;
    public AnimationClip customOcioAnimation;
    public AudioClip customOcioSFX;
    public AnimationClip stressedAnimation;
    //public AudioClip stressedSFX;

    private float hungerTimer, bladderTimer, socialTimer, sleepTimer;
    private SimComponent sim;
    private AudioSource audioSource;
    private ExternalAnimationController simExternalAnimator;
    private NavMeshAgent navMeshAgent;

    private Transform selectedOcioSpot;
    private bool playingAnimation;
    private float timer;
    public void UpdateFunMode()
    {
        if (selectedOcioSpot == null)
        {
            Transform oldSelectedSpot = selectedOcioSpot;
            while (selectedOcioSpot == oldSelectedSpot)
                selectedOcioSpot = ocioSpots[Random.Range(0, ocioSpots.Count)];

            navMeshAgent.SetDestination(selectedOcioSpot.position);
        }
        else if (!playingAnimation && Vector3.Distance(selectedOcioSpot.position, sim.transform.position) < 0.5f)
        {
            playingAnimation = true;
            timer = timeInSpot;
            // tiene una necesidad baja pero no puede suplirla
            if (sim.hunger < 0.75 || sim.bladder < 0.75 || sim.social < 0.75 || sim.sleep < 0.75)
            {
                simExternalAnimator.PlayExternalAnimation(stressedAnimation);
                audioSource.Stop();
                //audioSource.clip = stressedSFX;
                //audioSource.Play();
            }
            else
            {
                simExternalAnimator.PlayExternalAnimation(customOcioAnimation);
                audioSource.clip = customOcioSFX;
                audioSource.Play();
            }
        } 
        else if (playingAnimation)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                ExitFunMode();
            }
        }

        

    }
    public void ExitFunMode()
    {
        foreach (GameObject prop in ocioAnimationProps)
            prop.SetActive(false);

        simExternalAnimator.ManuallyDestroyAnimation();
        audioSource.Stop();
        selectedOcioSpot = null;
        playingAnimation = false;
    }
    private void TickValues()
    {
        hungerTimer -= Time.deltaTime;
        if (hungerTimer < 0)
        {
            sim.hunger = Mathf.Max(0, sim.hunger - 0.01f);
            hungerTimer = hungerDepletionTime;
        }

        bladderTimer -= Time.deltaTime;
        if (bladderTimer < 0)
        {
            sim.bladder = Mathf.Max(0, sim.bladder - 0.01f);
            bladderTimer = bladderDepletionTime;
        }

        socialTimer -= Time.deltaTime;
        if (socialTimer < 0)
        {
            sim.social = Mathf.Max(0, sim.social - 0.01f);
            socialTimer = socialDepletionTime;
        }

        sleepTimer -= Time.deltaTime;
        if (sleepTimer < 0)
        {
            sim.sleep = Mathf.Max(0, sim.sleep - 0.01f);
            sleepTimer = sleepDepletionTime;
        }
    }

    void Start()
    {
        sim = GetComponent<SimComponent>();
        audioSource = GetComponent<AudioSource>();
        simExternalAnimator = GetComponent<ExternalAnimationController>();
        simExternalAnimator = GetComponent<ExternalAnimationController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        hungerTimer = hungerDepletionTime;
        bladderTimer = bladderDepletionTime;
        socialTimer = socialDepletionTime;
        sleepTimer = sleepDepletionTime;
        selectedOcioSpot = null;
    }
    void Update()
    {
        TickValues();
    }
}
