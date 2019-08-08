using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : PhysicsObject
{
    Sounds sfx;
    public CameraShake camShake;

    PlayerAttack attackScript;
    PlayerController controllerScript;
    PlayerHealth healthScript;
    PlayerConsume consumeScript;

    ComboCounter comboScript;
    Rigidbody2D rb;
    Animator anim;

    bool bossChargeHittable;
	[SerializeField] float chargeWeight = 0.08f;
	[SerializeField] float tornadoWeight = 0.02f;

    public float bossChargeDamage;
    public IconCoolDown chargeIcon;
    public IconCoolDown dodgeIcon;
    public IconCoolDown tornadoIcon;
    public GameObject ChargeDustRight;
    public GameObject ChargeDustLeft;
    public Transform playerFacing;
    int instantiateCount;

    float skillsTimer = 0.0f; // A skillsTimer variable is shared between the three skills.

    void Start()
    {
        sfx = FindObjectOfType<Sounds>();
        bossChargeHittable = true;
        anim = GetComponent<Animator>();
        attackScript = GetComponent<PlayerAttack>();
        comboScript = GetComponent<ComboCounter>();
        healthScript = GetComponent<PlayerHealth>();
        consumeScript = GetComponent<PlayerConsume>();
        controllerScript = GetComponent<PlayerController>();
        GameObject iconsHolder = GameObject.Find("HUDCanvas/AbilityIcons");
        chargeIcon = iconsHolder.transform.GetChild(0).GetComponent<IconCoolDown>();
        dodgeIcon = iconsHolder.transform.GetChild(1).GetComponent<IconCoolDown>();
        tornadoIcon = iconsHolder.transform.GetChild(3).GetComponent<IconCoolDown>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void ComputeVelocity()
    {
        if (!isDodging && !isCharging && !isSpinning && !consumeScript.consuming && !attackScript.attacking)
        { // Make sure the player is not using other skills.
            if (chargeIcon.isActiveAndEnabled)
            {
                if (Input.GetKeyUp(GameInputManager.GIM.charge) && chargeIcon.isSkillUsable())
                {
                    anim.Play("Player_Charge0");
                    if (bGrounded == false)
                    {
                        chargeDelay = 0.0f;
                    }
                    else
                    {
                        chargeDelay = 0.2f;
                    }
                    isCharging = true;
                    sfx.PlayTCharge();
                    chargeIcon.setCoolDown(chargeCoolDown);
                }
            }

            if (dodgeIcon.isActiveAndEnabled)
            {
                if (Input.GetKeyUp(GameInputManager.GIM.dodge) && dodgeIcon.isSkillUsable())
                {
                    isDodging = true;
                    sfx.PlayRoll();
                    dodgeIcon.setCoolDown(dodgeCoolDown);
                }
            }

            if (tornadoIcon.isActiveAndEnabled)
            {
                if (Input.GetKeyUp(GameInputManager.GIM.tornado) && tornadoIcon.isSkillUsable())
                {
                    isSpinning = true;
                    animEnded = false;
                    tornadoIcon.setCoolDown(tornadoCoolDown);
                }
            }
        }

        useCharge();
        useDodge();
        useTornado();
    }

    [SerializeField] float chargeDelay = 0.2f; // The amount of delay before rushing forward (*focus energy)
    [SerializeField] float chargeDuration = 0.60f; //original mduration = 0.4f & speed = 48.0f
    [SerializeField] float chargeSpeed = 24.0f;
    [SerializeField] float chargeCoolDown = 3f;
    public bool isCharging = false;

    void useCharge()
    {
        if (isCharging)
        {
            controllerScript.enabled = false; // Disable the scripts that control actions
            attackScript.enabled = false;
            consumeScript.enabled = false;

            float total = chargeDelay + chargeDuration;

            if ((skillsTimer > total))
            { // Conditions that end the skill
                skillsTimer = 0.0f;
                isCharging = false;
                collidedBodies.Clear();
                instantiateCount = 0;
                anim.SetBool("charging", false);

                controllerScript.enabled = true; // Re-enable the scripts that control actions
                attackScript.enabled = true;
                consumeScript.enabled = true;
                gravityModifier = 1.0f;
                return;
            }

            if (skillsTimer > chargeDelay)
            {
                //	targetVelocity.x = ((skillsTimer/chargeDuration)*(skillsTimer/chargeDuration) * chargeSpeed) * m_facingDir;
                gravityModifier = 0.0f; // Rather than adding an upward force, change the gravity to zero for this script.

                anim.SetBool("charging", true);
                
                targetVelocity.x = (((total - skillsTimer) / chargeDuration) * ((total - skillsTimer) / chargeDuration) * ((total - skillsTimer) / chargeDuration) * chargeSpeed) * transform.lossyScale.x; // This equation causes the x velocity to slow down over time.

                if (!bGrounded)
                    AddForce(0, 0); // To ensure the y-velocity remains zero while charging in the air.

                if (instantiateCount < 1)
                {
                    GameObject dustInstance = Instantiate(ChargeDustRight, playerFacing.position, playerFacing.rotation);
                    Vector3 dustCloudScale = dustInstance.transform.localScale;
                    dustCloudScale.x = transform.localScale.x; // Invert the dustRight to match the charging direction
                    dustInstance.transform.localScale = dustCloudScale;
                    instantiateCount++;
                }
            }
            skillsTimer += Time.deltaTime;
        }
    }

    [SerializeField] float m_DodgeRollSpeed = 8;
    [SerializeField] float dodgeDelay = 0f;
    [SerializeField] float dodgeDuration = 0.6f;
    [SerializeField] float dodgeCoolDown = 3f;
    public bool isDodging = false;

    void useDodge()
    {
        if (isDodging)
        {
            controllerScript.enabled = false; // Disable the scripts that control actions
            attackScript.enabled = false;
            consumeScript.enabled = false;

            if ((skillsTimer > dodgeDuration + dodgeDelay))
            {
                controllerScript.enabled = true; // Re-enable the scripts that control actions
                attackScript.enabled = true;
                consumeScript.enabled = true;

                healthScript.setVulnerability(true);

                skillsTimer = 0.0f;
                isDodging = false;
                anim.Play("Player_Idle");
                return;
            }

            if (skillsTimer > dodgeDelay)
            {
                healthScript.setVulnerability(false); // Invulnerable.

                anim.Play("Player_DodgeRoll");

                // float dodgedTime = skillsTimer - dodgeDelay / dodgeDuration; // The amount of time spent on dodge rolling.
                // targetVelocity.x = (((dodgeDuration - dodgedTime) * (dodgeDuration - dodgedTime) + 0.2f) * m_DodgeRollSpeed) * transform.lossyScale.x;
                // targetVelocity.x = (dodgedTime * dodgedTime * m_DodgeRollSpeed) * transform.lossyScale.x;
                targetVelocity.x = m_DodgeRollSpeed * transform.lossyScale.x;
            }
            skillsTimer += Time.deltaTime;
        }
    }

    public bool isSpinning = false;
    [SerializeField] float tornadoCoolDown = 8.0f;
    [SerializeField] float tornadoDPS = 4.0f; // How much damage does the tornado skill deal in one second?
    [SerializeField] Collider2D tornadoHitBox;
    [SerializeField] float windPower = 2.0f; // The higher the value, the quicker the enemies get sucked into the tornado.
    [SerializeField] float TornadoDamageInterval = 1.0f; // How often does the tornado deal damage and flinch the enemies (in second).
    bool animEnded = false;

    void AnimFinished() // A function to be called at the end of Player_Tornado animation.
    {
        animEnded = true;
    }

    void useTornado()
    {
        if (isSpinning)
        {
            controllerScript.enabled = true;
            attackScript.enabled = false;
            consumeScript.enabled = false;

            if (animEnded)
            {
                skillsTimer = 0.0f;
                isSpinning = false;
                collidedBodies.Clear();

                controllerScript.enabled = true; // Re-enable the scripts that control actions
                attackScript.enabled = true;
                consumeScript.enabled = true;

                healthScript.setVulnerability(true);
                anim.Play("Player_Idle");
                return;
            }

            if (skillsTimer >= TornadoDamageInterval)
            {
                skillsTimer = 0.0f;
                for (int i = 0; i < collidedBodies.Count; i++) // Loop and deal damage to each collidedBody.
                {
                    HealthScript otherHealth = collidedBodies[i].GetComponent<HealthScript>();
                    if (otherHealth == null) // Cannot get health script, it must be the boss because healthscript for boss was attached at the parent of the collider2D...
                        otherHealth = collidedBodies[i].GetComponentInParent<HealthScript>();
                    if (otherHealth.IsVulnerable())
                    {
                        attackScript.lastHitTime = Time.time;
                        comboScript.UpdateCC(otherHealth.ScoreValue);
						otherHealth.TakeDamage(tornadoDPS * TornadoDamageInterval, Vector2.zero, true, healthScript, tornadoWeight, collidedBodies[i]);
                        sfx.PlayPunch1();
                    }
                }
            }
            
            camShake.Shake(0.1f, 0.1f);
            anim.Play("Player_Tornado");
            healthScript.setVulnerability(false);
            skillsTimer += Time.deltaTime; // Sync time with physics update for the tornado to deal damage based on damage interval specified.
        }
    }

    public void resetSkillTimer() // If player is flinched, whatever skill he is using is terminated.
    {
        skillsTimer = 0.0f;
        controllerScript.enabled = true;
        attackScript.enabled = true;
        gravityModifier = 1.0f;
        collidedBodies.Clear();
        isCharging = false;
        isDodging = false;
        isSpinning = false;
    }

    List<Collider2D> collidedBodies = new List<Collider2D>(20);
    [SerializeField] int ChargeSkillDamage = 10;

    void OnTriggerExit2D(Collider2D other)
    {
        if (isSpinning && collidedBodies.Contains(other)) // A body left the tornado hit box, we should not deal damage to it in useTornado anymore.
        {
            collidedBodies.Remove(other);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            HealthScript enemyHealthManager = other.GetComponent<HealthScript>();
            if (enemyHealthManager.IsVulnerable())
            {
                if (isCharging && skillsTimer > chargeDelay + 0.02f && !collidedBodies.Contains(other))
                {
                    collidedBodies.Add(other);
                    attackScript.lastHitTime = Time.time;
                    comboScript.UpdateCC(enemyHealthManager.ScoreValue);

					enemyHealthManager.TakeDamage(ChargeSkillDamage, new Vector2(targetVelocity.x * 4, 0), true, healthScript, chargeWeight, other);
                    sfx.PlayPunch2();
                    if (controllerScript.facingRight == true)
                    {
                        camShake.ShakeRight(0.3f, 0.2f);
                    }
                    else if (controllerScript.facingRight == false)
                    {
                        camShake.ShakeLeft(0.3f, 0.2f);
                    }
                }

                if (isSpinning && tornadoHitBox.IsTouching(other))
                {
                    if (!collidedBodies.Contains(other))
                    {
                        collidedBodies.Add(other);
                    }
                    float unit_xDistance = (transform.position.x - other.transform.position.x) / tornadoHitBox.bounds.extents.x;
                    float unit_yDistance = (transform.position.y - other.transform.position.y) / tornadoHitBox.bounds.extents.y;
                    // Sucks the enemies staying inside tornadohitbox to the player's position.
                    enemyHealthManager.TakeDamage(0, new Vector2(unit_xDistance, 0), false); // Damage is dealt inside useTornado() function.
                }

            }
        }
        else if (other.tag == "Boss")
        {
            BossHealth bossHealth = other.GetComponentInParent<BossHealth>();
            if (isCharging && skillsTimer > chargeDelay)
            {
                if (bossChargeHittable == true)
                {
                    attackScript.lastHitTime = Time.time;
                    comboScript.UpdateCC(bossHealth.ScoreValue);
					bossHealth.TakeDamage(bossChargeDamage, Vector2.zero, true, healthScript, chargeWeight, other);
                    sfx.PlayPunch2();
                    camShake.ShakeRight(0.3f, 0.2f);
                    bossChargeHittable = false;
                    StartCoroutine(BossChargeReset());
                }
            }

            if (isSpinning && !collidedBodies.Contains(other))
            {
                if (tornadoHitBox.IsTouching(other))
                {
                    collidedBodies.Add(other);
                    
                }
            }
        }
    }

    IEnumerator BossChargeReset()
    {
        yield return new WaitForSeconds(1.5f);
        bossChargeHittable = true;
    }

    public void EnablePlayerAttack()
    {
        attackScript.enabled = true;
    }
    public void DisablePlayerAttack()
    {
        attackScript.enabled = false;
    }

    public void PlayTornado()
    {
        sfx.PlayTornado();
    }
}
