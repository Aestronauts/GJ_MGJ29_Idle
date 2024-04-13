using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;

    [Header("Public Player Stats")]
    public float ChargePerFrame;
    public float AttackBonus;

    [Header("Exposed Private Vars")]

    [SerializeField]
    private float AttackCharge = 1;

    [Header("References")]
    public DiceShooter DiceShooter;
    public Animator PlayerAnimator;
    public Animator BossAnimator;

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
        UI_Manager.instance.UpdateAttackCharge(AttackCharge);
        if (AttackCharge >= 100) 
        {
            Tick();
            AttackCharge = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Click();
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
        int RollResult = RNG_Manager.instance.RNG(6);
        Debug.Log("Value is:" + RollResult);
        DiceShooter.ThrowDice(1, 1, RollResult);
        Attack(CalculateOutgoingDamage(RollResult));

    }

    public float CalculateOutgoingDamage(int RNG_Value)
    {
        float final_value = (float)RNG_Value;
        final_value += AttackBonus;

        return final_value;
    }

    public void Attack(float Outgoing_Damage)
    {
        PlayerAnimator.SetTrigger("Attack");
        BossAnimator.SetTrigger("Attack");
        BossBehavior.instance.ChangeHP(-Outgoing_Damage, false);
    }
}
