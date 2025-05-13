using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectBoolPair
{
    public GameObject gameObject;
    public bool value;
}
public class ToggleObjects : MonoBehaviour
{
    public List<GameObjectBoolPair> objects = new List<GameObjectBoolPair>();

    public void ToggleAllObjects()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].gameObject.SetActive(objects[i].value);
        }
    }
}
