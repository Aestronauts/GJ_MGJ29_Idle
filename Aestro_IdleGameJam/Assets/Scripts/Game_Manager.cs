using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using TMPro;



public enum GAMESTATE
{
    PRECOMBAT,
    INCOMBAT,
    PAUSECOMBAT,
    POSTCOMBAT
};

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;

    [Header("Player Stats")]
    public float HP;

    [Header("Exposed Private Vars")]

    public GAMESTATE GameState;
    [SerializeField]
    private float AttackCharge = 1;
    [SerializeField]
    private float BossAttackCharge = 1;

    [Header("References")]
    public DiceShooter DiceShooter;
    public UI_3D_Dice PlayerDiceUI;
    public Animator PlayerAnimator;
    //public Animator BossAnimator;

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
        GameState = GAMESTATE.INCOMBAT;

    }

    private void Start()
    {
        BossBehavior.instance.InitiateBossData(PersistentData.instance.ReturnBossData());
        HP = PersistentData.instance.MaxHP;
        PlayerDiceUI.Rolling = true;
    }

    void Update()
    {
        if (GameState == GAMESTATE.INCOMBAT)
        {
            AttackCharge += PersistentData.instance.ChargePerFrame;
            UI_Manager.instance.UpdateAttackCharge(AttackCharge);
            BossAttackCharge += BossBehavior.instance.ChargePerFrame;
            if (AttackCharge >= 100)
            {
                Tick();
                AttackCharge = 0;
            }
            if (BossAttackCharge >= 100)
            {
                BossBehavior.instance.BossRollDice();
                BossAttackCharge = 0;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Click();
            }
        }
    }

    public void ChangeGameState(GAMESTATE s)
    {
        GameState = s;
        if (s == GAMESTATE.INCOMBAT)
        {

        }
        else if (s== GAMESTATE.POSTCOMBAT)
        {
            UI_Manager.instance.SpawnUpgrades();
        }
    }

    public void LoadNextScene()
    {
        PersistentData.instance.IncreaseLevelNumber();
        SceneManager.LoadScene(1);
    }

    public void Tick()
    {
        Debug.Log("Tick!");
        ThrowADice();
    }

    public void TakeDamage(float Damage)
    {
        Debug.Log("Receive Damage!");
        HP -= Damage;
        if (HP < 0)
        {
            //Player Die
        }
    }

    public void Click()
    {
        Debug.Log("Click!");
        //ThrowADice();
    }

    public void ThrowADice()
    {
        Debug.Log("Throw a dice!");
        int RollResult = RNG_Manager.instance.RNG(PersistentData.instance.DiceConfig[PersistentData.instance.Dice].NumberOfSides);
        Debug.Log("Value is:" + RollResult);
        //DiceShooter.ThrowDice(PersistentData.instance.Dice, 1, RollResult);
        StartCoroutine(DiceStop(RollResult.ToString()));
    }

    public float CalculateOutgoingDamage(int RNG_Value)
    {
        float final_value = (float)RNG_Value;
        final_value += PersistentData.instance.AttackBonus;

        return final_value;
    }

    public void Attack(float Outgoing_Damage)
    {
        PlayerAnimator.SetTrigger("Attack");
        //BossAnimator.SetTrigger("Attack");
        BossBehavior.instance.ChangeHP(-Outgoing_Damage, false);
    }

    private IEnumerator DiceStop(string _numberToShow)
    {
        PlayerDiceUI.Rolling = false;
        foreach (Transform _child in PlayerDiceUI.transform.GetChild(0).transform.GetChild(0))
        {
            if (_child.GetComponent<TMP_Text>())
                _child.GetComponent<TMP_Text>().text = _numberToShow;
        }
        yield return new WaitForSeconds(1);
        Attack(CalculateOutgoingDamage(int.Parse(_numberToShow)));
        yield return new WaitForSeconds(1);
        PlayerDiceUI.Rolling = true;
    }
}
