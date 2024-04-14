using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpInAnArc : MonoBehaviour
{
    // Adjust the speed for the application.
    public float speed = 5.0f;
    public float crashSpeed = 5.0f;
    public float timeToRise = 1f;
    public float curveInfluence = 0.025f;

    // The target (cylinder) position.
    public Transform target;
    private Vector3 startPos;
    private float dist_total, dist_current; // NOTE - get distance, then do some weird math to arc up when close to the middle
    private float curve = 0;
    private float curveHighest = 0;

    private bool goUp = true;

    void Awake()
    {
        // Position the cube at the origin.
        startPos = transform.position;    
        dist_total =  Vector3.Distance(target.position, startPos);
        print($"Dist_Total: {dist_total}");
    }

    private void OnEnable()
    {
        goUp = true;
        transform.LookAt(null);
        ActivateArc();
    }

    void Update()
    {              
        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move        
        dist_current = Vector2.Distance(target.position, transform.position);        
        

        // move the dice out if they are close to the middle of distance
        if (goUp)
        {
            print("first half");
            curve = (dist_total - dist_current) * curveInfluence;
            curveHighest = curve;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            transform.position += new Vector3(0, curve, 0);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, step* crashSpeed);
        }


        if (Input.GetKeyDown(KeyCode.Space))
            ActivateArc();
    }

    public void ActivateArc()
    {
        goUp = true;
        transform.LookAt(null);
        transform.position = startPos;
        StartCoroutine(StartArc());
    }

    private IEnumerator StartArc()
    {
        yield return new WaitForSeconds(timeToRise);
        goUp = false;
        transform.LookAt(target);        
       
    }

    public void OnTriggerEnter(Collider trig)
    {
        // if boss enemy
        // explode vfx
        // turn off look at
    }

}// end of LerpInAnArc class
