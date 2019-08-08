using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSplash : MonoBehaviour {

    private Animator animator;
    public float splashInterval = 4f;
    float nextSplashTime;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        nextSplashTime = Time.time + splashInterval;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - nextSplashTime > 0)
        {
            animator.SetBool("Splash", true);
            StartCoroutine(ResetSplash(animator));
        }
    }

    IEnumerator ResetSplash(Animator anim)
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Splash", false);
    }

    /*public void SetSplashFalse()
    {
        animator.SetBool("Splash", false);
    }*/
}
