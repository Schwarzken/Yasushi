using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSkip : MonoBehaviour
{
    //public GameObject slideTransition;
    public GameObject tutorialSkipPanel;
    public GameObject player;
    public GameObject skipSpawn;
    public GameObject dialogPanel;
    public bool skip = true;
    public TutorialSlideTransition tutSlideTransition;
    public PlayerController playerController;
    public Sounds sounds;

    //private Animator anim;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 0f;
        tutorialSkipPanel.gameObject.SetActive(true);
        dialogPanel.gameObject.SetActive(false);
        playerController.enabled = false;
    }

    public void SkipYes()
    {
        tutSlideTransition.SlideIn();
        tutorialSkipPanel.gameObject.SetActive(false);
        sounds.PlaySamurai();
        sounds.fading2 = true;
        StartCoroutine(AfterFade(sounds));
    }

    public void PlayerTeleport()
    {
        player.transform.position = skipSpawn.transform.position;
        playerController.enabled = true;
    }

    public void SkipNo()
    {
        tutorialSkipPanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
        skip = false;
        sounds.PlayKamiiHello();
    }

    public static IEnumerator AfterFade(Sounds bgmM)
    {
        yield return new WaitForSeconds(1.2f);
        print("AfterFade");
        bgmM.fading1 = false;
        bgmM.fadeDone1 = true;
    }
}
