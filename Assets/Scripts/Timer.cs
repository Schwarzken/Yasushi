using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int timeLeft;
    public int almostDone;
    public Text countdownText;
    public GameObject gameOverPanel;
    bool activated = false;
    PauseMenu pauseMenu;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    public void StartTimer()
    {
        if(activated == false)
        {
            StartCoroutine(LoseTime());
            activated = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        countdownText.text = timeLeft + "";

        if (timeLeft <= 0)
        {
            StopCoroutine(LoseTime());
            gameOverPanel.gameObject.SetActive(true);
        }
        if(timeLeft <= almostDone)
        {
			animator.SetBool ("almostDone", true);
        }
    }

    public void StopCoroutines()
    {
        StopAllCoroutines();
    }

    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }
}