using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChukaIdakoAI : EnemyAI {

    Sounds sounds;
    [SerializeField] float m_BAtkDamage;
    [SerializeField] float BAtkWeight = 0.04f;
    [SerializeField] float m_SkillDamage; // Total damage dealt to the player throughout the pulling animation.
    [SerializeField] float GrabWeight = 0.08f; // The delay applied when the tentacle touches the player for the first time.

    bool animEnded = false;
    [SerializeField] Collider2D hitBox;
    [SerializeField] Collider2D pullBox;

    [SerializeField] AnimationClip fleeAnim;
    CITentacleTip tentacle_tip;
    Vector3 tentacle_lfpos; // tentacle_tip's position in the previous frame.

    void Start()
    {
        sounds = FindObjectOfType<Sounds>();
        tentacle_tip = transform.Find("tentacle_tip").GetComponent<CITentacleTip>();
        tentacle_tip.setPullRange(m_SkillRange); // Set the maximum range of the tentacle tip and create a chain of tentacle body sprites.
    }

    void AnimFinished() // A function to be called at the end of an animation
    {
        animEnded = true;
    }

    [SerializeField] float fleeDuration = 0.8f;
    float fleeTimer = 0.0f;
    float punchedDirection = 1;
    bool isPunched = false; //

    void Flee(bool noGround)
    {
        if (fleeTimer < fleeDuration)
        {
            m_animator.Play(fleeAnim.name);
            if ((shouldJump || noGround) && m_frames > 9)  // Knocked a wall or reached the edge of an platform when flee-ing.
            {
                punchedDirection *= -1;
            }
            targetVelocity.x = -punchedDirection * m_speed * 1.5f; // Runs in opposite punched direction.
            m_facingDir = Mathf.Sign(targetVelocity.x);
        }
        else
        {
            fleeTimer = 0.0f;
            isPunched = false;
            animEnded = false;
            ChangeState(AIStates.MOVE);
        }
        fleeTimer += Time.deltaTime;
    }

    protected override void useBasicAttack(Vector2 dir_to_player, bool noGround)
    {
        //if (dir_to_player.x < m_basicAttackRange || dir_to_player.x > -m_basicAttackRange)
        if (animEnded)
        { 
            if (isPunched) // Punched the player, it's time to flee.
            {
                Flee(noGround);
                return;
            }
            animEnded = false;

            if (dir_to_player.x > m_basicAttackRange || dir_to_player.x < -m_basicAttackRange) // The player has gone far
            {
                ChangeState(AIStates.MOVE);
                return;
            }

            if (m_facingDir > 0 && dir_to_player.x < 0) // Punching in the wrong direction -> Flip.
                m_facingDir = -1;
            else if (m_facingDir < 0 && dir_to_player.x > 0)
                m_facingDir = 1;
        }
        
        m_animator.Play(attackAnim.name);
    }

    protected override void useSkill(bool noGround)
    {
        skillTimer += Time.deltaTime;

        if (tentacle_tip.Extrude(skillTimer))
        { // Extrude function returns true after it fully retracted.
            grabbed = false;
            skillTimer = 0.0f;
            InterruptSkill();
            return;
        }
       
        m_animator.Play(skillAnim.name); // Uses a blank animation: "CIdako_Pulling"
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
                    if (hitBox.IsTouching(other))
                    {
                        isPunched = true;
                        punchedDirection = m_facingDir;
                        PlayerHealth.TakeDamage(m_BAtkDamage, Vector2.zero, true, enemyHealthScript, BAtkWeight, other);
                    }
                }
            }
        }
    }

    bool grabbed = false; // True if the tip has touched (and 'grabbed') the player.

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            HealthScript PlayerHealth = other.GetComponent<HealthScript>();
            if (PlayerHealth.IsVulnerable())
            {
                if (m_currentState == AIStates.SKILL && skillTimer > 0.1f)
                {
                    if (pullBox.IsTouching(other))
                    {
                        if (!grabbed) // Player is grabbed.
                        {
                            grabbed = true;
                            PlayerHealth.TakeDamage(5, Vector2.zero, false, enemyHealthScript, GrabWeight, other);
                        }

                        float hForce = tentacle_tip.transform.position.x - other.transform.position.x;
                        float vForce = other.bounds.extents.y;
                        PlayerHealth.TakeDamage(m_SkillDamage * Time.deltaTime, new Vector2(hForce * 60, vForce), true);
                    }
                }
            }
        }
    }

    public void PlayChukkaAttack()
    {
        sounds.PlayChukkaAttack();
    }

    public void PlayChukkaSkill()
    {
        sounds.PlayChukkaSkill();
    }

    public void PlayChukkaFlinch()
    {
        sounds.PlayChukkaFlinch();
    }

    void InterruptSkill()
    {
        skillCDCounter = skillCD;
        ChangeState(AIStates.MOVE);
    }
}