using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpInAnArc : MonoBehaviour
{
    // Adjust the speed for the application.
    public float speed = 2.0f;
    public float crashSpeed = 2.0f;

    // The target (cylinder) position.
    public Transform target;
    private Vector3 startPos;
    private float dist_total, dist_current; // NOTE - get distance, then do some weird math to arc up when close to the middle
    private float curve = 0;
    private float curveHighest = 0;

    void Awake()
    {
        // Position the cube at the origin.
        startPos = transform.position;    
        dist_total =  Vector3.Distance(target.position, startPos);
        print($"Dist_Total: {dist_total}");
    }

    void Update()
    {              
        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        dist_current = Vector2.Distance(target.position, transform.position);        
        //print($"Dist_Curr: {dist_current}");

        // move the dice out if they are close to the middle of distance
        if (dist_current > (dist_total*0.5f))
        {
            print("first half");
            curve = dist_total - dist_current;
            curveHighest = curve;
        }
        else
        {
            print("second half");
            if (curve > 0)
                curve -= Time.deltaTime * crashSpeed;
            else
                curve = 0;
        }

        transform.position += new Vector3(0, curve, 0);

    }

}// end of LerpInAnArc class
