using BehaviorDesigner.Runtime.Tasks.Tutorials;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerActions : MonoBehaviour
{
    public enum Mode { None, Move, Thunder, Gold }

    public List<GameObject> allSims;
    public float thunderTime;

    private Mode mode;
    private GameObject selectedSim;
    private SimComponent selectedSimComponent;
    private NavMeshAgent selectedSimNavAgent;

    private GameObject outlinedGameObject;
    void Start()
    {
        outlinedGameObject = null;
    }

    void Update()
    {
        switch (mode)
        {
            case Mode.Move:
                HandleMoveSim();
                break;
            case Mode.Gold:
                HandleGold();
                break;
            case Mode.Thunder:
                HandleThunder();
                break;
            case Mode.None:
                HandleSelectSim();
                break;
        }

        if (selectedSim != null)
            selectedSimComponent.SendSimToUI();

        HandleChangeMode();
    }

    private void HandleChangeMode()
    {
        Mode oldMode = mode;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (mode == Mode.Move)
            {
                mode = Mode.None;
                if (selectedSim != null)
                {
                    if (selectedSimComponent.currentActivity != null)
                        selectedSimComponent.enableAutoModeOnActivityDone = true;
                    else
                        selectedSimComponent.playerMoving = false;

                    if (outlinedGameObject != null)
                    {
                        SetLayerRecursively(outlinedGameObject, LayerMask.NameToLayer("Default"));
                        outlinedGameObject = null;
                    }

                    selectedSimComponent.toggleDiamondAnimation(false);
                }
            }
            else
            {
                mode = Mode.Move;

                if (selectedSim != null)
                {
                    selectedSimComponent.enableAutoModeOnActivityDone = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (mode == Mode.Gold)
                mode = Mode.None;
            else
                mode = Mode.Gold;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (mode == Mode.Move)
                mode = Mode.Thunder;
            else
                mode = Mode.Thunder;
        }

        if (oldMode != mode)
        {
            UIManager.Instance.ChangePlayerActionIcon(mode);
        }
    }
    private void HandleSelectSim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // seleccionar smartObject
        if (Physics.Raycast(ray, out hit, 100f) && hit.collider.gameObject.tag == "SmartObject")
        {
            if (outlinedGameObject == null)
            {
                outlinedGameObject = hit.collider.gameObject.transform.parent.gameObject;

                SetLayerRecursively(hit.collider.gameObject, LayerMask.NameToLayer("OutlinedObject"));
            }

        }
        else if (outlinedGameObject != null)
        {
            SetLayerRecursively(outlinedGameObject, LayerMask.NameToLayer("Default"));

            outlinedGameObject = null;
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide))
            {
                if (selectedSim != null)
                    selectedSimComponent.toggleDiamondAnimation(false);

                selectedSim = hit.collider.gameObject;
                selectedSimComponent = selectedSim.GetComponent<SimComponent>();
                selectedSimNavAgent = selectedSim.GetComponent<NavMeshAgent>();
                selectedSimComponent.toggleDiamondAnimation(true);
            }
            else if (Physics.Raycast(ray, out hit, 100f) && hit.collider.gameObject.tag == "SmartObject")
            {
                UIManager.Instance.ChangeSmartObjectUI(hit.collider.gameObject.GetComponentInParent<SmartObject>());
            }
            else if (mode == Mode.None)
            {
                if (selectedSim != null)
                {
                    selectedSimComponent.toggleDiamondAnimation(false);
                }
                UIManager.Instance.HideUI();
                selectedSim = null;
                selectedSimComponent = null;
                selectedSimNavAgent = null;
            }
        }
    }
    private void HandleMoveSim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // seleccionar smartObject
        if (Physics.Raycast(ray, out hit, 100f) && hit.collider.gameObject.tag == "SmartObject")
        {
            if (outlinedGameObject == null)
            {
                outlinedGameObject = hit.collider.gameObject.transform.parent.gameObject;

                SetLayerRecursively(hit.collider.gameObject, LayerMask.NameToLayer("OutlinedObject"));
            }

        }
        else if (outlinedGameObject != null)
        {
            SetLayerRecursively(outlinedGameObject, LayerMask.NameToLayer("Default"));

            outlinedGameObject = null;
        }


        // mover
        if (Input.GetMouseButtonDown(0) && selectedSim != null && selectedSimComponent.currentActivity == null)
        {
            if (!selectedSimComponent.playerMoving)
            {
                selectedSimComponent.playerMoving = true;
                selectedSim.GetComponent<SimPersonality>().ExitFunMode();
            }

            if (outlinedGameObject == null && Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Floor"), QueryTriggerInteraction.Collide))
            {
                selectedSimNavAgent.SetDestination(hit.point);
            }
            else if (outlinedGameObject != null && !outlinedGameObject.GetComponent<SmartObject>().occupied)
            {
                Activity a = outlinedGameObject.GetComponent<SmartObject>().GetBestActivity(selectedSimComponent);

                if (a.necessity == Necessity.SOCIAL)
                {
                    int i = 0;
                    while (i < allSims.Count && selectedSimComponent.currentActivity == null)
                    {
                        SimComponent simCandidate = allSims[i].GetComponent<SimComponent>();
                        if (selectedSimComponent.name != simCandidate.name && simCandidate.currentActivity == null)
                        {
                            selectedSim.GetComponent<SimPersonality>().ExitFunMode();
                            a.reserveActivity(selectedSimComponent);
                            selectedSimComponent.currentActivity = a;

                            simCandidate.GetComponent<SimPersonality>().ExitFunMode();
                            a.reserveActivity(simCandidate);
                            simCandidate.queuedActivity = null;
                            simCandidate.currentActivity = a;
                            simCandidate.playerMoving = true;
                        }
                        i++;
                    }   
                }
                else
                {
                    selectedSim.GetComponent<SimPersonality>().ExitFunMode();
                    a.reserveActivity(selectedSimComponent);
                    selectedSimComponent.currentActivity = a;
                }
            }
        }
    }

    private void HandleGold()
    {

    }

    private void HandleThunder()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide) 
            && hit.collider.gameObject.GetComponent<SimComponent>().currentActivity == null && hit.collider.gameObject.GetComponent<SimComponent>().queuedActivity == null)
        {
            hit.collider.gameObject.GetComponent<SimPersonality>().ExitFunMode();
            hit.collider.gameObject.GetComponent<SimComponent>().Thunder(thunderTime);

            UIManager.Instance.ThunderAnimation();
        }
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
