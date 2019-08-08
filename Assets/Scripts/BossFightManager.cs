using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour {

	Sounds sound;
    BossAI bossAI;
	[SerializeField]BossHealth bossHealth;
	[SerializeField]GameObject gameComplete;
	[SerializeField]GameObject player;
	int activeCount = 0;
	public StageComplete stageComplete;
    InitiateDialog initiateDialog;

    // Use this for initialization
    void Start () {
		sound = GetComponent<Sounds> ();
		sound.PlayBossTheme ();
        initiateDialog = GetComponent<InitiateDialog>();
        initiateDialog.StartDialog();
        //sound.PlayBossDialog();
        StartCoroutine(BossDialogDelay(sound));
        bossAI = FindObjectOfType<BossAI>();
        bossAI.enabled = false;
    }

	void LateUpdate()
	{
		if (bossHealth.isDead == true) {
			if (activeCount < 1) {
				ActivateGameComplete ();
				activeCount++;
			}
		}
	}
	void ActivateGameComplete()
	{
		stageComplete.ComputeFinalScore();
	}

    IEnumerator BossDialogDelay(Sounds sound)
    {
        yield return new WaitForSeconds(0.5f);
        sound.PlayBossDialog();
    }
}
