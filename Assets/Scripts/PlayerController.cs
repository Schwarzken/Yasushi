using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public Sounds sfx;
    public Rigidbody2D rb;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    
    public bool facingRight; 
    public SpriteRenderer spriteRenderer;
	public Transform playerFacing;
	public GameObject JumpEffect;
    public bool jumping;
    private Animator animator;

    // Use this for initialization
	void Start()
	{
		facingRight = true;
	}

    void Awake()
    {
        //sfx = GetComponent<Sounds>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
		playerFacing = GetComponent<Transform> ();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

		if (GetComponent<PlayerAttack> ().attacking == false && GetComponent<PlayerConsume>().consuming == false && GetComponent<PlayerSkill>().isCharging == false) {
			if (Input.GetKey (GameInputManager.GIM.right)) {
				move.x += 1 ;
			}
			if(Input.GetKey(GameInputManager.GIM.left)){
				move.x -= 1;
			}
		}

		animator.SetFloat ("speed", Mathf.Abs (move.x));	

		if (move.x > 0) {
			facingRight = true;
			playerFacing.localScale = new Vector3(1, 1, 1);
		} else if (move.x < 0) {
			facingRight = false;
			playerFacing.localScale = new Vector3(-1, 1, 1);
		}

        if(GetComponent<PlayerAttack>().attacking == false && GetComponent<PlayerConsume>().consuming == false)
        {
			if(Input.GetKeyDown(GameInputManager.GIM.jump) && bGrounded)
            {
                sfx.PlayJump();
                velocity.y = jumpTakeOffSpeed;
				Instantiate (JumpEffect, playerFacing.position, playerFacing.rotation);
            }
			else if(Input.GetKeyUp(GameInputManager.GIM.jump))
            {
                if(velocity.y > 0)
                {
                    velocity.y = velocity.y * .5f;
                }
            }
        }
		else if (Input.GetKeyUp(GameInputManager.GIM.jump))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * .5f;
            }
        }

		if (bGrounded == false && GetComponent<PlayerHealth>().takeDamage == false) {
			animator.SetBool ("jumping", true);
            jumping = true;
		} else if(bGrounded == true){
			animator.SetBool ("jumping", false);
            jumping = false;
		}

        /*bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.0f) : (move.x < 0.0f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }*/
        //animator.SetBool("grounded", grounded);
        //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        targetVelocity = move * maxSpeed;
    }
}
