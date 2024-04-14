using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;






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

    // events that can hold addition outcomes
    public UnityEvent LinkedEvents_OnHit;
    public UnityEvent LinkedEvents_OnHeal;
    public UnityEvent LinkedEvents_OnDeath;

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
        UI_Manager.instance.UpdateBossHPText(healthPoints, MaxHP);

    }

    public void InitiateBossData(Boss data)
    {
        Instantiate(data.BossMeshPrefab,this.transform);
        MaxHP = data.MaxHP;
        healthPoints = data.MaxHP;
        AttackDice = data.AttackDice;
        AttackBonus = data.AttackBonus;
        ChargePerFrame = data.ChargePerFrame;
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
        int Result = RNG_Manager.instance.RNG(PersistentData.instance.DiceConfig[AttackDice].NumberOfSides);
        StartCoroutine(BossDiceStop(Result.ToString()));
    }

    public void DealDamageToPlayer(float Damage)
    {
        Game_Manager.instance.TakeDamage(Damage);
    }


    // decide if we gained health or lost it and what to do
    private void HPChangeReact(bool _lostHP)
    {
        if (_lostHP)
        {
            print("boss lost hp"); // can set animation & sound too // LinkedEvents_OnHit.Invoke()
            UI_Manager.instance.UpdateBossHPText(healthPoints, MaxHP);
        }
        else
            print("boss gained hp"); // can set animation & sound too // LinkedEvents_OnHeal.Invoke()

        // play animations
        // play sounds

        // check if HP is too low or high
        CheckHP();
    }// end of HPChangeReact()

    // check if the hp is too low or high
    private void CheckHP()
    {
        // give reward
        GiveReward();

        if(healthPoints <= 0)
        {
            DeathCall();
            //trigger death
            //give extra reward?
        }

    }// end of CheckHP()

    //reward money message
    private void GiveReward() // maybe add a multiplier option
    {
        print("gimme dat cash muneeee");
        MoneyManager.PlayerMoney += moneyValueOnHit;
    }// end of GiveReward()

   
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


    private IEnumerator BossDiceStop(string _numberToShow)
    {
        Game_Manager.instance.BossDiceUI.Rolling = false;
        /*foreach (Transform _child in Game_Manager.instance.BossDiceUI.transform.GetChild(0).transform.GetChild(0))
        {
            if (_child.GetComponent<TMP_Text>())
                _child.GetComponent<TMP_Text>().text = _numberToShow;
        }*/
        yield return new WaitForSeconds(1);
        DealDamageToPlayer(int.Parse(_numberToShow) + AttackBonus);
        yield return new WaitForSeconds(1);
        Game_Manager.instance.BossDiceUI.Rolling = true;
    }

}// end of BossBehavior class
