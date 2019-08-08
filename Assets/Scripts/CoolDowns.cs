using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoolDowns : MonoBehaviour
{
    public Slider consumeSlider;
    public bool coolingDown;
    public float cooldown;
    private float nextUse;

    void Update()
     {
        if (Input.GetKeyDown(KeyCode.E))
        {
              Checking();
        }
        if (coolingDown == true)
        {
            UpdateUI();
            print("consume");
        }
        else
        {
            consumeSlider.value = 1;
        }
     }

    void Checking()
    {
        if (Time.time > nextUse && coolingDown == false)
        {
            consumeSlider.value = 0;
            nextUse = Time.time + cooldown;
            coolingDown = true;
        }
    }

    void UpdateUI()
    {
        consumeSlider.value += 1.0f / cooldown * Time.deltaTime;
        if(consumeSlider.value >= 1)
        {
            coolingDown = false;
        }
    }
}