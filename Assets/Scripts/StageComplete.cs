using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageComplete : MonoBehaviour {

    public GameObject stageCompletePanel;
    public Text scoreEarned;
    public Text timer;
    public int multiplier;
    int timerInt;
    public static int bonusScore;
    PlayerController playerController;
    EnemyAI enemyAI;
    ReloadCurrentScene reloadCurrentScene;
    Timer Timer;
    public Sounds sounds;
    public ComboCounter comboCounter;

    void Start()
    {
        reloadCurrentScene = FindObjectOfType<ReloadCurrentScene>();
        enemyAI = FindObjectOfType<EnemyAI>();
        playerController = FindObjectOfType<PlayerController>();
        Timer = FindObjectOfType<Timer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            ComputeBonusScore();
        }
    }

    public void ComputeBonusScore()
    {
        Timer.StopCoroutines();
        playerController.enabled = !playerController.enabled;
        stageCompletePanel.SetActive(true);
        sounds.PlayYOO();
        timerInt = int.Parse(timer.text);
        bonusScore = timerInt * multiplier;
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + bonusScore);
        scoreEarned.text = PlayerPrefs.GetInt("Score") - reloadCurrentScene.prevScore + "";
        Time.timeScale = 0f;
        //enemyAI.enabled = false;
    }

	public void ComputeFinalScore()
	{
		stageCompletePanel.SetActive (true);
		sounds.PlayYOO ();
        comboCounter.ComboReset();
		scoreEarned.text = PlayerPrefs.GetInt("Score") + "";
	}
}
