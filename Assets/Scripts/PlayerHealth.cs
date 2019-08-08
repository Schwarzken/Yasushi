using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : HealthScript {
    public CameraShake camShake;
    public Sounds sfx;
    public GameObject gameOverPanel;
    public Slider healthSlider;
    PlayerAttack playerFever;
	ComboCounter comboUI;
	Animator anim;
	PlayerAttack hitBoxes;
	PlayerConsume consuming;
    EnemyAI[] enemyAI;
    Timer timer;
	[HideInInspector] public bool takeDamage;
    bool isDead;
    // bool damaged;
    PlayerSkill skillScript;
    PlayerController controllerScript;
    Animator animator;

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        enemyAI = FindObjectsOfType<EnemyAI>();
		hitBoxes = GetComponent<PlayerAttack> ();
        currentHealth = MaxHealth;
        playerFever = GetComponent<PlayerAttack>();
		comboUI = GetComponent<ComboCounter> ();
		consuming = GetComponent<PlayerConsume> ();
        skillScript = GetComponent<PlayerSkill>();
        controllerScript = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(currentHealth<=0)
        {
            StartCoroutine("Die");
        }
        healthSlider.value = currentHealth;
    }

    public override void FreezeFrames(float freezeDelay)
    {
        // Disable the behavior scripts and the animator for 0.06
        playerFever.enabled = false; // Player attack script
        consuming.enabled = false; // Player consume script
        skillScript.enabled = false;
        controllerScript.enabled = false;
        animator.enabled = false;
        Invoke("UnFreezeFrames", freezeDelay);
    }

    protected override void UnFreezeFrames()
    {
        // Enable the behavior scripts and the animator, again.
        animator.enabled = true;
        skillScript.enabled = true;

        if (!(skillScript.isCharging || skillScript.isDodging || skillScript.isSpinning))
        { // No need to enable the following scripts if player is using skills, because they will be enabled when the skill has finished.
            playerFever.enabled = true;
            consuming.enabled = true;
            controllerScript.enabled = true;
        } 
    }

    public override void TakeDamage(float amount, Vector2 knockBackForce, bool flinch, HealthScript attacker = null, float freezeDelay = 0.0f, Collider2D gotHitCollider = null)
    {
		if (playerFever.feverMode == false && consuming.consuming == false) {
			currentHealth -= amount;

            GetComponent<PlayerController>().AddForce(knockBackForce.x, knockBackForce.y);

            if (flinch)
            {
                anim.Play("Player_Flinch");
                GetComponent<PlayerSkill>().resetSkillTimer();
            }

            if (gotHitCollider)
            { // Pass attacker (their health scripts) and 'this object's collider that was hit' as argument for hiteffect instantiation.
                Vector3 hitPos = gotHitCollider.bounds.ClosestPoint(new Vector3(attacker.transform.position.x, attacker.transform.position.y + 0.86f, 0)); // 0.86f is the shoulder height of yasushi.
                GameObject hitEffectInst = Instantiate(hitEffect, hitPos, attacker.transform.rotation);
                Vector3 hitEffectScale = hitEffectInst.transform.localScale;
                hitEffectScale.x *= Mathf.Sign(attacker.transform.position.x - gotHitCollider.transform.position.x);
                hitEffectInst.transform.localScale = hitEffectScale;
            }

            if (attacker != null && freezeDelay > 0)
            {
                attacker.FreezeFrames(freezeDelay);
                FreezeFrames(freezeDelay);
            }

            comboUI.ComboReset ();
			hitBoxes.DeactivateRightHitBox1 ();
			hitBoxes.DeactivateRightHitBox2 ();
			hitBoxes.DeactivateRightHitBox3 ();
			hitBoxes.DeactivateRightChargebox ();
			takeDamage = true;
			GetComponent<PlayerAttack> ().attacking = false;
			GetComponent<PlayerAttack> ().chargeTimer = 0;
			anim.SetBool ("chargingup", false);
			anim.SetBool ("charged", false);
            camShake.Shake(0.1f, 0.1f);
            sfx.PlayFlinch();
        } else {
			return;
		}
    }

    public void ReplenishHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > MaxHealth)
            currentHealth = MaxHealth;

        healthSlider.value = currentHealth;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name == "KillZone")
        {
            currentHealth = 0;
        }
    }

    IEnumerator Die()
    {
		anim.Play ("Player_Die");
        gameOverPanel.gameObject.SetActive(true);
        this.GetComponent<PlayerController>().enabled = false;
        for(int i=0;i<enemyAI.Length;i++)
        {
            if(enemyAI[i] != null)
            {
                enemyAI[i].enabled = false;
            }
        }
        timer.StopCoroutines();
        //SceneManager.LoadScene("Main");
        yield return null;
    }

	public void TakeDamageOff()
	{
		if (takeDamage == true) {
			takeDamage = false;
		}
	}
}
