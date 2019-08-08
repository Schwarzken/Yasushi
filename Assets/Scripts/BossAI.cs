using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour {

    [SerializeField] public float attackInterval = 3.0f; // How often, in seconds does the boss attack?
    float nextAttackTime;

    Sounds sound;
    Animator animator;
    BossHealth bossHealthScript;
    Transform playerTransform; // Store the reference to the player.
    public PlayerController playerController;

    bool feverMode = false;
	public CameraShake camShake;
    
    [SerializeField] Transform ZoneA; // The leftmost area, in the boss scene
    [SerializeField] Transform ZoneB; // The mid bottom area, in the boss scene
    [SerializeField] Transform ZoneC; // The rightmost area, in the boss scene
    [SerializeField] Transform ZoneD; // The mid top area,  in the boss scene

    [SerializeField] AnimationClip idleAnim; // The animation of boss doing nothing.
    [SerializeField] AnimationClip stabAnimZoneA; // Boss stabs the leftmost area.
    [SerializeField] AnimationClip stabAnimZoneD; // Boss stabs the mid top area.
    [SerializeField] AnimationClip stabAnimZoneC; // Boss stabs the rightmost area.
    [SerializeField] AnimationClip fireTorchAnim; // Boss burns the player.
    [SerializeField] AnimationClip handsweepHighAnim;
    [SerializeField] AnimationClip handsweepLowAnim;
    [SerializeField] AnimationClip flinchAnim; // Boss flinched.

    [SerializeField] Collider2D zoneAHitBox;
    [SerializeField] Collider2D zoneCHitBox;
    [SerializeField] Collider2D zoneDHitBox;
    [SerializeField] Collider2D flameHitBox;
    [SerializeField] Collider2D highSweepHitBox;
    [SerializeField] Collider2D LowSweepHitBox;

    bool playerHasBeenStabbed = false; // Player can only take (stab)damage if this flag is set to false.

    bool animIsPlaying = false;
    char stabZone;
    BossStates m_currentState = 0;

    [SerializeField] float stabDamage = 20; // One-time damage dealt to the player if he enters the knife hitbox.
    [SerializeField] float stabWeight = 0.08f; // The duration of anim freeze if the player enters the knife hitbox.
    [SerializeField] float fireDamagePersecond = 10; // Total damage in one second dealt to the player if he stays in fire hitbox.
    [SerializeField] float handSweepDamage = 20;
    [SerializeField] float handSweepWeight = 0.08f;
    int sweptCount = 0; // How many times had the boss swept his hand?
    bool sweepBottom; // The boss sweeps the bottom area of the scene?

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        bossHealthScript = GetComponent<BossHealth>();
        playerTransform =  GameObject.FindGameObjectWithTag("Player").transform;
        nextAttackTime = Time.time + attackInterval;
        sound = FindObjectOfType<Sounds>();
        Time.timeScale = 1; // Unpause the game. Should be removed in the next build.
    }

    void AnimationHasEnded() // *Call this function as animation event at the end of each stab clip to ensure the state transition works.
    {
        animIsPlaying = false;
    }
    
    void Update()
    {
        switch (m_currentState)
        {
            case BossStates.IDLE:
                animator.Play(idleAnim.name);

                if (Time.time - nextAttackTime > 0) // Cool down is over, attack again.
                {
                    if (playerTransform != null) // Player exists and not destroyed.
                    {
                        // Decide its attack mode by checking which zone the player is staying.
                        Vector3 playerPos = playerTransform.position;
                        if (playerPos.y < ZoneD.position.y && Random.value < 0.3f) // 30% chance to use flamethrower
                        { 
						    stabZone = 'B';
                            ChangeState(BossStates.FIRETORCH);
                        }
                        else if (Random.value < 0.3f) // 30% chance to use hand sweeping
                        { // Initiate the consecutive hand sweeping attacks.
                            sweptCount = 0;
                            sweepBottom = playerPos.y < ZoneD.position.y;
                            ChangeState(BossStates.HANDSWEEP);
                        }
                        else if (playerPos.x < ZoneA.position.x)
                        {
                            stabZone = 'A';
                            sound.PlayKnife();
                            ChangeState(BossStates.KNIFESTAB);
                        }
                        else if (playerPos.x < ZoneB.position.x)
                        {
                            if (playerPos.y > ZoneD.position.y) // Player is standing on the platform.
                            {
                                stabZone = 'D';
                                sound.PlayKnife();
                                ChangeState(BossStates.KNIFESTAB);
                            }
                            else // Player is under the platform.
                            {
                                stabZone = 'B';
                                ChangeState(BossStates.FIRETORCH);
                            }
                        }
                        else if (playerPos.x < ZoneC.position.x)
                        {
                            stabZone = 'C';
                            sound.PlayKnife();
                            ChangeState(BossStates.KNIFESTAB);
                        }
                    }
                }
                break;

            case BossStates.KNIFESTAB:
                if (!animIsPlaying)
                {
                    ChangeState(BossStates.IDLE);
                    nextAttackTime = Time.time + attackInterval;
                    return;
                }

                switch (stabZone)
                {
                    case 'A':
                        animator.Play(stabAnimZoneA.name);
                        break;
                    case 'C':
                        animator.Play(stabAnimZoneC.name);
                        break;
                    case 'D':
                        animator.Play(stabAnimZoneD.name);
                        break;
                }
                break;

            case BossStates.FIRETORCH:
                if (!animIsPlaying)
                {
                    ChangeState(BossStates.IDLE);
                    nextAttackTime = Time.time + attackInterval;
                    return;
                }
                animator.Play(fireTorchAnim.name);
                break;

            case BossStates.HANDSWEEP:
                if (!animIsPlaying)
                {
                    animIsPlaying = true;
                    sweepBottom = !sweepBottom; // One of the sweeping animation has played, now invert the flag to play the other.
                    sweptCount++;
                    if (sweptCount > 1)
                    {
                        ChangeState(BossStates.IDLE);
                        return;
                    }
                }
                if (sweepBottom)
                    animator.Play(handsweepLowAnim.name);
                else
                    animator.Play(handsweepHighAnim.name);
                break;

            case BossStates.FLINCHED:
                if (!animIsPlaying)
                {
                    ChangeState(BossStates.IDLE);
                    return;
                }
                animator.Play(flinchAnim.name);
                break;

            case BossStates.DEAD:
                Destroy(gameObject);
                playerController.enabled = false;
                break;
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && m_currentState == BossStates.FIRETORCH)
        {
            // (!(bossCollider.IsTouching(other) || stunHitBox.IsTouching(other))) // Colliding with the chef's body or stunHitBox (hand?) does not hurt the player.
            if (flameHitBox.IsTouching(other))
            {
                HealthScript playerHealth = other.GetComponent<HealthScript>();
                if (playerHealth.IsVulnerable())
                {
                    playerHealth.TakeDamage(fireDamagePersecond * Time.deltaTime, new Vector2(-10,0), true);
                }
            }
        }

        if (!playerHasBeenStabbed) // Ensure one-time damage.
        {
            if (other.CompareTag("Player") && m_currentState == BossStates.KNIFESTAB)
            {
                if ((zoneAHitBox.IsTouching(other) || zoneCHitBox.IsTouching(other) || zoneDHitBox.IsTouching(other)))
                {
                    HealthScript playerHealth = other.GetComponent<HealthScript>();
                    if (playerHealth.IsVulnerable())
                    {
						playerHealth.TakeDamage(stabDamage, Vector2.zero, true, bossHealthScript, stabWeight, other);
                        playerHasBeenStabbed = true;
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && m_currentState == BossStates.HANDSWEEP)
        {
            if (highSweepHitBox.IsTouching(other) || LowSweepHitBox.IsTouching(other))
            {
                HealthScript playerHealth = other.GetComponent<HealthScript>();
                if (playerHealth.IsVulnerable())
                {
                    playerHealth.TakeDamage(handSweepDamage, Vector2.zero, true, bossHealthScript, handSweepWeight, other);
                }
            }
        }
    }

    public void ChangeState(BossStates newState)
    {
        m_currentState = newState;
        playerHasBeenStabbed = false;
        animIsPlaying = true;
    }

	public void CameraShake()
	{
		camShake.Shake (0.4f, 0.2f);
	}

	public void FlameShake()
	{
		camShake.ShakeLeft (0.4f, 0.5f);
	}

    public void BossStab()
    {
        sound.PlayBossStab();
    }
}

public enum BossStates
{
    IDLE,
    KNIFESTAB,
    FIRETORCH,
    HANDSWEEP,
    FLINCHED,
    DEAD
};
