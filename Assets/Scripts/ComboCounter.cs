using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboCounter : GameController {

	public int comboCount;
	public GameObject comboCounter;
	private GameController gameController;
	public Text SubScore;
	public Text countText;
	public float comboDelay;
	public float maxComboTime = 1.0f;
	private PlayerAttack attackScript;
    float lastHitTime;

    // Use this for initialization
    void Start () {
		attackScript = GetComponent<PlayerAttack> ();
		gameController = FindObjectOfType<GameController> ();
	}

    void Update()
    {
        if (Time.time - lastHitTime > maxComboTime)
            ComboReset();
    }

    // Update is called once per frame
    public void UpdateCC (int subscore) {
		comboCount++;
        lastHitTime = Time.time;
		SetCountText ();
        AddSubScore(subscore); // was missing, this is a function in GameController script but was not called.
        SubScore.text = subScore.ToString(); // was missing
		comboCounter.SetActive (true);
	}

    void SetSubScoreText()
    {
        SubScore.text = subScore.ToString(); // was missing
    }

    public void ComboReset()
	{
        SetCombo(comboCount);
        AddScore();
        comboCount = 0;
		comboCounter.SetActive(false);
		//System.Convert.ToInt32(SubScore.text)
	}

	void SetCountText()
	{
		countText.text = "x" + comboCount.ToString() + " HIT!";
	}

}
