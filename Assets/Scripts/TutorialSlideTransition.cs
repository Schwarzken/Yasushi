using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSlideTransition : MonoBehaviour
{

    public GameObject slideTransition;
    private Animator anim;

    TutorialSkip tutorialSkip;

    // Use this for initialization
    void Start()
    {
        tutorialSkip = FindObjectOfType<TutorialSkip>();
        anim = slideTransition.GetComponent<Animator>();
        anim.enabled = false;
    }

    public void SlideIn()
    {
        anim.enabled = true;
        anim.Play("slideTransitionFromRightTutorial");
    }

    public void ActualResume()
    {
        Time.timeScale = 1f;
    }

    public void TutorialSkip()
    {
        tutorialSkip.PlayerTeleport();
    }
}
