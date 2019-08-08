using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoonAI : EnemyAI {

    [SerializeField] float m_BAtkDamage;
	[SerializeField] float BAtkWeight; // The duration of animation freeze when headbutt hit the player.
    bool animEnded = false;
    public Collider2D[] hitBoxes;
    Sounds sfx;

    void Start()
    {
        sfx = FindObjectOfType<Sounds>();
    }

    void AnimFinished() // A function to be called at the end of an animation
    {
        animEnded = true;
    }

    protected override void useBasicAttack(Vector2 dir_to_player, bool noGround)
    {
        if (animEnded)
        { // Only move after basic attack animation is finished
            animEnded = false;
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
    
    //Play Goon SFX
    public void PlayGoonAttack()
    {
        sfx.PlayGoonAttack();
    }

    public void PlayGoonFlinch()
    {
        sfx.PlayGoonFlinch();
    }

    public void PlayGoonJump()
    {
        sfx.PlayGoonJump();
    }

    public void PlayGoonDeath()
    {
        sfx.PlayGoonDeath();
    }

    protected override void useSkill(bool noGround) { ChangeState(AIStates.MOVE); }
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
