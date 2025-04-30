using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    [SerializeField] private GameObject simPanel;
    [SerializeField] private Image simFaceHolder;
    [SerializeField] private TMP_Text simNameText;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider bladderSlider;
    [SerializeField] private Slider socialSlider;
    [SerializeField] private Slider sleepSlider;
    [SerializeField] private IconUI moveIcon;
    [SerializeField] private IconUI goldIcon;
    [SerializeField] private IconUI thunderIcon;

    public void ChangePlayerActionIcon(PlayerActions.Mode mode)
    {
        moveIcon.changeState(mode == PlayerActions.Mode.Move);
        goldIcon.changeState(mode == PlayerActions.Mode.Gold);
        thunderIcon.changeState(mode == PlayerActions.Mode.Thunder);
    }
    public void HideSimUI()
    {
        simPanel.SetActive(false);
    }
    public void ChangeSimUI(Sprite newSprite, string name, float hunger, float bladder, float social, float sleep)
    {
        simPanel.SetActive(true);
        simFaceHolder.sprite = newSprite;
        simNameText.text = name;
        hungerSlider.value = hunger;
        bladderSlider.value = bladder;
        socialSlider.value = social;
        sleepSlider.value = sleep;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

        
    }

    private void Start()
    {
        HideSimUI();
    }
}
