using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// <para>
///  Rotating Object is meant to 
///   </para>
///   <para>
///  Event Handler holds a variable for type, and one common method to trigger it based on needed action. 
/// </para>
/// </summary>

public class RotatingObject : MonoBehaviour
{
    public Transform objectToRotate;
    [Range(0, 20)]
    public float multiplier = 1;
    private float storedMultiplier;

    // a set of bools to decide which way it rotates
    public bool rotateX, rotateY, rotateZ;

    // the speed to rotate by
    public Vector3 rotateDirection;


    private void Start()
    {
        if (!objectToRotate) objectToRotate = transform;
    }

    public void UpdateMultiplierSpeed(float _newMultiplier)
    {
        StartCoroutine(SmoothSpeedChange(_newMultiplier));
    }

    // the update method
    private void Update()
    {
        // rotate on X, then rotate by speed
        if(rotateX == true) { objectToRotate.Rotate(rotateDirection.x * Time.deltaTime * multiplier, 0, 0);}

        // rotate on Y, then rotate by speed
        if (rotateY == true) { objectToRotate.Rotate(0,rotateDirection.y * Time.deltaTime * multiplier, 0); }

        // rotate on Z, then rotate by speed
        if (rotateZ == true) { objectToRotate.Rotate(0, 0, rotateDirection.z * Time.deltaTime * multiplier); }
    }// end of Update


    public IEnumerator SmoothSpeedChange(float _newMultiplier)
    {
        storedMultiplier = multiplier;
        float duration = 3f;
        float timeElapsed = 0f;
        float startMultiplier = multiplier;

        while(timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            multiplier = Mathf.Lerp(startMultiplier, _newMultiplier, t);
            yield return null;        
        }

        multiplier = _newMultiplier;

    }


}// end of rotating object script
