using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject player;
    public Animator anim;
    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject pauseButtons;
    public GameObject skipButtons;
    public GameObject skipPanel;
    public GameObject dialogP;
    //public TutorialSkip tutorialSkip;
    //public PlayerController playerController;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(SceneManager.GetActiveScene().buildIndex != 0)
            {
                if (GameIsPaused)
                {
                    Resume();

                }
                else
                {
                    Pause();
                }
            }
        }
        if(GameIsPaused)
        {
            player.GetComponent<PlayerController>().enabled = false;
        }
    }

    public void Resume()
    {
        if (skipButtons != null)
        {
            skipButtons.gameObject.SetActive(true);
        }
        
        anim.SetBool("IsPaused", false);
       
        GameIsPaused = false;
        if(skipPanel != null)
        {
            if (skipPanel.gameObject.activeInHierarchy == false)
            {
                player.GetComponent<PlayerController>().enabled = true;
            }
        }
        else
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
        if (dialogP.gameObject.activeInHierarchy == true)
        {
            player.GetComponent<PlayerController>().enabled = false;
        }
    }

    public void Pause()
    {
        if(skipButtons != null)
        {
            skipButtons.gameObject.SetActive(false);
        }
        player.GetComponent<PlayerController>().enabled = false;
        anim.SetBool("IsPaused", true);
        pauseMenuUI.gameObject.SetActive(true);
        pauseButtons.gameObject.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void OptionsPause()
    {
        Time.timeScale = 0f;
    }

    public void SetPauseButtonsActive()
    {
        pauseButtons.gameObject.SetActive(true);
    }

    public void SetPauseButtonsInactive()
    {
        pauseButtons.gameObject.SetActive(false);
    }

    public void ActualResume()
    {
        pauseMenuUI.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
   
}
