using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yrotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation, editable in Unity Editor

    // Update is called once per frame
    void Update()
    {
        // Rotate the GameObject around the Y-axis
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
