using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CITentacleTip : MonoBehaviour {

    float maxPullRange = 4.0f;
    Vector3 originalPos; // Retract to original position 

    Transform tentacle_end; // The base object in which all the body tiles parented to.
    Transform tentacle_body;
    GameObject[] tbody_instances;
    [SerializeField] float widthOffset = 0.01f;

    Collider2D tipCollider;
    ContactFilter2D contactFilter;
    RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    ChukaIdakoAI parentAI;

    void Awake()
    {
        originalPos = transform.localPosition;
        tentacle_end = transform.parent.Find("tentacle_end");
        tentacle_body = tentacle_end.Find("tentacle_body");
        tipCollider = GetComponent<BoxCollider2D>();
        parentAI = GetComponentInParent<ChukaIdakoAI>();

        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    [SerializeField] float skillDelay = 0.0f;
    float extendedDuration = 0.0f;
    [SerializeField] float maxExtendDuration = 0.8f;
    // [SerializeField] float retractDuration = 0.4f; // retracting is always twice as fast as extruding... Fixed in the logic!
    bool isRetracting = false;

    public bool Extrude(float skillTimer) // ChukaIdakoAI pass the skillTimer variable inherited from EnemyAI as argument.
    {
        int count = tipCollider.Cast(new Vector2(Mathf.Sign(transform.lossyScale.x), 0), contactFilter, hitBuffer, 0.001f);

        if (count > 0 || skillTimer > skillDelay + maxExtendDuration) // If count is greater than 0 -> hit something.
        {
            isRetracting = true;
        }

        if (isRetracting && skillTimer > skillDelay + extendedDuration + extendedDuration / 2 || (extendedDuration > 0 && skillTimer == 0)) // Used skill but skill got reset.
        {
            isRetracting = false;
            return true;
        }

        Vector3 tipLocalPos = transform.localPosition;

        float weight = 0.0f;

        if (isRetracting) // It's time to retract.
        {
            float t = (skillTimer - extendedDuration)/(extendedDuration/2);
            weight = 3 * t * t - 2 * t * t * t;
            tipLocalPos.x = Mathf.Lerp(transform.localPosition.x, originalPos.x, weight);
        }
        else if (skillTimer > skillDelay) // It's time to extend.
        {
            extendedDuration = skillTimer;
            float t = (skillTimer / maxExtendDuration);
            weight = 3 * t * t - 2 * t * t * t;
            tipLocalPos.x = Mathf.Lerp(originalPos.x, originalPos.x + maxPullRange, weight);
        }

        RevealBodies();
        transform.localPosition = tipLocalPos;
        return false;
    }

    void LateUpdate()
    {
        if (parentAI.skillTimer == 0 && extendedDuration > 0)
        {
            transform.localPosition = originalPos;
            extendedDuration = 0.0f;
            RevealBodies();
        }
    }

    public void setPullRange(float skill_range)
    {
        maxPullRange = skill_range;
        CreateBodies();
    }

    float firstZ = 0.01f; // The first instantiated body tile has a Z-pos of 0.01. Next one is 0.02... -> last one.

    void CreateBodies()
    {
        Sprite tbody_sprite = tentacle_body.GetComponent<SpriteRenderer>().sprite;

        float width = tbody_sprite.bounds.extents.x * 2; // The width of the tentacle body tiles.
        width -= widthOffset; // A little offset to make the chain closer.

        // Initiate an array to store the tentacle body objects needed, based on the pull range.
        tbody_instances = new GameObject[(int)(maxPullRange / width)];

        for (int i = 0; i < tbody_instances.Length; i++)
        {   // Load and deactivate the tentacle body objects
            tbody_instances[i] = Instantiate(tentacle_body, tentacle_end.transform, true).gameObject;
            tbody_instances[i].transform.localPosition = new Vector3((i + 1) * width, 0, firstZ * (i + 1)); 
            tbody_instances[i].SetActive(false);
        }

        // Adjust the pull range to fit the tentacle body tiles. Link the tip with the bodies.
        maxPullRange = width * (tbody_instances.Length + 1) - tentacle_end.localPosition.x - originalPos.x;
    }

    void RevealBodies() // Activate the body objects, based on the current position of this transform.
    {
        float dist = Vector2.Distance(transform.position, tentacle_body.position);
        for (int i = 0; i < tbody_instances.Length; i++)
        {
            if (tbody_instances[i].transform.localPosition.x < dist)
            {
                tbody_instances[i].SetActive(true);
            }
            else
            {
                tbody_instances[i].SetActive(false);
            }
        }
    }
}