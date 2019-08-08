using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* March 18, 4:19AM: Rework needed!*/
public class MakiRollAI : EnemyAI
{
    Sounds sfx;
    [SerializeField] float m_DodgeRollSpeed; //9 is fine if using box collider
    [SerializeField] int m_BAtkDamage;
    [SerializeField] float BAtkWeight = 0.04f;
    bool animEnded = false;
    bool rolling = false; // A flag to start rolling forward inside UseSkill(). Rolling is set to true in "MakiDodge" animation.
    public Collider2D[] hitBoxes;

    void Start()
    {
        sfx = FindObjectOfType<Sounds>();
    }

    void AnimFinished() // A function to be called at the end of an animation
    {
        animEnded = true;
    }

    void RollForward() // A function to be called in the middle of "MakiDodge" animation
    {
        rolling = true;
    }

    protected override void useBasicAttack(Vector2 dir_to_player, bool noGround)
    {
        if (animEnded)
        { // Only move after basic attack animation is finished
            animEnded = false;
            bool playerFaceRight = m_playerTransform.GetComponent<PlayerController>().facingRight;

            if (isSkillUsable())
            {
                if ((dir_to_player.x < 0 && playerFaceRight) || // The player is at the left of Maki and is facing right
                (dir_to_player.x > 0 && !playerFaceRight)) // The player is at the right of Maki and is facing left
                {
                    Collider2D[] cldrs = GetComponentsInChildren<Collider2D>(); // Disabling all active hitbox
                    for (int i = 2; i < cldrs.Length; i++) // i starts from 2 because there are two colliders attached on MakiRoll parent object.
                    {
                        if (cldrs[i].gameObject.name == "RightHitBox" || cldrs[i].gameObject.name == "LeftHitBox")
                            if (cldrs[i].isActiveAndEnabled)
                                cldrs[i].gameObject.SetActive(false);
                    }

                    ChangeState(AIStates.SKILL);
                    return;
                }
            }

            if (dir_to_player.x > m_basicAttackRange || dir_to_player.x < -m_basicAttackRange) // The player has gone far
            {
                ChangeState(AIStates.MOVE);
                return;
            }

            if (m_facingDir > 0 && dir_to_player.x < 0) // Punching in the wrong direction
                m_facingDir = -1;
            else if (m_facingDir < 0 && dir_to_player.x > 0)
                m_facingDir = 1;
        }
        
        m_animator.Play(attackAnim.name);
        
    }

    protected override void useSkill(bool noGround) // Dodge roll
    {
        if (animEnded || noGround || shouldJump)
        {
            InterruptSkill();
            return;
        }

        m_animator.Play(skillAnim.name);

        if (rolling) {
            targetVelocity.x = m_DodgeRollSpeed * m_facingDir;
            enemyHealthScript.setVulnerability(false);
        }
        
        skillTimer += Time.deltaTime;
    }

    //Play Maki SFX
    public void PlayMakiAttack()
    {
        sfx.PlayMakiAttack();
    }

    public void PlayMakiFlinch()
    {
        sfx.PlayMakiFlinch();
    }

    public void PlayMakiJump()
    {
        sfx.PlayMakiJump();
    }

    public void PlayMakiDeath()
    {
        sfx.PlayMakiDeath();
    }

    void InterruptSkill()
    {
        skillCDCounter = skillCD;
        enemyHealthScript.setVulnerability(true);
        skillTimer = 0.0f;
        animEnded = false;
        rolling = false;
        ChangeState(AIStates.MOVE);
    }

    public void rotateRigidBodyAroundPointBy(Rigidbody2D rb, Vector3 origin, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);
        rb.MovePosition(q * (rb.transform.position - origin) + origin);
        rb.MoveRotation(Quaternion.Angle(rb.transform.rotation, rb.transform.rotation * q));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            HealthScript PlayerHealth = other.GetComponent<HealthScript>();
            if (PlayerHealth.IsVulnerable())
            {
                if (m_currentState == AIStates.ATTACK) /*Making sure the player is hit by the children's collider (left/right hitbox) but not the default collider*/
                {
                    for (int i = 0; i < hitBoxes.Length - 1; i++)
                        if (hitBoxes[i].IsTouching(other))
							PlayerHealth.TakeDamage(m_BAtkDamage, Vector2.zero, true, enemyHealthScript, BAtkWeight, other);
                }
            }
        }
    }
}
