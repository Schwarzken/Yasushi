using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerFullPackage : PhysicsObject
{
	//PlayerController movement setup
    public Rigidbody2D rb;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    
    public bool isVulnerable = true; // Should be inside health script!
    public bool facingRight; 
    public SpriteRenderer spriteRenderer;
	public Transform playerFacing;

    private Animator animator;


    // Use this for initialization
	void Start()
	{
		facingRight = true;
	}
    void Awake()
    {
		//PlayerMovement start/awake
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
		playerFacing = GetComponent<Transform> ();
    }

	//PlayerMovement
    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

		if (GetComponent<PlayerAttack> ().attacking == false && GetComponent<PlayerConsume>().consuming == false) {
			move.x = Input.GetAxisRaw ("Horizontal");
		}

		animator.SetFloat ("speed", Mathf.Abs (move.x));	

		if (move.x > 0) {
			playerFacing.localScale = new Vector3(1, 1, 1);
		} else if (move.x < 0) {
			playerFacing.localScale = new Vector3(-1, 1, 1);
		}

        if (Input.GetButtonDown("Jump") && bGrounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * .5f;
            }
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
