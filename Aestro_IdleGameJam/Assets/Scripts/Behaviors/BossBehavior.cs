using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;






/// <summary>
/// <para>
/// Handles any possible information and functions the boss may need to consider as well as communicating results
/// </para>
/// </summary>
public class BossBehavior : MonoBehaviour
{
    public static BossBehavior instance;
    public string readableName = "myNameJeoff";
    public int moneyValueOnHit = 50;
    private float healthPoints;
    private float MaxHP;
    public int AttackDice;
    public float AttackBonus;
    public float ChargePerFrame;
    public CinemachineVirtualCamera vcamForDisadvantage;

    public Animator BossAnimator;

    // events that can hold addition outcomes
    public UnityEvent LinkedEvents_OnHit;
    public UnityEvent LinkedEvents_OnHeal;
    public UnityEvent LinkedEvents_OnDeath;
    public UnityEvent LinkedEvents_OnAttack;

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

    private void Start()
    {

    }

    public void InitiateBossData(Boss data)
    {
        GameObject g = Instantiate(data.BossMeshPrefab,this.transform);
        BossAnimator = g.GetComponent<Animator>();
        MaxHP = data.MaxHP;
        healthPoints = data.MaxHP;
        AttackDice = data.AttackDice;
        AttackBonus = data.AttackBonus;
        ChargePerFrame = data.ChargePerFrame;
        UI_Manager.instance.UpdateBossHPText(healthPoints, MaxHP);
        UI_Manager.instance.UpdateLevelNameandBossName(PersistentData.instance.LevelNumber, data.Name);
    }

    // on call, the caller can change the HP of this unit/script.
    // _amount = how much to change
    // _setInstead = when true the hp is set to the new amount
    public void ChangeHP(float _amount, bool _setInstead)
    {
        float healthWas = healthPoints;

        if (_setInstead)
            healthPoints = _amount;
        else
            healthPoints += _amount;

        // react to health change
        HPChangeReact(healthPoints < healthWas);
    }// end of ChangeHP()

    public void BossRollDice()
    {
        AttackEventReceipt attkReceipt = new AttackEventReceipt(AttackEventReceipt.SENDER.Boss, AttackEventReceipt.EVENT_DIRECTION.Attacking, false, PersistentData.instance.RollwithDisadvantage > 0, 0, 0, 0, false, healthPoints, healthPoints, false, 0, false, 0, 0);

        int Result = 1000;
        for (int i = 0; i < 1 + PersistentData.instance.RollwithDisadvantage; i++)
        {
            Result = Mathf.Min(Result, RNG_Manager.instance.RNG(PersistentData.instance.DiceConfig[AttackDice].NumberOfSides));
            if (i == 0) { attkReceipt.highestDie = Result; attkReceipt.lowestDie = Result; }
            if (i != 0 && Result > attkReceipt.highestDie) attkReceipt.highestDie = Result;
            if (i != 0 && Result < attkReceipt.lowestDie) attkReceipt.lowestDie = Result;
        }
        BossAnimator.SetTrigger("Attack");
        LinkedEvents_OnAttack.Invoke();
        StartCoroutine(BossDiceStop(Result.ToString(), attkReceipt));
    }

    public void DealDamageToPlayer(float Damage, AttackEventReceipt _attkRcpt)
    {
        Game_Manager.instance.TakeDamage(Damage, _attkRcpt);
    }   

    // decide if we gained health or lost it and what to do
    private void HPChangeReact(bool _lostHP)
    {
        if (_lostHP)
        {
            print("boss lost hp"); // can set animation & sound too 
            LinkedEvents_OnHit.Invoke();
            UI_Manager.instance.UpdateBossHPText(healthPoints, MaxHP);
            BossAnimator.SetTrigger("TakeHit");
        }
        else
            LinkedEvents_OnHeal.Invoke();

        // play animations
        // play sounds

        // check if HP is too low or high
        if (StillAlive())
        {
            SpawnPlayersVisualEffects();
        }
    }// end of HPChangeReact()

    // check if the hp is too low or high
    private bool StillAlive()
    {
        // give reward
        GiveReward();

        if(healthPoints <= 0)
        {
            BossAnimator.SetTrigger("Die");
            DeathCall();
            //trigger death
            //give extra reward?
            return false;
        }
        return true;
    }// end of StillAlive()

    //reward money message
    private void GiveReward() // maybe add a multiplier option
    {
        print("gimme dat cash muneeee");
        MoneyManager.PlayerMoney += moneyValueOnHit;
    }// end of GiveReward()

    public void SpawnPlayersVisualEffects() // PersistentData.instance.RollwithDisadvantage
    {
        print("DEV-NOTE: Here (spot 2) we can check what cards the player has unlocked and spawn visuals to support that.");
    }

   
    // all actions related to dying
    private void DeathCall()
    {
        print($"This {readableName} character died");
        Game_Manager.instance.ChangeGameState(GAMESTATE.POSTCOMBAT);
        // call any functions related
        LinkedEvents_OnDeath.Invoke();
        // play death animation
        // announce game finish function

    }


    private IEnumerator BossDiceStop(string _numberToShow, AttackEventReceipt _attkRcpt)
    {
        Game_Manager.instance.BossDiceUI.Rolling = false;
        foreach (Transform _child in Game_Manager.instance.BossDiceUI.transform.GetChild(0).transform.GetChild(0))
        {
            if (_child.GetComponent<TMP_Text>())
                _child.GetComponent<TMP_Text>().text = _numberToShow;
        }
        yield return new WaitForSeconds(1);
        DealDamageToPlayer(int.Parse(_numberToShow) + AttackBonus, _attkRcpt);
        yield return new WaitForSeconds(1);
        Game_Manager.instance.BossDiceUI.Rolling = true;
    }

}// end of BossBehavior class
