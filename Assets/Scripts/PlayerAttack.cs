using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : PhysicsObject
{ 
    public Sounds sfx;
    public CameraShake camShake;

    PlayerController playerController;
    Animator anim;
	[SerializeField]bool isRight = true;
    int attackno = 0;
    float lastClickedTime = 0f;
    public float comboDelay;
    public float attackDelay;
    private float originalAttackDelay;
    public float chainDelay;
    private float originalChainDelay;
    bool canAttack = true;
    bool midChain = true;
   // public int ComboCounter = 0;
   // public Text countText;
    float maxComboTime = 1f;
    public float lastHitTime;
    public float chargeTimer;
    public float charged;
    private float originalCharged;
    public bool attacking;
	bool isUppercut;
	bool feverRepeat = true;

    //Stepping
    public float stepDistance;
    public float punchDamage = 10.0f; // *New in V6.5* added to replace damageTaken in EnemyHealthManager.
	public float uppercutDamage = 20.0f;

    //Hitboxes
    public GameObject RightHitbox1;
    public GameObject RightHitbox2;
    public GameObject RightHitbox3;
    public GameObject RightChargebox;

    //Combocounters
    private ComboCounter comboUI;
    PlayerHealth playerHealth;
    PlayerConsume playerConsume;
    [SerializeField] float punchWeight = 0.03f;
    [SerializeField] float finalPunchWeight = 0.06f;
    [SerializeField] float uppercutWeight = 0.12f;

    //Attacking setters
    bool RightAttack1, RightAttack2, RightAttack3, RightCharge = false;
    bool RightCharging;
	bool hasHit;

    //Fever mode
    public bool feverMode = false;
    float feverLength;
    [SerializeField] float feverDuration;
    [SerializeField] int feverRequired;

    // Use this for initialization
    void Start()
    {
		isUppercut = false;
        playerController = GetComponent<PlayerController>();
        originalChainDelay = chainDelay;
        originalAttackDelay = attackDelay;
        originalCharged = charged;
        //gameController = FindObjectOfType<GameController>();
        RightCharging = false;
        attacking = false;
        isRight = true;
        comboUI = GetComponent<ComboCounter>();
        anim = GetComponent<Animator>();
        anim.Play("Player_Idle");
       // ComboCounter = 0;
        canAttack = true;
        //SetCountText();
        RightHitbox1.SetActive(false);
        RightHitbox2.SetActive(false);
        RightHitbox3.SetActive(false);
        RightChargebox.SetActive(false);
        playerHealth = GetComponent<PlayerHealth>();
        playerConsume = GetComponent<PlayerConsume>();
    }

    void Step()
    {
        if (isRight)
        {
			AddForce(stepDistance, 0);
        }
        else
        {
			AddForce(-stepDistance, 0);
        }
    }

    void OnClick()
    {
        if (canAttack == true)
        {
			if (Input.GetKey(GameInputManager.GIM.attack))
            {
				attacking = true;
                chargeTimer += Time.deltaTime;
                if (chargeTimer > 0.5)
                {
					anim.SetBool ("chargingup", true);
                }
            }
        }
        //Checks if player can attack before proceeding
		if (canAttack == true && playerHealth.takeDamage == false && playerConsume.consuming == false)
        {
			if (Input.GetKeyUp(GameInputManager.GIM.attack))
            {
				anim.SetBool ("chargingup", false);
                attacking = true;
                RightCharging = false;
                if (chargeTimer < charged)
				{
					chargeTimer = 0;
                    lastClickedTime = Time.time;
                    attackno++;
                    if (attackno > 3)
                    {
                        attackno = 0;
                    }
					if (attackno == 1)
					{
						RightAttack1 = true;
						canAttack = false;
					}
                    if (attackno == 2)
                    {
                        RightAttack2 = true;
						canAttack = false;
                    }
                    if (attackno == 3)
                    {
                        RightAttack3 = true;
						canAttack = false;
                    }
					Step ();
                }
				else if (chargeTimer >= charged)
                {
					isUppercut = true;
					anim.SetBool ("charged", true);
					anim.SetBool ("chargingup", false);
                    chargeTimer = 0;
                }

            }

        }

        //Plays animations

        if (RightAttack1 == true)
        {
            sfx.PlayPunch1OnCast();
			if (feverMode == false) {
				anim.Play ("Player_Attack1");
			} else if (feverMode == true) {
				anim.Play ("Player_FeverAttack1");
			}
            OnAnimExit();
        }

        attackno = Mathf.Clamp(attackno, 0, 3);
        if (RightAttack2 == true)
        {
            sfx.PlayPunch2OnCast();
			if (feverMode == false) {
				anim.Play ("Player_Attack2");
			} else if (feverMode == true) {
				anim.Play ("Player_FeverAttack2");
			}
            OnAnimExit();
        }
		if (RightAttack3 == true) {
			sfx.PlayPunch3OnCast ();
			if (feverMode == false) {
				anim.Play ("Player_Attack3");
			} else if (feverMode == true) {
				anim.Play ("Player_FeverAttack3");
			}
			OnAnimExit ();
			canAttack = false;
		}
	}

    // Update is called once per frame
    void Update()
    {
        isRight = gameObject.GetComponent<PlayerController>().facingRight;
        //Ensures input and checks if conditions for the next branch is set (MidChain or No)
        OnClick();
        FeverMode();

        if (Time.time - lastClickedTime > comboDelay)
        {
            attackno = 0;
            midChain = false;
        }
        if (canAttack == false)
        {
            if (midChain == false)
            {
                if (Time.time - lastClickedTime > attackDelay)
                {
                    canAttack = true;
					attacking = false;
                }
            }
            else
            {
                if (Time.time - lastClickedTime > chainDelay)
                {
                    canAttack = true;
					attacking = false;
                }
            }
        }

		if (attacking == false) {
			anim.SetBool ("chargingup", false);
			anim.SetBool ("charged", false);
		}

        //Resets Combo Counter
        if (Time.time - lastHitTime > maxComboTime)
        {
            comboUI.ComboReset();
        }

    }
    

    public void OnAnimExit()
    {
        //Occurs only after the animation finishes
        if (RightAttack1 == true)
        {
            RightAttack1 = false;
            midChain = true;
			DeactivateRightHitBox1 ();
        }
        if (RightAttack2 == true)
        {
            RightAttack2 = false;
            midChain = true;
        }
        if (RightAttack3 == true)
        {
            RightAttack3 = false;
            midChain = false;
        }
        if (RightCharge == true)
        {
            RightCharge = false;
            midChain = true;
        }
    }

	List<Collider2D> hitBodies = new List<Collider2D>(20);

    void OnTriggerStay2D(Collider2D coll)
    {
        //Collision program

		if (coll.gameObject.tag == "Enemy" && !hitBodies.Contains (coll)) {
			HealthScript enemyHealthManager = coll.GetComponent<HealthScript> ();
			if (enemyHealthManager.IsVulnerable ()) {
				int knockBackDir = isRight ? 1 : -1;
				//if(coll.GetComponent<EnemyHealthManager>().wasHit == false){
				//if (hasHit == false && !hitBodies.Contains(coll)){
				if (attacking == true) {
					if (RightHitbox1.GetComponent<Collider2D> ().IsTouching (coll) || RightHitbox2.GetComponent<Collider2D> ().IsTouching (coll) || RightHitbox3.GetComponent<Collider2D> ().IsTouching (coll) || RightChargebox.GetComponent<Collider2D> ().IsTouching (coll)) {
						hitBodies.Add (coll);
						comboUI.UpdateCC (enemyHealthManager.ScoreValue);
						if (isUppercut == true) {
							enemyHealthManager.TakeDamage (uppercutDamage, new Vector2 (knockBackDir * 8f, 10f), true, playerHealth, uppercutWeight, coll);
							sfx.PlayPunch2 ();
							camShake.Shake (0.3f, 0.2f);
						} else if (attackno == 0 || attackno == 1 || attackno == 2) {
							enemyHealthManager.TakeDamage (punchDamage, new Vector2 (knockBackDir * 4f, 2f), false, playerHealth, punchWeight, coll); 
							sfx.PlayPunch1 ();
                            if (playerController.facingRight == true)
                            {
                                camShake.ShakeRight(0.1f, 0.2f);
                            }
                            else if (playerController.facingRight == false)
                            {
                                camShake.ShakeLeft(0.1f, 0.2f);
                            }
                        } else if (attackno == 3) {
							enemyHealthManager.TakeDamage (punchDamage, new Vector2 (knockBackDir * 8f, 2f), true, playerHealth, finalPunchWeight, coll);
							sfx.PlayPunch2 ();
							if (playerController.facingRight == true) {
								camShake.ShakeRight (0.2f, 0.2f);
							} else if (playerController.facingRight == false) {
								camShake.ShakeLeft (0.2f, 0.2f);
							}
							//camShake.Shake(0.2f, 0.1f);
						}
						lastHitTime = Time.time;
						hasHit = true;
					}
				}
			}
		} else if (coll.gameObject.tag == "Boss" && !hitBodies.Contains (coll)) {
			HealthScript bossHealth = coll.GetComponentInParent<BossHealth> ();
			if (attacking == true) {
				if (RightHitbox1.GetComponent<Collider2D> ().IsTouching (coll) || RightHitbox2.GetComponent<Collider2D> ().IsTouching (coll) || RightHitbox3.GetComponent<Collider2D> ().IsTouching (coll) || RightChargebox.GetComponent<Collider2D> ().IsTouching (coll)) {
					hitBodies.Add (coll);
					comboUI.UpdateCC (bossHealth.ScoreValue);
					if (attackno == 0 || attackno == 1 || attackno == 2) {
						bossHealth.TakeDamage (punchDamage, Vector2.zero, false, playerHealth, punchWeight, coll);
						sfx.PlayPunch1 ();
					} else if (attackno == 3) {
						bossHealth.TakeDamage (punchDamage, Vector2.zero, false, playerHealth, finalPunchWeight, coll);
						sfx.PlayPunch2 ();
						if (playerController.facingRight == true) {
							camShake.ShakeRight (0.2f, 0.2f);
						} else if (playerController.facingRight == false) {
							camShake.ShakeLeft (0.2f, 0.2f);
						}
					}
					lastHitTime = Time.time;
					hasHit = true;
				}
			}
		}
    }


    public void ActivateRightHitBox1()
	{
		hitBodies.Clear ();
        RightHitbox1.SetActive(true);
		hasHit = false;
		attacking = true;
    }

    public void DeactivateRightHitBox1()
    {
        RightHitbox1.SetActive(false);
    }

    public void ActivateRightHitBox2()
	{
		hitBodies.Clear ();
        RightHitbox2.SetActive(true);
		hasHit = false;
		attacking = true;
    }

    public void DeactivateRightHitBox2()
    {
        RightHitbox2.SetActive(false);
    }

    public void ActivateRightHitBox3()
    {
		hitBodies.Clear ();
        RightHitbox3.SetActive(true);
		hasHit = false;
		attacking = true;
    }

    public void DeactivateRightHitBox3()
    {
        RightHitbox3.SetActive(false);
    }
   
    public void ActivateRightChargebox()
	{
		hitBodies.Clear ();
        RightChargebox.SetActive(true);
		hasHit = false;
		attacking = true;
		isUppercut = true;
    }
    public void DeactivateRightChargebox()
    {
        RightChargebox.SetActive(false);
		isUppercut = false;
    }

    public void FeverMode()
    {
		if (comboUI.comboCount != 0)
		{
			if (comboUI.comboCount % feverRequired == 0)
			{
				feverMode = true;
				feverLength = feverDuration;
				attackDelay = originalAttackDelay / 2;
				chainDelay = originalChainDelay / 2;
				charged = originalCharged / 2;
				if(feverRepeat == true)
				{
					sfx.PlayFever();
					feverRepeat = false;
				}
			}
		}
		if (feverLength > 0)
		{
			feverLength -= Time.deltaTime;
		}
		else if (feverLength <= 0)
		{
			feverMode = false;
			attackDelay = originalAttackDelay;
			charged = originalCharged;
			feverRepeat = true;
		}
		if(feverMode == true)
		{
			anim.SetBool("fever", true);
		}
		else{
			anim.SetBool("fever", false);
		}
    }

	public void NotAttacking()
	{
		attacking = false;
	}

	public void NotCharged()
	{
		anim.SetBool ("charged", false);
	}
}


