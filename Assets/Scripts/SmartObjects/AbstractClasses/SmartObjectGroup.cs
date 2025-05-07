using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmartObjectGroup : MonoBehaviour
{
    public Necessity necessity;
    [SerializeField] protected List<SmartObject> smartObjects;
    public abstract Activity GetBestSmartObjectActivity(SimComponent sim);
}
