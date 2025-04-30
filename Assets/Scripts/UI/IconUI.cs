using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconUI : MonoBehaviour
{
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private Image image;
    public void changeState(bool state)
    {
        image.sprite = state ? onSprite : offSprite;
    }
    private void Start()
    {
        image = GetComponent<Image>();
    }
}
