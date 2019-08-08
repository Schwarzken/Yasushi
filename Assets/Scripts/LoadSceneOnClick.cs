using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    public SlideTransition slideTransition;

    private int SceneIndex;

    public void LoadByIndex(int sceneIndex)
    {
        SceneIndex = sceneIndex;
        if(sceneIndex == 1)
        {
            PlayerPrefs.SetInt("Score", 0);
        }
        Time.timeScale = 0f;
        slideTransition.SlideIn();
    }

    public void ActualLoad()
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
