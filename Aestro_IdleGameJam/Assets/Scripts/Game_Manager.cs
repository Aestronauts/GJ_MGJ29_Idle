using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;

    [Header("Public Player Stats")]
    public float ChargePerFrame;

    [Header("Exposed Private Vars")]

    [SerializeField]
    private float AttackCharge = 1;

    // Start is called before the first frame update
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

    void Update()
    {
        AttackCharge += ChargePerFrame;
        if (AttackCharge >= 100) 
        {
            Tick();
            AttackCharge = 0;
        }
    }

    public void Tick()
    {
        Debug.Log("Tick!");
        ThrowADice();
    }

    public void Click()
    {
        Debug.Log("Click!");
        ThrowADice();
    }

    public void ThrowADice()
    {
        Debug.Log("Throw a dice!");
        Debug.Log("Value is:" + RNG_Manager.instance.RNG(6));
    }
}
