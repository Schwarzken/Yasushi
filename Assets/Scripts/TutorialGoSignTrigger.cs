using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGoSignTrigger : MonoBehaviour {

    public GameObject goSign;
    public GameObject goon;

    void Start()
    {
        goon.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            goon.SetActive(true);
            goSign.SetActive(false);
        }
    }
}
