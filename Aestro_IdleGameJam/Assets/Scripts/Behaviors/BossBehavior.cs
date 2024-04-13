using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// <para>
/// Handles any possible information and functions the boss may need to consider as well as communicating results
/// </para>
/// </summary>
public class BossBehavior : MonoBehaviour
{
    public string readableName;
    [SerializeField] private int healthPoints = 100;
    [SerializeField] private bool dyingTriggered = false;

    public UnityEvent LinkedEvents_OnHit;
    public UnityEvent LinkedEvents_OnHeal;
    public UnityEvent LinkedEvents_OnDeath;

    // on call, the caller can change the HP of this unit/script.
    // _amount = how much to change
    // _setInstead = when true the hp is set to the new amount
    public void ChangeHP(int _amount, bool _setInstead)
    {
        int healthWas = healthPoints;

        if (_setInstead)
            healthPoints = _amount;
        else
            healthPoints += _amount;

        // react to health change
        HPChangeReact(healthPoints < healthWas);
    }// end of ChangeHP()

    // decide if we gained health or lost it and what to do
    private void HPChangeReact(bool _lostHP)
    {
        if (_lostHP)
            print("boss lost hp"); // can set animation & sound too // LinkedEvents_OnHit.Invoke()
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
            //trigger death
            //give extra reward?
        }

    }// end of CheckHP()

    //reward money message
    private void GiveReward() // maybe add a multiplier option
    {
        print("gimme dat cash muneeee");
    }// end of GiveReward()

    // all actions related to dying
    private void DeathCall()
    {
        print($"This {readableName} character died");
        // call any functions related
        LinkedEvents_OnDeath.Invoke();
        // play death animation
        // announce game finish function

    }

}// end of BossBehavior class
