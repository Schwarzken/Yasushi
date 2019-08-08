using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight2Manager : MonoBehaviour {

    Sounds sounds;
    InitiateDialog initiateDialog;
    WaveSpawner waveSpawner;
    public GameObject knivesHolder;
    public GameObject chukaTutorial;
    public GameObject tornadoTutorial;
    bool preventRepeat = false;
    public GameObject timer;
    public Timer Timer;
    public GameObject waveUI;
    public GameObject chuka;
    public bool ChukaIsAlive = true;
    public OilSplash oilSplashLeft;
    public OilSplash oilSplashRight;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1f;
        preventRepeat = false;
        waveSpawner = GetComponent<WaveSpawner>();
        initiateDialog = GetComponent<InitiateDialog>();
        initiateDialog.StartDialog();
        sounds = GetComponent<Sounds>();
        sounds.PlaySound2();
        StartCoroutine(ChukkaDelay(sounds));
		timer.gameObject.SetActive (false);
        chuka.GetComponent<EnemyAI>().enabled = false;
	}

    private void Update()
    {
        if(initiateDialog.done == true && initiateDialog.firstDone == true)
        {
            if(ChukaIsAlive == true)
            {
                    chuka.GetComponent<EnemyAI>().enabled = true;
                
            }   
            else if (ChukaIsAlive == false)
            {
                if (preventRepeat == false)
                {
                    oilSplashLeft.enabled = true;
                    oilSplashRight.enabled = true;
                    knivesHolder.GetComponent<Collider2D>().enabled = true;
                    chukaTutorial.gameObject.SetActive(false);
                    tornadoTutorial.gameObject.SetActive(true);
                    waveSpawner.enabled = true;
                    timer.gameObject.SetActive(true);
                    StartCoroutine(TimerSpawn(Timer));
                    waveUI.gameObject.SetActive(true);
                    preventRepeat = true;
                }
            }
        }
    }

	IEnumerator TimerSpawn(Timer timer)
	{
		yield return new WaitForSeconds(0f);
		timer.gameObject.SetActive (true);
		StartCoroutine (TimerDelay (Timer));
	}

	IEnumerator TimerDelay(Timer timer)
	{
		yield return new WaitForSeconds (5f);
		timer.StartTimer();
	}
    IEnumerator ChukkaDelay(Sounds sounds)
    {
        yield return new WaitForSeconds(0.5f);
        sounds.PlayChukkaHey();
    }
}
