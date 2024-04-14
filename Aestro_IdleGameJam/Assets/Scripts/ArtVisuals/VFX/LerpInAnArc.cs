using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpInAnArc : MonoBehaviour
{
    // Adjust the speed for the application.
    public float speed = 1.0f;
    public float crashSpeed = 20.0f;
    public float timeToRise = 1f;
    public float curveInfluence = 0.005f;

    // The target (cylinder) position.
    public Transform target;
    private Vector3 startPos;
    private float dist_total, dist_current; // NOTE - get distance, then do some weird math to arc up when close to the middle
    private float curve = 0;

    private bool goUp = true;

    private Dice_Projectile refDice_Proj;

    void Awake()
    {
        // Position the cube at the origin.
        startPos = transform.position;    
        

        if (!GameObject.FindGameObjectWithTag("BossTarget"))
        {
            refDice_Proj = transform.GetComponent<Dice_Projectile>();
            refDice_Proj.enabled = true;
            this.enabled = false;
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("BossTarget").transform;
            refDice_Proj = transform.GetComponent<Dice_Projectile>();
            refDice_Proj.enabled = false;
            dist_total = Vector3.Distance(target.position, startPos);
        }
    }

    private void OnEnable()
    {
        goUp = true;
        transform.LookAt(null);
        ActivateArc();
    }

    void FixedUpdate()
    {              
        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move        
        dist_current = Vector2.Distance(target.position, transform.position);        
        

        // move the dice out if they are close to the middle of distance
        if (goUp)
        {
            print("first half");
            curve = (dist_total - dist_current) * curveInfluence;
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
        if(trig.tag == "BossTarget")
        {
            print("EXPLODE ON BOSS");
            // explode vfx


            // turn off look at
            goUp = false;
            transform.LookAt(target);
            Destroy(this, 1.5f);
        }

    }

}// end of LerpInAnArc class
