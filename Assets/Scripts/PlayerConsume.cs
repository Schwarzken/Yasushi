using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerConsume : MonoBehaviour
{
    public Sounds sfx;
    Fight1Manager f1Manager;
    Fight2Manager f2Manager;
    EnemyHealthManager consumeCheck;
    PlayerHealth healthCheck;
    public int recoveredHealth;
    public GameObject RightConsumeBox;
    float consumeLength = 0;
    public float consumeDuration;
	public bool consuming = false;
	private Animator anim;
    private ComboCounter comboUI;
	PlayerSkill skillScript;
	public float cooldown; // The cooldown duration of consume skill.
	public IconCoolDown consumeIcon;

    //For Special Enemies Checking
    // public GameObject tempura1;
    // public GameObject makiroll1;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            f1Manager = FindObjectOfType<Fight1Manager>();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            f2Manager = FindObjectOfType<Fight2Manager>();
        }
        comboUI = GetComponent<ComboCounter>();
        consumeCheck = GetComponent<EnemyHealthManager>();
		consuming = false;
        healthCheck = gameObject.GetComponent<PlayerHealth>();
        RightConsumeBox.SetActive(false);
        anim = GetComponent<Animator> ();
		skillScript = GetComponent<PlayerSkill> ();
		GameObject iconsHolder = GameObject.Find("HUDCanvas/AbilityIcons");
		consumeIcon = iconsHolder.transform.GetChild(4).GetComponent<IconCoolDown>();
    }

    void Update()
    {
		if (Input.GetKeyDown(GameInputManager.GIM.consume) && consumeIcon.isSkillUsable())
        {
			Consume ();
        }

        if (consumeLength > 0)
        {
            consumeLength -= Time.deltaTime;
        }
        else if (consumeLength <= 0)
        {
			consuming = false;
			DeactivateRightConsumeBox();
        }
        
    }

	// 6/4/2018 - Removed Checking and UpdateUI function. Sorry!
	// No need to check whether a skill is casting, because consumeScript will be disabled when a skill is used, and enabled after the skill ends.
    void Consume()
    { 
		if (GetComponent<PlayerAttack> ().attacking == false && GetComponent<PlayerController> ().jumping == false) {
			if (consumeLength <= 0) {
				consuming = true;
				consumeLength = consumeDuration;
				anim.Play ("Player_Consume");
				ActivateRightConsumeBox ();
				consumeIcon.setCoolDown (cooldown);
				//canConsume = false;
				//nextUse = consumeCooldown;
			}
		}
    }

    void ActivateRightConsumeBox()
    {
        RightConsumeBox.SetActive(true);
    }

    void DeactivateRightConsumeBox()
    {
        RightConsumeBox.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
			if (consuming == true) {
				consumeCheck = coll.gameObject.GetComponent<EnemyHealthManager> ();
				if (consumeCheck.canConsume == true) {
                    healthCheck.ReplenishHealth(recoveredHealth); // new in v6.4.6
                    if (coll.GetComponent<TempuraAI>()) // The consumed target is a tempura.
                        if (!skillScript.chargeIcon.isActiveAndEnabled)
                        {
                            skillScript.chargeIcon.gameObject.SetActive(true);
                            f1Manager.TempuraIsAlive = false;
                        }   
                    if (coll.GetComponent<MakiRollAI>()) // The consumed target is a maki roll.
                        if (!skillScript.dodgeIcon.isActiveAndEnabled)
                        {
                            skillScript.dodgeIcon.gameObject.SetActive(true);
                            f1Manager.MakiIsAlive = false;
                        }
                    if (coll.GetComponent<ChukaIdakoAI>()) // The consumed target is a chuka idako.
                        if (!skillScript.tornadoIcon.isActiveAndEnabled)
                        {
                            skillScript.tornadoIcon.gameObject.SetActive(true);
                            f2Manager.ChukaIsAlive = false;
                        }
                    comboUI.UpdateCC(consumeCheck.ScoreValue);
                    sfx.PlayConsume();
                    Destroy (coll.gameObject);
				}
			}
        }
    }
}
