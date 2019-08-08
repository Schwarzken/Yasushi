using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    float shakeAmount = 0;
   // public float length = 0;
    //Vector3 rotation;

    public void KnifeShake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    public void KnifeDrop()
    {
        this.GetComponent<InteractableObject>().enabled = true;
    }

    void DoShake()
    {
        if (shakeAmount > 0)
        {
            // float randomRotate = Random.Range(0f, shakeAmount);
            //rotation.z = randomRotate;
            this.transform.Rotate(0,0,Random.Range(0,shakeAmount));
            this.transform.Rotate(0,0,Random.Range(0,-shakeAmount));
        }
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        this.transform.rotation = Quaternion.identity;
    }
}
