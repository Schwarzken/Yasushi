using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDropActivator : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag== "Player")
        {
            this.GetComponent<KniveDrop>().enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            this.GetComponent<KniveDrop>().enabled = false;
        }
    }
}
