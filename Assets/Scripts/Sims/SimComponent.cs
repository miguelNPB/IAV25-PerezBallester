using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimComponent : MonoBehaviour
{
    public Sprite spriteFace;
    public string name;
    public float hunger = 1;
    public float bladder = 1;
    public float social = 1;
    public float sleep = 1;


    [SerializeField] private DiamondComponent diamond;
    public void toggleDiamondAnimation()
    {
        diamond.ToggleSelected();
    }
    public void SendSimToUI()
    {
        UIManager.Instance.ChangeSimUI(spriteFace, name, hunger, bladder, social, sleep);
    }
    void Start()
    {
        
    }
    void Update()
    {
        diamond.updateHappy(Mathf.Min(hunger, bladder, social, sleep));
    }
}
