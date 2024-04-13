using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNG_Manager : MonoBehaviour
{
    public static RNG_Manager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public int RNG (int NumberOfSides)
    {
        return Random.Range(1, NumberOfSides);
    }
}
