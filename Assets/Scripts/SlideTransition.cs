using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlideTransition : MonoBehaviour {

    public GameObject slideTransition;
    private Animator anim;
    public LoadSceneOnClick loadSceneOnClick;

	// Use this for initialization
	void Start () {
        anim = slideTransition.GetComponent<Animator>();
        anim.enabled = false;
	}
	
	public void SlideIn()
    {
        anim.enabled = true;
        anim.Play("slideTransitionFromRight");
    }

    public void ActualResume()
    {
        Time.timeScale = 1f;
    }

    public void ActualLoad()
    {
        loadSceneOnClick.ActualLoad();
    }
}
