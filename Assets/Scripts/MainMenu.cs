using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    public GameObject mainMenu;
    public GameObject menuButtons;
    public GameObject logo;
    public Sounds sounds;
    
   
    private Animator anim;
    void Start()
    {
        Time.timeScale = 1f;
        sounds.PlayOkami();
        mainMenu.gameObject.SetActive(true);
        anim = mainMenu.GetComponent<Animator>();
        anim.enabled = true;
        anim.Play("OpenMenu");
    }

    public void SetButtonsActive()
    {
        logo.gameObject.SetActive(true);
        menuButtons.gameObject.SetActive(true);
    }
}
