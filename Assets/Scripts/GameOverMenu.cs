using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour {

    public Animator anim;
    public GameObject gameOverMenuUI;
    public GameObject gameOverButtons;
    
    // Update is called once per frame
    void Start()
    {
        anim.SetBool("IsPaused", true);
        Time.timeScale = 0f;
    }

    public void SetPauseButtonsActive()
    {
        gameOverButtons.gameObject.SetActive(true);
    }

    public void SetPauseButtonsInactive()
    {
        gameOverButtons.gameObject.SetActive(false);
    }

    public void ActualResume()
    {
        gameOverMenuUI.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
