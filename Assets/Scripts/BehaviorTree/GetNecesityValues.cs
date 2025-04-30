using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetNecesityValues : Action
{
    public SharedGameObject simGameObject;
    public SharedFloat hunger;
    public SharedFloat bladder;
    public SharedFloat social;
    public SharedFloat sleep;
    public override void OnStart()
    {
        SimComponent sim = simGameObject.Value.GetComponent<SimComponent>();
        hunger.SetValue(sim.hunger);
        bladder.SetValue(sim.hunger);
        social.SetValue(sim.hunger);
        sleep.SetValue(sim.sleep);
    }
}
