using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempuraAI : EnemyAI
{
    Sounds sfx;
    [SerializeField] float chargeDelay = 0.6f; // The amount of delay before rushing forward (*focus energy)
    [SerializeField] float chargeDuration = 0.40f;
    [SerializeField] float chargeSpeed = 48.0f;

    bool doneAttacking = false;
    [SerializeField] float BAtkDamage;
    [SerializeField] float BAtkWeight = 0.06f;
    [SerializeField] float chargeDamage = 10;
    [SerializeField] float chargeWeight = 0.08f;
    public Collider2D[] hitBoxes;
	[SerializeField] Transform dustCloud;
	int instantiateCount;
	Transform myTransform;

    private void Start()
    {
        sfx = FindObjectOfType<Sounds>();
		myTransform = GetComponent<Transform> ();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            HealthScript PlayerHealth = other.GetComponent<HealthScript>();
            if (PlayerHealth.IsVulnerable())
            {
                if (m_currentState == AIStates.SKILL && skillTimer > chargeDelay) // Collided with the player when charging forward
                {
                    PlayerHealth.TakeDamage(chargeDamage, new Vector2(targetVelocity.x * (chargeDelay + chargeDuration) / skillTimer, 0), true, enemyHealthScript, chargeWeight, other);
                    InterruptSkill();
                }
                else if (m_currentState == AIStates.ATTACK)
                {
                    for (int i = 0; i < hitBoxes.Length - 1; i++) /*Making sure the player is hit by the children's collider (left/right hitbox) but not the default collider*/
                        if (hitBoxes[i].IsTouching(other))
                        {
                            PlayerHealth.TakeDamage(BAtkDamage, new Vector2(getFacingDirection() * 40, 5), true, enemyHealthScript, BAtkWeight, other);
                        }
                }
            }
        }
    }

    void FinishedAttacking() // A function to be called at the end of basic attack animation clips
    {
        doneAttacking = true;
    }

    protected override void useBasicAttack(Vector2 dir_to_player, bool noGround)
    {
        // Play animation and change to move when the animation ends...
        if (doneAttacking)
        {
            if (dir_to_player.x > m_basicAttackRange || dir_to_player.x < -m_basicAttackRange) // The player has gone far
            {
                ChangeState(AIStates.MOVE);
                doneAttacking = false;
                return; // Terminate this function, ignoring the following instructions
            }

            if (m_facingDir > 0 && dir_to_player.x < 0) // Punching in the wrong direction
                m_facingDir = -1;
            else if (m_facingDir < 0 && dir_to_player.x > 0)
                m_facingDir = 1;
        }
        
        m_animator.Play(attackAnim.name);
        
    }

    //Play Tempura SFX
    public void PlayTempuraAttack()
    {
        sfx.PlayTempuraAttack();
    }

    public void PlayTempuraFlinch()
    {
        sfx.PlayTempuraFlinch();
    }

    public void PlayTempuraJump()
    {
        sfx.PlayTempuraJump();
    }

    public void PlayTempuraDeath()
    {
        sfx.PlayTempuraDeath();
    }

    public void PlayTempuraSkill()
    {
        sfx.PlayTempuraSkill();
    }

    protected override void useSkill(bool noGround) // Tempura Charge
    {
        float total = chargeDelay + chargeDuration;

        if ((skillTimer > total) || shouldJump || noGround)
        { // Conditions that end the skill
            InterruptSkill();
            return;
        }

        if (skillTimer > chargeDelay)
        {
			if (m_facingDir < 0) {
				if (instantiateCount < 1) {
					dustCloud.localScale = new Vector3 (-1, 1, 1);
					Instantiate (dustCloud, myTransform.position, myTransform.rotation);
					instantiateCount++;
				}
			} else {
				if (instantiateCount < 1) {
					dustCloud.localScale = new Vector3 (1, 1, 1);
					Instantiate (dustCloud, myTransform.position, myTransform.rotation);
					instantiateCount++;
				}
			}
            m_animator.Play(skillAnim.name); // skill animation = tempura_charge1
            //	targetVelocity.x = ((skillTimer/chargeDuration)*(skillTimer/chargeDuration) * chargeSpeed) * m_facingDir;
            targetVelocity.x = (((total - skillTimer) / chargeDuration) * ((total - skillTimer) / chargeDuration) * ((total - skillTimer) / chargeDuration) * chargeSpeed) * m_facingDir; // This equation caused the x velocity slows down over time.
        }
        else
            m_animator.Play("Tempura_Charge0");
        skillTimer += Time.deltaTime;
    }

    void InterruptSkill()
    {
        skillTimer = 0.0f;
        skillCDCounter = skillCD;
		instantiateCount = 0;
        ChangeState(AIStates.MOVE);
    }
}
