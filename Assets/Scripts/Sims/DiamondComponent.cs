using BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondComponent : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;
    public float returnSpeed = 2f;
    private float initialPositionY;
    private Quaternion initialRotation;
    private float floatTimer = 0f;

    private Renderer renderer;
    private bool animActive;
    private void Start()
    {
        initialPositionY = transform.position.y;
        initialRotation = transform.rotation;

        animActive = false;
        renderer = gameObject.GetComponent<Renderer>();
    }
    private void Update()
    {
        AnimationSelected();
    }
    public void ToggleSelected(bool value)
    {
        animActive = value;
    }
    public void UpdateHappy(float newHappy)
    {
        float green = Mathf.InverseLerp(0, 0.5f, newHappy);
        float red = 1 - Mathf.InverseLerp(0.5f, 1f, newHappy);

        renderer.material.color = new Color(red, green, 0 );
    }

    // Animacion de flotar al seleccionar el sim
    private void AnimationSelected()
    {
        if (animActive)
        {
            floatTimer += Time.deltaTime * floatFrequency;
            float yOffset = Mathf.Sin(floatTimer) * floatAmplitude;

            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

            // Limitar a 0–360
            if (transform.eulerAngles.y > 360f)
            {
                Vector3 euler = transform.eulerAngles;
                euler.y -= 360f;
                transform.eulerAngles = euler;
            }

            Vector3 originalPos = transform.position;
            originalPos.y = initialPositionY;
            Vector3 floatedPosition = originalPos + new Vector3(0, yOffset, 0);
            transform.position = floatedPosition;
        }
        else
        {
            Vector3 originalPos = transform.position;
            originalPos.y = initialPositionY;
            transform.position = Vector3.Lerp(transform.position, originalPos, Time.deltaTime * returnSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * returnSpeed);
        }
    }
}
