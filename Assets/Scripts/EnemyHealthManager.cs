using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthManager : HealthScript
{
    bool isAlive;
    public Text SubScore;
    private SpriteRenderer spriteRenderer;

    public GameObject consumeIndicator; //For consumable animation

    public float smallKnock;
    public float smallKnockLength;
    public float knockback;
    public float knockupx;
    public float knockupy;
    public float knockbackLength;
    public float knockbackDuration;
    public float knockupLength;
    bool knockFromRight = false; //Checking if From Right

    [HideInInspector] public bool canConsume = false;

    public Transform other;
    private FightBoundary fightBoundary;
    private Rigidbody2D myrigidbody2D;
    private GameController gameController;
    private EnemyAI enemyAI;
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        //consumeAnim = consumeIndicator.GetComponent<Animator>();
        fightBoundary = GetComponent<FightBoundary>();
        Transform SubScore_xform = GameObject.Find("HUDCanvas").transform.Find("ComboCounter/SubScore");
        SubScore = SubScore_xform.GetComponent<Text>();
        SubScore.text = ScoreValue + "";
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        enemyAI = GetComponent<EnemyAI>();
        gameController = FindObjectOfType<GameController>();
        isAlive = true;
        currentHealth = MaxHealth;
        myrigidbody2D = GetComponent<Rigidbody2D>();
        knockupx = knockback / 2;
        //knockupy = 200;
        smallKnock = knockback / 2;
        animator = GetComponent<Animator>();
    }

    void isOnRight() //Checks if On Right
    {
        other = GameObject.FindGameObjectWithTag ("Player").transform;
        if (transform.position.x < other.position.x)
        {
            knockFromRight = true;
        }
        else
        {
            knockFromRight = false;
        }
    }

    void Death()
    {
        if (currentHealth <= 0 && canDie)
        {     
            enemyAI.ChangeState(AIStates.DEAD);
            isAlive = false;
        }
        else
        {
            return;
        }
    }

    public void DestroyEnemy() // To be called by death animations, but not-in-use currently.
    {
        SubScore.text += ScoreValue;
        gameController.AddSubScore(ScoreValue);
		Destroy (gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "KillZone")
        {
            TakeDamage(MaxHealth, Vector2.zero, true);
        }
    }

    void Consumable()
    {
        if (currentHealth <= (MaxHealth * 0.2))
        {
            canConsume = true;
            enemyAI.enabled = false;
            if(this.name == "Tempura1" || this.name == "MakiRoll1" || this.name == "Chuka1")
            {
                vulnerable = false;
                this.canDie = false;
            }
        }
    }

    private void Knockback()
    {
        if (knockbackLength > 0)
        {
            if (knockFromRight == true)
            { //If on Right, knocks enemy to left
                myrigidbody2D.velocity = new Vector2(-knockback, knockback);
            }
            else
            { //Knocks enemy to the right
                myrigidbody2D.velocity = new Vector2(knockback, knockback);
            }
            knockbackLength -= Time.deltaTime;
        }
        if (knockupLength > 0)
        {
            if (knockFromRight == true)
            { //If on Right, knocks enemy to left
                
                myrigidbody2D.velocity = new Vector2(-knockupx, knockupy);
            }
            else
            { //Knocks enemy to the right
                
                myrigidbody2D.velocity = new Vector2(knockupx, knockupy);
            }
            knockupLength -= Time.deltaTime;
        }
        if (smallKnockLength > 0)
        {
            if (knockFromRight == true)
            { //If on Right, knocks enemy to left
                myrigidbody2D.velocity = new Vector2(-smallKnock, smallKnock);
            }
            else
            { //Knocks enemy to the right
                myrigidbody2D.velocity = new Vector2(smallKnock, smallKnock);
            }
            smallKnockLength -= Time.deltaTime;
        }
        //To reset
        if (knockupLength <= 0 && knockbackLength <= 0 && smallKnockLength <= 0)
        {
            myrigidbody2D.velocity = new Vector2(0, 0);
        }
        //To reset
    }

    IEnumerator SpriteFlash()
    {
        spriteRenderer.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(1, 1, 1);
    } // NOT IN USE, REPLACED WITH ANIMATION

    IEnumerator ConsumableSpriteFlash()
    {
        spriteRenderer.color = new Color(0, 0, 1);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(1, 1, 1);
    } // NOT IN USE, REPLACED WITH ANIMATION

    //To trigger enemy's fight boundary so that they don't leave the screen
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.name == "EnemyFightBoundary")
        {
            this.fightBoundary.enabled = true;
        }
    }

	public void resetConsumable()
	{
		canConsume = false;
	}

    public override void FreezeFrames(float freezeDelay)
    {
        // Disable the behavior scripts and the animator for 0.06.
        enemyAI.enabled = false;
        animator.enabled = false;
        Invoke("UnFreezeFrames", freezeDelay);
    }

    protected override void UnFreezeFrames()
    {
        // Enable the behavior scripts and the animator, again.
        enemyAI.enabled = true;
        animator.enabled = true;
    }

    /*====================FORMAT IN PROGRESS====================*/
    public override void TakeDamage(float amount, Vector2 knockBackForce, bool flinch, HealthScript attacker = null, float freezeDelay = 0.0f, Collider2D gotHitCollider = null)
    {
        currentHealth -= amount;
        enemyAI.AddForce(knockBackForce.x, knockBackForce.y);

        if (flinch) enemyAI.ChangeState(AIStates.FLINCHED);

        if (gotHitCollider)
        { // Pass attacker (their health scripts) and 'this object's collider that was hit' as argument for hiteffect instantiation.
            Vector3 hitPos = gotHitCollider.bounds.ClosestPoint(new Vector3(attacker.transform.position.x, attacker.transform.position.y + 0.86f, 0)); // 0.86f is the shoulder height of yasushi.
            GameObject hitEffectInst = Instantiate(hitEffect, hitPos, attacker.transform.rotation);
            Vector3 hitEffectScale = hitEffectInst.transform.localScale;
            hitEffectScale.x *= Mathf.Sign(attacker.transform.position.x - gotHitCollider.transform.position.x);
            hitEffectInst.transform.localScale = hitEffectScale;
        }

        if (attacker != null && freezeDelay > 0)
        {
            attacker.FreezeFrames(freezeDelay);
            FreezeFrames(freezeDelay);
        }

        if (currentHealth <= 0 && canDie)
        {
            vulnerable = false;
            canConsume = false;
            consumeIndicator.gameObject.SetActive(false); // Turn off the consumeIndicator why dying anim is playing so that it won't confuse the player.
            enemyAI.ChangeState(AIStates.DEAD); // Dead state plays death animation and call DestroyEnemy() at the end of its anim.
        }
        else if (currentHealth <= (MaxHealth * 0.2)) // When health fell under 20%, enemies became consumable.
        {
            enemyAI.ChangeState(AIStates.CONSUMABLE);
            canConsume = true;
            consumeIndicator.gameObject.SetActive(true);
            if (this.name == "Tempura1" || this.name == "MakiRoll1" || this.name == "Chuka1")
            {
                vulnerable = false;
                this.canDie = false;
            }
        }
    }
}




