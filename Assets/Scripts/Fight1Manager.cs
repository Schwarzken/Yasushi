using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight1Manager : MonoBehaviour {
    public GameObject tempura;
    public GameObject makiRoll;
    public GameObject waveUI;
    public GameObject tempuraTutorial;
    public GameObject makiTutorial;
    public GameObject rollTutorial;
    public GameObject timer;
    EnemyAI enemyAI;
    InitiateDialog initiateDialog;
    WaveSpawner waveSpawner;
    public Timer Timer;
    public Sounds bgmManager;
    private float searchCountdown = 1f;
    private bool isDone;
    private bool preventRepeat;
    public bool TempuraIsAlive;
    public bool MakiIsAlive;
    

    // Use this for initialization
    void Start () {
        Time.timeScale = 1f;
        TempuraIsAlive = true;
        MakiIsAlive = true;
        isDone = false;
        preventRepeat = false;
        makiRoll.SetActive(false);
        enemyAI = FindObjectOfType<EnemyAI>();
        enemyAI.enabled = false;
        initiateDialog = GetComponent<InitiateDialog>();
        waveSpawner = GetComponent<WaveSpawner>();
        initiateDialog.StartDialog();
        tempuraTutorial.gameObject.SetActive(true);
        bgmManager.PlayDialog();
        bgmManager.PlayTempuraKora();
		timer.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if(isDone == false)
        {
            if(TempuraIsAlive == false)
            {
                if (preventRepeat == false)
                {
                    tempuraTutorial.gameObject.SetActive(false);
                    makiTutorial.gameObject.SetActive(true);
                    makiRoll.SetActive(true);
                    initiateDialog.StartDialog();
                    initiateDialog.SwitchSprites();
                    bgmManager.PlayMakiAniki();
                    preventRepeat = true;
                    return;
                }
            }
            if (MakiIsAlive == false)
            {
                if (waveUI != null)
                {
                    waveUI.SetActive(true);
                }
                bgmManager.PlayWave();
                bgmManager.fading1 = true;
                StartCoroutine(AfterFade(bgmManager));
                waveSpawner.enabled = true;
                makiTutorial.gameObject.SetActive(false);
                rollTutorial.gameObject.SetActive(true);
                StartCoroutine(TimerSpawn(Timer));
                isDone = true;
                return;
            }
        }
        else
        {
            return;
        }
	}

    public static IEnumerator AfterFade(Sounds bgmM)
    {
        yield return new WaitForSeconds(1.2f);
        print("AfterFade");
        bgmM.fading1 = false;
        bgmM.fadeDone1 = true;
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
}
