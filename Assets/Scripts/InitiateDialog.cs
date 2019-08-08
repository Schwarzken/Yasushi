using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitiateDialog : MonoBehaviour {
    
    public GameObject sprite1;
    public GameObject sprite2;
    public GameObject goon2;
    public GameObject dialogBox;
    public GameObject waveUI;
    public GameObject timer;
    public GameObject goSign;
    public bool done;
    //public string startingDialog1;
    //public string startingDialog2;
    public bool firstDone = false;
    WaveSpawner waveSpawner;
    TextTyper textTyper; 
    PlayerController playerController;
	[SerializeField] PlayerSkill playerSkill;
	[SerializeField] PlayerConsume playerConsume;
    public PauseMenu pauseMenu;
    Timer Timer;
    EnemyAI enemyAI;
    TutorialSkip tutorialSkip;
    BossAI bossAI;

    [System.Serializable]
    public class DialogLines
    {
        public string line;
    }

    public DialogLines[] lines;
    public DialogLines[] lines2;
    private int nextLine = 0;
    private int currentLine = 0;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            tutorialSkip = GetComponent<TutorialSkip>();
        }
        textTyper = GetComponent<TextTyper>();
    }

    // Use this for initialization
    public void StartDialog() {
        done = false;
        if (SceneManager.GetActiveScene().buildIndex != 4 && SceneManager.GetActiveScene().buildIndex != 5)
        {
            enemyAI = FindObjectOfType<EnemyAI>();
            enemyAI.enabled = false;
        }
        Timer = FindObjectOfType<Timer>();
        playerController = FindObjectOfType<PlayerController>();
        waveSpawner = GetComponent<WaveSpawner>();
        if(waveUI != null)
        {
            waveUI.SetActive(false);
        }
        textTyper = GetComponent<TextTyper>();
        dialogBox.SetActive(true);
        if(sprite2 != null)
        {
            sprite2.gameObject.SetActive(false);
        }
        if(sprite1 != null)
        {
            sprite1.gameObject.SetActive(true);
        }
        playerController.enabled = false;
        StartingDialog();
	}
	
    void StartingDialog()
    {
        if(firstDone == false)
        {
            StartCoroutine(UpdateText(lines[nextLine]));
        }
        else
        {
            StartCoroutine(UpdateText(lines2[nextLine]));
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            if (pauseMenu.GameIsPaused == false)
            {
                if (done == false)
                {
                    if (SceneManager.GetActiveScene().buildIndex == 1)
                    {
                        if (tutorialSkip.skip == false)
                        {
                            if (textTyper.messageFinished == true)
                            {
                                DialogFinished();
                                textTyper.StopTypeTextCoroutine();
                                if (firstDone == false)
                                {
                                    StartCoroutine(UpdateText(lines[nextLine]));
                                }
                                else
                                {
                                    if (lines2.Length != 0)
                                    {
                                        StartCoroutine(UpdateText(lines2[nextLine]));
                                    }
                                }
                            }
                            else if (textTyper.messageFinished == false)
                            {
                                print("asoifnaf");
                                textTyper.StopAllCoroutines();
                                if (firstDone == false)
                                {
                                    StartCoroutine(FinishText(lines[nextLine]));
                                }
                                else
                                {
                                    if (lines2.Length != 0)
                                    {
                                        StartCoroutine(FinishText(lines2[nextLine]));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (textTyper.messageFinished == true)
                        {
                            DialogFinished();
                            textTyper.StopTypeTextCoroutine();
                            if (firstDone == false)
                            {
                                StartCoroutine(UpdateText(lines[nextLine]));
                            }
                            else
                            {
                                if (lines2.Length != 0)
                                {
                                    StartCoroutine(UpdateText(lines2[nextLine]));
                                }
                            }
                        }
                        else if (textTyper.messageFinished == false)
                        {
                            print("asoifnaf");
                            textTyper.StopAllCoroutines();
                            if (firstDone == false)
                            {
                                StartCoroutine(FinishText(lines[nextLine]));
                            }
                            else
                            {
                                if (lines2.Length != 0)
                                {
                                    StartCoroutine(FinishText(lines2[nextLine]));
                                }
                            }
                        }
                    }
                }
            }

		if (done == false) {
			playerSkill.enabled = false;
			playerConsume.enabled = false;
		} else {
			playerSkill.enabled = true;
			playerConsume.enabled = true;
		}
	}

    IEnumerator FinishText(DialogLines _line)
    {
        print("FinishTextCoroutine");
        textTyper.FinishDialog(_line.line);
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator UpdateText(DialogLines _line)
    {
        textTyper.PlayDialog(_line.line);
        yield return new WaitForSeconds(0.1f);
    }

    void DialogFinished()
    {
        if (firstDone == false)
        {
            if (nextLine + 1 > lines.Length - 1)
            {
                dialogBox.SetActive(false);
                playerController.enabled = true;
                if (goSign != null)
                {
                    goSign.SetActive(true);
                }
				if (enemyAI != null) {
					enemyAI.enabled = true;
				}
                //for Main only
                if (goon2 != null)
                {
                    goon2.GetComponent<EnemyAI>().enabled = false;
                }
                done = true;
                firstDone = true;
                if (SceneManager.GetActiveScene().buildIndex == 5)
                {
                    bossAI = FindObjectOfType<BossAI>();
                    bossAI.enabled = true;
                }
                currentLine = 0;
                nextLine = 0;
            }
            else
            {
                nextLine++;
                currentLine = nextLine - 1;
            }
        }
        else if (firstDone == true)
        {
            if (nextLine + 1 > lines2.Length - 1)
            {
                dialogBox.SetActive(false);
                playerController.enabled = true;
                if (goSign != null)
                {
                    goSign.SetActive(true);
                }
				if (enemyAI != null) {
					enemyAI.enabled = true;
				}
                
                done = true;
                currentLine = 0;
                nextLine = 0;
            }
            else
            {
                nextLine++;
                currentLine = nextLine - 1;
            }
        }
    }

    public void SwitchSprites()
    {
        sprite1.gameObject.SetActive(false);
        sprite2.gameObject.SetActive(true);
    }
}
