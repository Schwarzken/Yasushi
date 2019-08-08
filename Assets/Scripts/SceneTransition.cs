using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    public int sceneIndex;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            //PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 500);
            LoadByIndex(sceneIndex);
        }
    }

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
