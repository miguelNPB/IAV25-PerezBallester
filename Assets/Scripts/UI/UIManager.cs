using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    [Header("Colores")]
    [SerializeField] private Color hungerColor;
    [SerializeField] private Color bladderColor;
    [SerializeField] private Color socialColor;
    [SerializeField] private Color sleepColor;
    [Header("UI info sim")]
    [SerializeField] private GameObject simPanel;
    [SerializeField] private Image simFaceHolder;
    [SerializeField] private TMP_Text simNameText;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider bladderSlider;
    [SerializeField] private Slider socialSlider;
    [SerializeField] private Slider sleepSlider;
    [SerializeField] private List<Sprite> prioritySprites;
    [SerializeField] private Image hungerPrioritySprite;
    [SerializeField] private Image bladderPrioritySprite;
    [SerializeField] private Image socialPrioritySprite;
    [SerializeField] private Image sleepPrioritySprite;
    [Header("UI player controls")]
    [SerializeField] private IconUI moveIcon;
    [SerializeField] private IconUI goldIcon;
    [SerializeField] private IconUI thunderIcon;
    [Header("UI info smartobjects")]
    [SerializeField] private GameObject smartObjectPanel;
    [SerializeField] private Sprite hungerSprite;
    [SerializeField] private Sprite bladderSprite;
    [SerializeField] private Sprite socialSprite;
    [SerializeField] private Sprite sleepSprite;
    [SerializeField] private TMP_Text smartObjectName;
    [SerializeField] private List<GameObject> smartObjectActivityHolder;
    [SerializeField] private List<TMP_Text> smartObjectActivitynames;
    [SerializeField] private List<TMP_Text> smartObjectActivitytimes;
    [SerializeField] private List<TMP_Text> smartObjectActivityrestores;
    [SerializeField] private List<Image> smartObjectActivityimages;
    [Header("Volver al menu prinicpal")]
    [SerializeField] private GameObject escapePanel;

    private Animator animator;
    private AudioSource audioSource;
    public void ThunderAnimation()
    {
        animator.SetTrigger("Thunder");
        audioSource.Play();
    }
    public void ChangePlayerActionIcon(PlayerActions.Mode mode)
    {
        moveIcon.changeState(mode == PlayerActions.Mode.Move);
        goldIcon.changeState(mode == PlayerActions.Mode.Gold);
        thunderIcon.changeState(mode == PlayerActions.Mode.Thunder);
    }
    public void HideUI()
    {
        smartObjectActivityHolder[0].SetActive(false);
        smartObjectActivityHolder[1].SetActive(false);
        smartObjectActivityHolder[2].SetActive(false);
        simPanel.SetActive(false);
        smartObjectPanel.SetActive(false);
    }
    public void ChangeSimUI(SimComponent sim)
    {
        simPanel.SetActive(true);
        simFaceHolder.sprite = sim.spriteFace;
        simNameText.text = sim.name;
        hungerSlider.value = sim.hunger;
        bladderSlider.value = sim.bladder;
        socialSlider.value = sim.social;
        sleepSlider.value = sim.sleep;

        hungerPrioritySprite.sprite = prioritySprites[sim.getPriority(Necessity.HUNGER)];
        bladderPrioritySprite.sprite = prioritySprites[sim.getPriority(Necessity.BLADDER)];
        socialPrioritySprite.sprite = prioritySprites[sim.getPriority(Necessity.SOCIAL)];
        sleepPrioritySprite.sprite = prioritySprites[sim.getPriority(Necessity.SLEEP)];
    }   
    private Sprite GetNecessitySprite(Necessity n)
    {
        switch (n)
        {
            case Necessity.HUNGER:
                return hungerSprite;
            case Necessity.BLADDER:
                return bladderSprite;
            case Necessity.SOCIAL:
                return socialSprite;
            case Necessity.SLEEP:
                return sleepSprite;
        }
        return null;
    }
    private Color GetNecessityColor(Necessity n)
    {
        switch (n)
        {
            case Necessity.HUNGER:
                return hungerColor;
            case Necessity.BLADDER:
                return bladderColor;
            case Necessity.SOCIAL:
                return socialColor;
            case Necessity.SLEEP:
                return sleepColor;
        }
        return Color.black;
    }
    public void ChangeSmartObjectUI(SmartObject smartObject)
    {
        smartObjectPanel.SetActive(true);
        smartObjectName.text = smartObject.name;

        for (int i = 0; i < smartObject.activities.Count; i++)
        {
            Activity a = smartObject.activities[i];
            smartObjectActivitynames[i].text = a.name;
            smartObjectActivitytimes[i].text = a.activityTime + "s";
            smartObjectActivityrestores[i].text = (a.restore * 100).ToString();
            smartObjectActivityimages[i].sprite = GetNecessitySprite(a.necessity);
            Color color = GetNecessityColor(a.necessity);
            color.a = smartObjectActivityHolder[i].GetComponent<Image>().color.a;
            smartObjectActivityHolder[i].GetComponent<Image>().color = color;
            smartObjectActivityHolder[i].SetActive(true);
        }
        
    }

    public void OpenEscapeButton()
    {
        escapePanel.SetActive(true);
    }

    public void CloseEscapeButton()
    {
        escapePanel.SetActive(false);
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
        HideUI();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        escapePanel.SetActive(false);
    }
}
