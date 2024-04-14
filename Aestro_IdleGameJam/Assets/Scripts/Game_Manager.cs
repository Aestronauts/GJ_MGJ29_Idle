using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using UnityEditor.SearchService; // doesnt build
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Mathematics;
using UnityEngine.Playables;



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
    public static bool GameIsPaused = false;

    [Header("Player Stats")]
    public float HP;

    [Header("Exposed Private Vars")]

    public GAMESTATE GameState;
    private GAMESTATE GameState_BeforePause;
    private float timescale_beforePause = 1;
    public GameObject pauseMenuObj;
    [SerializeField]
    private float AttackCharge = 1;
    [SerializeField]
    private float BossAttackCharge = 1;

    [Header("References")]
    public DiceShooter DiceShooter;
    public UI_3D_Dice PlayerDiceUI;
    public UI_3D_Dice BossDiceUI;
    public Animator PlayerAnimator;
    //public Animator BossAnimator;

    public GameObject SequenceObject;

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
        BossDiceUI.Dice = Instantiate(PersistentData.instance.DiceConfig[BossBehavior.instance.AttackDice].Prefab, BossDiceUI.transform).transform;
        //BossDiceUI.Dice.gameObject.layer = 7;
        HP = PersistentData.instance.MaxHP;
        PlayerDiceUI.Dice = Instantiate(PersistentData.instance.DiceConfig[PersistentData.instance.Dice].Prefab, PlayerDiceUI.transform).transform;
        //PlayerDiceUI.Dice.gameObject.layer = 7;
        PlayerDiceUI.Rolling = true;
        BossDiceUI.Rolling = true;
        UI_Manager.instance.UpdatePlayerHPText(HP, HP);
    }

    void Update()
    {
        if (GameState == GAMESTATE.INCOMBAT)
        {
            AttackCharge += PersistentData.instance.ChargePerFrame;
            BossAttackCharge += BossBehavior.instance.ChargePerFrame;
            UI_Manager.instance.UpdateAttackCharge(AttackCharge, BossAttackCharge);
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
        // at the end of update
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            TogglePause();
    }

    public void TogglePause()
    {
        if (!GameIsPaused)
        {
            GameIsPaused = true;
            GameState_BeforePause = GameState;
            GameState = GAMESTATE.PAUSECOMBAT;
            if(pauseMenuObj)
                pauseMenuObj.SetActive(GameIsPaused);
            timescale_beforePause = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            GameIsPaused = false;
            GameState = GameState_BeforePause;
            pauseMenuObj.SetActive(GameIsPaused);
            Time.timeScale = timescale_beforePause;
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
            StartCoroutine(EndOfLevelSequence());
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
        UI_Manager.instance.UpdatePlayerHPText(HP, PersistentData.instance.MaxHP);
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
        /*foreach (Transform _child in PlayerDiceUI.transform.GetChild(0).transform.GetChild(0))
        {
            if (_child.GetComponent<TMP_Text>())
                _child.GetComponent<TMP_Text>().text = _numberToShow;
        }*/
        yield return new WaitForSeconds(1);
        Attack(CalculateOutgoingDamage(int.Parse(_numberToShow)));
        yield return new WaitForSeconds(1);
        PlayerDiceUI.Rolling = true;
    }

    private IEnumerator EndOfLevelSequence()
    {
        PlayerAnimator.gameObject.SetActive(false);
        BossBehavior.instance.gameObject.SetActive(false);
        SequenceObject.SetActive(true);
        SequenceObject.GetComponent<PlayableDirector>().Play();
        yield return new WaitForSeconds(11);
        UI_Manager.instance.SpawnUpgrades();
    }
}
