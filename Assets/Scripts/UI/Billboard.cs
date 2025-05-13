using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 dir = Camera.main.transform.position - transform.position;
        transform.forward = dir;
    }
}
