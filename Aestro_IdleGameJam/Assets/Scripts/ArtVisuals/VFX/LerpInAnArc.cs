using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpInAnArc : MonoBehaviour
{
    public float count;
    public Transform midPoint, endPoint;
    private Vector3 startPointPos;

    private void OnEnable()
    {
        startPointPos = transform.position;
        if (endPoint)
            midPoint.position = startPointPos + endPoint.position / 2 + new Vector3(0, Random.Range(-10, 10), 0);
    }

    void Update()
    {
        if (count < 1.0f)
        {
            count += 1.0f * Time.deltaTime;

            Vector3 m1 = Vector3.Lerp(startPointPos, midPoint.position, count);
            Vector3 m2 = Vector3.Lerp(midPoint.position, endPoint.position, count);
            transform.position = Vector3.Lerp(m1, m2, count);
        }
    }


}// end of LerpInAnArc class
