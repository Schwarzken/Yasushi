using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for health related scripts.
public class HealthScript : MonoBehaviour {

    [SerializeField] protected float MaxHealth; // 100 for player and special enemies, 50 for goon.
    public bool canDie = true; // Set it as false for the four demo enemies.

    protected float currentHealth;
    protected bool vulnerable = true; // Check (in player's/enemies' attack sprite) before applying damage or forces.
    public int ScoreValue;
	[SerializeField] protected GameObject hitEffect;

    public virtual void TakeDamage(float amount, Vector2 knockBackForce, bool flinch, HealthScript attacker = null, float freezeDelay = 0.0f, Collider2D gotHitCollider = null)
    {
        /* attacker = the health script of the gameObject whose script has just called TakeDamage()
		 * freezeDelay = the duration of animation freeze applied, both to the attacker/damaged.
		 * gotHitCollider = mandatory for the spawning of hit effects, as the hit effect will spawn at the bound of gotHitCollider
		 */
        currentHealth -= amount;
    }

    public virtual void FreezeFrames(float freezeDelay)
    {
        // Disable the behavior scripts and the animator for 0.06
        Invoke("UnFreezeFrames", freezeDelay);
    }

    protected virtual void UnFreezeFrames()
    {
        // Enable the behavior scripts and the animator, again.
    }

    public void setVulnerability(bool isVulnerable) { vulnerable = isVulnerable; }
    public bool IsVulnerable() { return vulnerable; }
}