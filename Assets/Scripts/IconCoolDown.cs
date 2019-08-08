using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconCoolDown : MonoBehaviour {

    //Demo UI elements
    Slider timerSlider; // A slider with a min value of 0 and a max value of 1.

    float cdAmount; // How long does this skill cool down?
    float nextUse;

    void Start()
    {
        timerSlider = GetComponent<Slider>();
    }

    public bool isSkillUsable() // Return true if this skill is not cooling down.
    {
        return timerSlider.value == 1;
    }

    public void setCoolDown(float amount) // Call this function when the player pressed a skill
    {
        cdAmount = amount;
        nextUse = Time.time + amount;
        timerSlider.value = 0;
    }

    void Update()
    {
        if (Time.time < nextUse)
            FillSlider();
        else
            timerSlider.value = 1;
    }
    
    void FillSlider()
    {
        timerSlider.value += Time.deltaTime / cdAmount;
    }
}
