using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public PlayerAttack playerAttack;
    public TextMeshProUGUI scoreText;
    public int subScore;
    private int finalScore;
    private int combo;
    private float searchCountdown = 1f;
    private int score;
    void Start()
    {
        //PlayerPrefs.SetInt("Score",0);
        score = 0;
        UpdateScore();
        Time.timeScale = 1f;
    }

    public void SetCombo(int comboValue)
    {
        combo = comboValue;
    }

    public void AddSubScore(int subValue)
    {
        subScore += subValue;
    }

    public void AddScore()
    {
        finalScore = subScore * combo;
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + finalScore);
        combo = 0;
        subScore = 0;
        //score += newScoreValue;
        //UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = score + "";
    }
}
