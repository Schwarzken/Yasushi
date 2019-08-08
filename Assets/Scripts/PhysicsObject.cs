using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float minGroundNormalY = 0.65f;
    public float gravityModifier = 1.0f;

    protected Vector2 targetVelocity;
    protected Vector2 otherForces;
    protected bool bGrounded;
    protected bool shouldJump; // A flag for NPCs, especially enemy AI to react to wall and pit
    public Vector2 groundNormal;
    protected Rigidbody2D rgb2D;

    // The only collider that will be casting ray. It can be assigned manually through the inspector, in case of multiple collider components 
    // exist in the same game object. Left none and it will be assigned with code.
    [SerializeField] protected Collider2D mainCollider;

    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.0001f;
    // The smaller shellRadius is, the smaller the gap between its collider and its colliding surface.
    protected const float shellRadius = 0.018f;

    bool castRay = true;
    Collider2D cachedCollider;
    

    void OnEnable()
    {
        if (this.tag != "VBoard")
        {
            rgb2D = GetComponent<Rigidbody2D>();
        }
        if (mainCollider == null)
            mainCollider = GetComponentInParent<Collider2D>();
    }

    // Use this for initialization
    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    // Forces deplete overtime.
    public void AddForce(float horizontal_force = 0.0f, float vertical_force = 0.0f)
    {
        otherForces.x = horizontal_force;
        //otherForces.y = vertical_force;
        velocity.y = vertical_force;
    }

    void FixedUpdate()
    {
        bGrounded = false;
        shouldJump = false;

        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x + otherForces.x;

        otherForces *= 0.5f;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false); // Move along local x-axis of colliding surface (ground)

        move = Vector2.up * deltaPosition.y;

        Movement(move, true); // Move along global y-axis.
    }

    float offDuration = 0.04f; // Switch off the raycast for at least 0.4s (2 fixed frames) so that the player can go into the collider.
    float reCastTime;

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (cachedCollider && Time.time - reCastTime > 0)
        { // Entered a one way platform and ray has not been casted for 'offDuration'.
            if (!cachedCollider.IsTouching(mainCollider)) // Left the collider
            {
                castRay = true;
                cachedCollider = null;
            }
        }

        if (distance > minMoveDistance && castRay)
        {
            int count = mainCollider.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

            Debug.DrawRay(transform.position, new Vector3(move.x, move.y, transform.position.z) * 10, Color.cyan);

            hitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            // Check the normals
            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    if (!bGrounded)
                    {
                        bGrounded = true;
                        OnImpact(velocity);
                    }

                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                if(this.tag == "Player")
                {
                    if (/*!bGrounded &&*/hitBufferList[i].transform.CompareTag("OneWayCollider")) // Make sure to tag the one way platform as "OneWayCollider"
                    {
                        if (currentNormal.y < -minGroundNormalY || Mathf.Abs(currentNormal.x) > 0.85f) // Hit the bottom of a one-way platform or the both sides of the platform
                        {
                            castRay = false;
                            cachedCollider = hitBufferList[i].collider; // Save the one way platform since we stopped casting ray.
                            reCastTime = Time.time + offDuration;
                            return;
                        }
                    }
                }
                
                if (Mathf.Abs(currentNormal.x) > 0.8f) // Hitting a 80% or above vertical wall.
                    shouldJump = true;

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                if (castRay)
                {
                    float modifiedDistance = hitBufferList[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
        }
        if(this.tag != "VBoard")
        {
            rgb2D.position = rgb2D.position + move.normalized * distance;
        }

        }

    protected virtual void OnImpact(Vector3 currentVel) { }
}