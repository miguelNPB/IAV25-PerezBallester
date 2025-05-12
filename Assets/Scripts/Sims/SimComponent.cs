using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum Necessity { HUNGER, BLADDER, SOCIAL ,SLEEP };
public class SimComponent : MonoBehaviour
{
    [SerializeField] private DiamondComponent diamond;
    [SerializeField] private GameObject thunderedParticleSystem;
    public Sprite spriteFace;
    public string name;
    public float hunger = 1;
    public float bladder = 1;
    public float social = 1;
    public float sleep = 1;

    [HideInInspector]
    public Activity currentActivity;
    [HideInInspector]
    public Activity queuedActivity;
    [HideInInspector]
    public bool playerMoving;
    [HideInInspector]
    public bool distracted;
    private bool thundered;
    private float thunderTimer;
    [HideInInspector]
    public bool enableAutoModeOnActivityDone;


    private Animator animator;
    private NavMeshAgent navMeshAgent;

    public void toggleDiamondAnimation(bool value)
    {
        diamond.ToggleSelected(value);
    }
    public void SendSimToUI()
    {
        UIManager.Instance.ChangeSimUI(spriteFace, name, hunger, bladder, social, sleep);
    }
    public int getPriority(Necessity n)
    {
        switch (n)
        {
            case Necessity.HUNGER:
                return hunger > 0.75 ? 0 : hunger > 0.5 ? 1 : hunger > 0.25 ? 2 : 3;
            case Necessity.BLADDER:
                return bladder > 0.75 ? 0 : bladder > 0.5 ? 1 : bladder > 0.25 ? 2 : 3;
            case Necessity.SOCIAL:
                return social > 0.75 ? 0 : social > 0.5 ? 1 : social > 0.25 ? 2 : 3;
            case Necessity.SLEEP:
                return sleep > 0.75 ? 0 : sleep > 0.5 ? 1 : sleep > 0.25 ? 2 : 3;
            default:
                return -1;
        }
    }
    public float getNecessityDepletion(Necessity n)
    {
        switch (n)
        {
            case Necessity.HUNGER:
                return hunger;
            case Necessity.BLADDER:
                return bladder;
            case Necessity.SOCIAL:
                return social;
            case Necessity.SLEEP:
                return sleep;
            default:
                return -1;
        }
    }
    public void restoreNecessity(Necessity n, float ammount)
    {
        switch (n)
        {
            case Necessity.HUNGER:
                hunger = Mathf.Min(hunger + ammount, 1);
                break;
            case Necessity.BLADDER:
                bladder = Mathf.Min(bladder + ammount, 1);
                break;
            case Necessity.SOCIAL:
                social = Mathf.Min(social + ammount, 1);
                break;
            case Necessity.SLEEP:
                sleep = Mathf.Min(sleep + ammount, 1);
                break;
        }
    }

    public void Thunder(float time)
    {
        thundered = true;
        thunderTimer = time;
        animator.SetBool("Thundered", true);
        navMeshAgent.velocity = new Vector3(0, 0, 0);
        navMeshAgent.SetDestination(transform.position);
        thunderedParticleSystem.SetActive(true);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerMoving = false;
        enableAutoModeOnActivityDone = false;
        distracted = false;
        thundered = false;
    }
    void Update()
    {
        diamond.UpdateHappy(Mathf.Min(hunger, bladder, social, sleep));
        animator.SetBool("Walking", navMeshAgent.velocity.magnitude > 0.1f);

        if (thundered)
        {
            thunderTimer -= Time.deltaTime;
            if (thunderTimer < 0)
            {
                thunderedParticleSystem.SetActive(false);
                animator.SetBool("Thundered", false);
                thundered = false;
            }
        }
    }
}
