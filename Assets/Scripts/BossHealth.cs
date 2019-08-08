using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : HealthScript {

    Sounds sound;
    BossAI bossAI;
    Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Slider healthBar;

    [SerializeField] Transform TakoyakiPrefab;
    Transform takoyaki; // Stores the reference of the takoyaki spawned by boss.
    Vector3 takoyakiOrigin; // The position of the takoyaki.
	public bool isDead;

    void Start () {
		isDead = false;
        currentHealth = MaxHealth;
        bossAI = GetComponent<BossAI>();
        healthBar.maxValue = MaxHealth;
        healthBar.value = currentHealth;
        sound = FindObjectOfType<Sounds>();
        takoyaki = Instantiate(TakoyakiPrefab); // Spawn a takoyaki.
        animator = GetComponent<Animator>();
    }

    public override void FreezeFrames(float freezeDelay)
    {
        // Disable the behavior scripts and the animator for 0.06.
        bossAI.enabled = false;
        animator.enabled = false;
        Invoke("UnFreezeFrames", freezeDelay);
    }

    protected override void UnFreezeFrames()
    {
        // Enable the behavior scripts and the animator, again.
        bossAI.enabled = true;
        animator.enabled = true;
    }

    public override void TakeDamage(float amount, Vector2 knockBackForce, bool flinch, HealthScript attacker = null, float freezeDelay = 0.0f, Collider2D gotHitCollider = null)
    {
        currentHealth -= amount;

        if (gotHitCollider) { // Pass attacker (their health scripts) and 'this object's collider that was hit' as argument for hiteffect instantiation.
            Vector3 hitPos = gotHitCollider.bounds.ClosestPoint(new Vector3(attacker.transform.position.x, attacker.transform.position.y + 0.86f, 0)); // 0.86f is the shoulder height of yasushi.
            GameObject hitEffectInst = Instantiate(hitEffect, hitPos, attacker.transform.rotation);
            Vector3 hitEffectScale = hitEffectInst.transform.localScale;
            hitEffectScale.x *= Mathf.Sign(attacker.transform.position.x - gotHitCollider.transform.position.x);
            hitEffectInst.transform.localScale = hitEffectScale;
        }

        float bossHealthPercentage = (currentHealth / MaxHealth) * 100;
        if (bossHealthPercentage % 15 == 0) // Divided by 15 and has no remainder -> multiple of 15.
            if (takoyaki == null) // Make sure there's no other takoyaki.
                takoyaki = Instantiate(TakoyakiPrefab);

        if (bossHealthPercentage < 50) // If bossHealthPercentage is lower than 50% -> FEVER?
            // GetComponent<Animator>().SetBool("fever", true);
            bossAI.attackInterval = 0.0f;

        StartCoroutine(SpriteFlashRed());

        healthBar.value = currentHealth;

        if (flinch)
        {
            bossAI.ChangeState(BossStates.FLINCHED);
            sound.PlayBossFlinch();
        }
        
        if (attacker != null)
        {
            attacker.FreezeFrames(freezeDelay);
            FreezeFrames(freezeDelay);
        }

        if (currentHealth <= 0 && canDie)
        {
            vulnerable = false;
            bossAI.ChangeState(BossStates.DEAD); // Dead state plays death animation.
            //playerController.enabled = false;
            sound.PlayBossDeath();
            sound.PlayVictory();
            sound.fading3 = true;
            StartCoroutine(AfterFade(sound));
            isDead = true;
        }
    }

    IEnumerator SpriteFlashRed()
    {
        spriteRenderer.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(1, 1, 1);
    }

    public static IEnumerator AfterFade(Sounds bgmM)
    {
        yield return new WaitForSeconds(1.2f);
        bgmM.fading3 = false;
        bgmM.fadeDone3 = true;
    }
}
