using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;

    private SpriteRenderer spriteRenderer;
    private Activity activity;

    private float duration;
    private float timer;
    private bool running;
    public void StartProgressBar(float time)
    {
        duration = time;
        timer = 0;
        running = true;
    }
    public void SetupProgressBar()
    {
        spriteRenderer.sprite = sprites[0];
        spriteRenderer.enabled = true;
    }

    private void SetNecessityColor()
    {
        Necessity n = activity.necessity;
        switch (n)
        {
            case Necessity.HUNGER:
                spriteRenderer.color = new Color(1.0f, 0.54f, 0f, spriteRenderer.color.a);
                break;
            case Necessity.BLADDER:
                spriteRenderer.color = new Color(0f, 0.9f, 1f, spriteRenderer.color.a);
                break;
            case Necessity.SOCIAL:
                spriteRenderer.color = new Color(0.18f, 1f, 0f, spriteRenderer.color.a);
                break;
            case Necessity.SLEEP:
                spriteRenderer.color = new Color(1.0f, 0f, 0.91f, spriteRenderer.color.a);
                break;
        }
    }
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        activity = GetComponentInParent<Activity>();
        SetNecessityColor();
        spriteRenderer.enabled = false;
    }
    private void Update()
    {
        if (running)
        {
            timer += Time.deltaTime;

            spriteRenderer.sprite = sprites[(int)(timer / (duration / 12))];
            if (timer > (duration - (duration / 24)))
            {
                running = false;
                spriteRenderer.enabled = false;
            }
        }
    }
}
