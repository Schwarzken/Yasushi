using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadCurrentScene : MonoBehaviour {
    public int prevScore;

    void Start()
    {
        prevScore = PlayerPrefs.GetInt("Score");
        print("PrevScore" + prevScore);
    }

    public void RestartCurrentScene()
    {
        PlayerPrefs.SetInt("Score", prevScore);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

}
