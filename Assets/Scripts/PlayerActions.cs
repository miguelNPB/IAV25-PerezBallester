using BehaviorDesigner.Runtime.Tasks.Tutorials;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerActions : MonoBehaviour
{
    public enum Mode { None, Move, Thunder, Gold }

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
                break;
            case Mode.Thunder:
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
                    selectedSimComponent.playerMoving = false;
            }
            else
                mode = Mode.Move;
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
        //else if (Input.GetMouseButtonDown(1))
        //    mode = Mode.None;

        if (oldMode != mode)
        {
            UIManager.Instance.ChangePlayerActionIcon(mode);
        }
    }
    private void HandleSelectSim()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide))
            {
                selectedSim = hit.collider.gameObject;
                selectedSimComponent = selectedSim.GetComponent<SimComponent>();
                selectedSimNavAgent = selectedSim.GetComponent<NavMeshAgent>();
                selectedSimComponent.toggleDiamondAnimation();
            }
            else if (mode == Mode.None && selectedSim != null)
            {
                selectedSimComponent.toggleDiamondAnimation();
                UIManager.Instance.HideSimUI();
                selectedSim = null;
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

                for (int i = 0; i < outlinedGameObject.transform.childCount; i++)
                {
                    outlinedGameObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("OutlinedObject");
                }
            }

        }
        else if (outlinedGameObject != null)
        {
            for (int i = 0; i < outlinedGameObject.transform.childCount; i++)
            {
                outlinedGameObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Default");
            }

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
            else if (outlinedGameObject != null)
            {
                selectedSim.GetComponent<SimPersonality>().ExitFunMode();
                Activity a = outlinedGameObject.GetComponent<SmartObject>().GetBestActivity(selectedSimComponent);
                a.reserveActivity(selectedSimComponent);
                selectedSimComponent.currentActivity = a;

                selectedSim = null;
            }
        }
    }
}
