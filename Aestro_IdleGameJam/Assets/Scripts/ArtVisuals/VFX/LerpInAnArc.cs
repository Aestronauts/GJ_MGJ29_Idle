using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpInAnArc : MonoBehaviour
{
    // Adjust the speed for the application.
    public float speed = 2.0f;

    // The target (cylinder) position.
    public Transform target;
    private Vector3 startPos;
    private float dist_total, dist_current; // NOTE - get distance, then do some weird math to arc up when close to the middle

    void Awake()
    {
        // Position the cube at the origin.
        startPos = transform.position;         
    }

    void Update()
    {
        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        // move the dice out if they are close to the middle of distance
        if(dist_current >= dist_total / 2)
        {

        }
        else
        {

        }

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            // Swap the position of the cylinder.
            target.position *= -1.0f;
        }
    }

}// end of LerpInAnArc class
