using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondComponent : MonoBehaviour
{
    private Renderer renderer;
    private Animator animator;

    private bool animActive;
    private void Start()
    {
        animActive = false;
        renderer = gameObject.GetComponent<Renderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    public void ToggleSelected()
    {
        animActive = !animActive;
        animator.SetBool("Bool", animActive);
    }
    public void updateHappy(float newHappy)
    {
        float green = Mathf.InverseLerp(0, 0.5f, newHappy);
        float red = 1 - Mathf.InverseLerp(0.5f, 1f, newHappy);

        renderer.material.color = new Color(red, green, 0 );
    }
}
