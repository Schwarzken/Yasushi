using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGoon2Activator : MonoBehaviour {

    public GameObject goon2;

    private void Start()
    {
        goon2.GetComponent<EnemyAI>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            if(goon2 != null)
            {
                goon2.gameObject.SetActive(true);
                goon2.GetComponent<EnemyAI>().enabled = true;
                if(this.name == "Goon2")
                {
                    goon2.GetComponent<EnemyHealthManager>().canDie = false;
                }
            }
        }
    }
}
