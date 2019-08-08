using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour {

    public int score = 0;
    public LevelManager levelManager;
	// Use this for initialization

	void Start () {
        levelManager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name == "Player")
        {
            ScoreScript.scoreValue += score;
            levelManager.RespawnPlayer();
        }
    }
}
