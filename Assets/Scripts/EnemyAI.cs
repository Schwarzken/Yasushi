using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : PhysicsObject
{
    [SerializeField] protected float m_speed;
    [SerializeField] float m_jumpHeight;

    [HideInInspector] public float m_facingDir = 1.0f; // 1 or -1 **Check Renderer's flipX if the sprite drawn by artist does not face right.
    float m_oldDirection; // previous frame's moving direction, to check if the orientation has changed in this frame
    protected int m_frames; //the number of update frames passed since the last change in orientation

    float m_jumpCounter = 0.0f;
    float timeToTurn = 0.0f;
    // The maximum amount of time this unit can stay in air, its y velocity will decrease harshly after jumpCount > jumpDuration.
    [SerializeField] float m_maxJumpDuration;
    [SerializeField] protected float m_basicAttackRange;
    [SerializeField] protected float m_SkillRange;
    public float skillTimer = 0.0f;
    protected float skillCDCounter = 0.0f;
    [SerializeField] protected float skillCD = 2.0f;

    [SerializeField] protected AnimationClip idleAnim;
    [SerializeField] protected AnimationClip walkAnim;
    [SerializeField] protected AnimationClip attackAnim;
    [SerializeField] protected AnimationClip skillAnim;
    [SerializeField] protected AnimationClip flinchAnim;
    [SerializeField] protected AnimationClip consumableAnim;
    [SerializeField] protected AnimationClip deathAnim;

    public Rigidbody2D rb; // Used by fight boundary..
    SpriteRenderer m_spriteRenderer;
    protected Animator m_animator;
    protected Transform m_playerTransform;
    protected AIStates m_currentState = AIStates.MOVE;
    protected EnemyHealthManager enemyHealthScript;

    // Use this for initialization
    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealthScript = GetComponent<EnemyHealthManager>();
    }

    protected override void ComputeVelocity()
    {
        // It reached the edge of platform (or in air) if count equals to zero (no collider detected by the ray).
        int hitCount = Physics2D.Raycast(transform.position, Vector2.down, contactFilter, hitBuffer, 2);
        Vector2 playerDirection = m_playerTransform.position - transform.position; //unnormalized

        switch (m_currentState)
        {
            case AIStates.MOVE:
                Movement(hitCount == 0, playerDirection);
                if (walkAnim)
                    m_animator.Play(walkAnim.name);
                break;
            case AIStates.ATTACK:
                useBasicAttack(playerDirection, hitCount == 0);
                break;
            case AIStates.SKILL:
                useSkill(hitCount == 0);
                break;
            case AIStates.FLINCHED:
                skillTimer = 0.0f;
                // skillCDCounter = skillCD; // Reset enemies' skill CD if they're flinched.
                if (flinchAnim)
                    m_animator.Play(flinchAnim.name);
                else
                    ChangeState(AIStates.MOVE);
                break;
            case AIStates.CONSUMABLE:
                skillTimer = 0.0f;
                if (consumableAnim)
                    m_animator.Play(consumableAnim.name);
                break;
            case AIStates.DEAD:
                skillTimer = 0.0f;
                if (deathAnim)
                    m_animator.Play(deathAnim.name);
                else
                    enemyHealthScript.DestroyEnemy();
                break;
        }

        if (m_oldDirection == m_facingDir) { m_frames++; } // Store the num of frames of not changing orientation
        else { updateFacing(); }
        m_oldDirection = m_facingDir;
        skillCDCounter -= Time.deltaTime;
    }
    
    void Movement(bool noGround, Vector2 dir_to_player)
    {
        Vector2 move = Vector2.zero;

        if (Mathf.Abs(dir_to_player.y) < 10)
        { // If the vertical distance between this unit and the player is lesser than X units, chase player. The value of X was 2.
            if (dir_to_player.x < 0.1f && dir_to_player.x > -0.1f)
                timeToTurn = Time.time + 0.3f;
            if (Time.time - timeToTurn > 0)
                m_facingDir = Mathf.Sign(dir_to_player.x);
            if (noGround && dir_to_player.y > 0) // Don't jump if the player is at the bottom (?)
                shouldJump = true;
            move.x = m_facingDir;
            if (Mathf.Abs(dir_to_player.y) < shellRadius + 0.5f && bGrounded)
            { // The enemy and the player are in the same horizon.
                if (dir_to_player.x < m_basicAttackRange && dir_to_player.x > -m_basicAttackRange)
                { // Stops to basic attack
                    ChangeState(AIStates.ATTACK);
                    return;
                }
                else if (Mathf.Abs(dir_to_player.x) < m_SkillRange && isSkillUsable())
                {
                    ChangeState(AIStates.SKILL);
                    return;
                }
            }
        }
        else
        { // Patrol on its own platform...
            if (bGrounded && noGround && m_frames > 3) // is on floor and has reached the edge.
                m_facingDir *= -1;
            move.x = m_facingDir;
        }

        if (shouldJump && bGrounded)
        {
            velocity.y = m_jumpHeight;
            m_jumpCounter = 0.0f;
        }
        else if (m_jumpCounter > m_maxJumpDuration)
        {
            if (velocity.y > 0.0f)
                velocity.y = velocity.y * 0.5f;
        }

        m_jumpCounter += Time.deltaTime;
        targetVelocity = move * m_speed;
    }

    public float getFacingDirection() { return m_facingDir; }

    public void ChangeState(AIStates newState)
    {
        //print("Transition from " + m_currentState + " to " + newState);
        m_currentState = newState;
    }

    protected void updateFacing() // Update enemy's facing direction and localScale.x.
    {
        m_frames = 0; // Orientation changed -> Reset
        Vector3 localScale = transform.localScale;
        localScale.x = m_facingDir * Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }
    protected bool isSkillUsable() { return skillCDCounter < 0; }// Return true if skill is not cooling down.
    protected virtual void useSkill(bool noGround) { }
    protected virtual void useBasicAttack(Vector2 dir_to_player, bool noGround) { }
}

public enum AIStates
{
    MOVE,
    SKILL,
    ATTACK,
    FLINCHED,
    CONSUMABLE,
    DEAD,
    IDLE
};