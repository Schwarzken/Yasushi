using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : PhysicsObject {
	public float damage = 30.0f;
	HealthScript bossHealth;
    HealthScript playerHealth;
    HealthScript enemyHealth;
    EnemyAI enemyAI;
	Animator anim;
    PlayerController playerController;
    bool rotating;
    bool canBeHit;
    private float smooth;
    public float durationTime;
    public Vector3 rotationDirection;
    public int knifeKnockback;
    public float rotationStop;

	void Start()
	{
		anim = GetComponent<Animator> ();
        canBeHit = true;
        rotating = false;
	}

    private void Update()
    {
        if(rotating == true)
        {
            smooth = Time.deltaTime * durationTime;
            transform.Rotate(rotationDirection * smooth);
            print(transform.rotation.z);
        }
        if(transform.rotation.z <= -rotationStop)
        {
            
            rotating = false;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
	{
         if(this.tag == "Takoyaki")
         {
             if (coll.gameObject.tag == "Player" && coll.GetComponent<PlayerSkill>().isCharging == true)
             {
                 AddForce(150, 0);
                 anim.Play("Hit");
             }
             if (coll.gameObject.tag == "Boss" && velocity.x != 0)
             {
                 bossHealth = coll.GetComponentInParent<BossHealth>();
                 bossHealth.TakeDamage(damage, Vector2.zero, true);
                 Destroy(gameObject);
             }
         }
         else if(this.tag == "Knife")
         {
            if(coll.gameObject.tag == "Player")
            {
                playerHealth = coll.GetComponentInParent<PlayerHealth>();
                playerController = coll.GetComponentInParent<PlayerController>();
                if(playerController.facingRight == true)
                {
                    playerHealth.TakeDamage(20, new Vector2(-knifeKnockback, 0), true);
                }
                else
                {
                    playerHealth.TakeDamage(20, new Vector2(knifeKnockback, 0), true);
                }
            }
            else if (coll.gameObject.tag == "Enemy")
            {
                enemyHealth = coll.GetComponentInParent<EnemyHealthManager>();
                enemyAI = coll.GetComponentInParent<EnemyAI>();
                if (enemyAI.m_facingDir == 1f)
                {
                    enemyHealth.TakeDamage(30, new Vector2(-knifeKnockback, 0), true);
                }
                else
                {
                    enemyHealth.TakeDamage(20, new Vector2(knifeKnockback, 0), true);
                }
                //enemyHealth.TakeDamage(20, new Vector2(knifeKnockback, 0), true);
            }
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (this.tag == "VBoard")
        {
            if (coll.gameObject.tag == "Player" && coll.gameObject.GetComponent<PlayerSkill>().isCharging == true)
            {
                if (canBeHit == true)
                {
                    rotating = true;
                    canBeHit = false;
                }
            }
        }
    }
}
