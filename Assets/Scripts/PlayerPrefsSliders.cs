using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerPrefsSliders : MonoBehaviour {

    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider brightnessSlider;
    public Image brightnessPanel;


    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerPrefs.SetFloat("MasterSlider", 0.5f);
            PlayerPrefs.SetFloat("BGMSlider", 0.5f);
            PlayerPrefs.SetFloat("SFXSlider", 0.5f);
            PlayerPrefs.SetFloat("BrightnessSlider", 1f);
        }
        masterSlider.value = PlayerPrefs.GetFloat("MasterSlider");
        bgmSlider.value = PlayerPrefs.GetFloat("BGMSlider");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXSlider");
        brightnessSlider.value = PlayerPrefs.GetFloat("BrightnessSlider");
    }

    void Update()
    {
        PlayerPrefs.SetFloat("MasterSlider", masterSlider.value);
        PlayerPrefs.SetFloat("BGMSlider", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXSlider", sfxSlider.value);
        if(brightnessSlider != null)
        {
            PlayerPrefs.SetFloat("BrightnessSlider", brightnessSlider.value);
        }
        var tempColor = brightnessPanel.color;
        tempColor.a = 1 - brightnessSlider.value;
        brightnessPanel.color = tempColor;
        print(brightnessPanel.color.a);
    }
}
