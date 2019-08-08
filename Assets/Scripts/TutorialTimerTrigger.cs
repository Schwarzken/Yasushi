using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTimerTrigger : MonoBehaviour {

   // public GameObject timer;
    Timer Timer;

	// Use this for initialization
	void Start () {
        Timer = FindObjectOfType<Timer>();
    }

   void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //timer.gameObject.SetActive(true);
            Timer.StartTimer();
        }
    }
}
