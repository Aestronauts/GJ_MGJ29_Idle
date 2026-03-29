using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using UnityEditor.SearchService; // doesnt build
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Mathematics;
using UnityEngine.Playables;
using UnityEngine.Events;



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
    private int clickCharge = 0;
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
    public GameObject PlayerDeathSequenceObject;

    public GameObject FakeCave;
    public GameObject DiceProjectilePrefab;
    public GameObject Staff;
    public GameObject Hat;

    public UnityEvent onDiceRollFinish; // NOT SETUP YET - noah
    public UnityEvent onTakeDamage; // NOT SETUP YET - noah

    public List<AttackEventReceipt> attackReceiptsPlayer = new List<AttackEventReceipt>();
    public List<AttackEventReceipt> attackReceiptsBoss = new List<AttackEventReceipt>();

   
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
        UI_Manager.instance.UpdatePlayerStats();
        //size
        PlayerAnimator.gameObject.transform.localScale = Vector3.one * PersistentData.instance.SizeBuff;
        PlayerAnimator.speed *= 1 + PersistentData.instance.ChargePerFrame*5;
        Staff.transform.localScale *= 1+PersistentData.instance.AttackBonus/4;
        Hat.transform.localScale *= 1 + (PersistentData.instance.Armor-0.2f)*1.5f;
        // receipt variables
        attackReceiptsPlayer = new List<AttackEventReceipt>();
        attackReceiptsBoss = new List<AttackEventReceipt>();
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
        if (PersistentData.instance.LevelNumber > PersistentData.instance.LevelBossConfig.Count)
            PlayerWinState();
        else
        {
            attackReceiptsPlayer = null;
            attackReceiptsBoss = null;
            SceneManager.LoadScene(1);
        }
    }

    public void Tick()
    {
        Debug.Log("Tick!");
        ThrowADice();
    }

    public void TakeDamage(float Damage, AttackEventReceipt _attkRcpt)
    {
        Debug.Log("Receive Damage!");
        PlayerAnimator.SetTrigger("Hit");
        if (PersistentData.instance.ReturnDamage > 0)
        {
            Attack(Damage* PersistentData.instance.ReturnDamage, _attkRcpt);
        }
        HP -= (Damage*(1-PersistentData.instance.Armor));
        UI_Manager.instance.UpdatePlayerHPText(HP, PersistentData.instance.MaxHP);
        if (HP < 0)
        {
            StartCoroutine(DieSequence());
        }
    }

    public void PlayerWinState()
    {
        // reset boss persistent progress
        PersistentData.instance.LevelNumber = 1;
        // reset the persistent data
        PersistentData.instance.Initialize();
        // load main menu
        SceneLoader("TextScroll");//SceneLoader("MainMenu");
    }

    public void Click()
    {
        clickCharge++;
        Debug.Log("Click!");
        //ThrowADice();
    }

    public void ThrowADice()
    {
        Debug.Log("Throw a dice!");
        AttackEventReceipt attkReceipt = new AttackEventReceipt(AttackEventReceipt.SENDER.Player, AttackEventReceipt.EVENT_DIRECTION.Attacking, PersistentData.instance.RollwithAdvantage > 0, false, 0, 0, 0, false, HP, HP, false, 0, false, 0, 0);

        int RollResult = 0;
        for (int i = 0;  i < 1+PersistentData.instance.RollwithAdvantage; i++)
        {
           
            RollResult = Mathf.Max(RollResult, RNG_Manager.instance.RNG(PersistentData.instance.DiceConfig[PersistentData.instance.Dice].NumberOfSides));
            print($"Dice Roll = {RollResult}");
            if (i == 0) { attkReceipt.highestDie = RollResult; attkReceipt.lowestDie = RollResult; }
            if (i != 0 && RollResult > attkReceipt.highestDie) attkReceipt.highestDie = RollResult;
            if (i != 0 && RollResult < attkReceipt.lowestDie) attkReceipt.lowestDie = RollResult;
        }
        Debug.Log("Value is:" + RollResult);
        //DiceShooter.ThrowDice(PersistentData.instance.Dice, 1, RollResult);
        CheckAttackVisuals();
        StartCoroutine(DiceStop(RollResult.ToString(), attkReceipt));
    }

    public void StoreAttackReceipt(AttackEventReceipt _attkRcpt)
    {
        if (_attkRcpt == null) return;
        if (_attkRcpt.sender == AttackEventReceipt.SENDER.Player) attackReceiptsPlayer.Add(_attkRcpt);
        if(_attkRcpt.sender == AttackEventReceipt.SENDER.Boss) attackReceiptsBoss.Add(_attkRcpt);
        print($"Attack Receipt Added For: {_attkRcpt.sender.ToString()}");
    }

    public void CheckAttackVisuals()
    {
        print("DEV-NOTE: Here (spot 1) we can check what cards the player has unlocked and spawn visuals to support that.");
    }

    public float CalculateOutgoingDamage(int RNG_Value)
    {
        float final_value = (float)RNG_Value;
        final_value += PersistentData.instance.AttackBonus;

        return final_value;
    }

    public void Attack(float Outgoing_Damage, AttackEventReceipt _attkRcpt)
    {
        PlayerAnimator.SetTrigger("Attack");
        //BossAnimator.SetTrigger("Attack");
        //Vamp       
        HP = Mathf.Max(PersistentData.instance.MaxHP, Outgoing_Damage * PersistentData.instance.Vamp);
        _attkRcpt.healthChanged = PersistentData.instance.Vamp > 0;
        _attkRcpt.newHealth = HP;
        //dmg
        BossBehavior.instance.ChangeHP(-(Outgoing_Damage + clickCharge), false);
        _attkRcpt.totalDamage = Outgoing_Damage;
        clickCharge = 0;

        // final submission of attack
        StoreAttackReceipt(_attkRcpt);

    }

    private IEnumerator DiceStop(string _numberToShow, AttackEventReceipt _attkRcpt)
    {
        PlayerDiceUI.Rolling = false;
        foreach (Transform _child in PlayerDiceUI.transform.GetChild(0).transform.GetChild(0))
        {
            if (_child.GetComponent<TMP_Text>())
                _child.GetComponent<TMP_Text>().text = _numberToShow;
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(AttackSequence(int.Parse(_numberToShow)));
        Attack(CalculateOutgoingDamage(int.Parse(_numberToShow)), _attkRcpt);
        yield return new WaitForSeconds(1);
        PlayerDiceUI.Rolling = true;
    }

    private IEnumerator EndOfLevelSequence()
    {
        PlayerAnimator.gameObject.SetActive(false);
        BossBehavior.instance.BossAnimator.SetTrigger("Die");
        
        SequenceObject.SetActive(true);
        SequenceObject.GetComponent<PlayableDirector>().Play();
        yield return new WaitForSeconds(10);
        UI_Manager.instance.SpawnUpgrades();
    }

    private IEnumerator AttackSequence(int i)
    {
        for(int j = 0; j < i; j++)
        {
            GameObject g = Instantiate(DiceProjectilePrefab, transform.position, quaternion.identity);
            g.GetComponent<Dice_Projectile>().Initiate(PersistentData.instance.DiceConfig[PersistentData.instance.Dice].Prefab, BossBehavior.instance.transform);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator DieSequence()
    {
        PlayerAnimator.gameObject.SetActive(false);
        PlayerDeathSequenceObject.SetActive(true);
        PlayerDeathSequenceObject.GetComponent<PlayableDirector>().Play();
        GameState = GAMESTATE.PAUSECOMBAT;
        yield return new WaitForSeconds(10);
        PersistentData.instance.Initialize();
        SceneManager.LoadScene(0);
    }

    public void SceneLoader(string _sceneToLoad) // NOTE - May want to wait for animation timeline to finish first
    {
        Debug.Log("BACK TO MAIN MENU");
        Time.timeScale = 1;
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        SceneManager.LoadScene(_sceneToLoad);
    }
}

[System.Serializable]
public class AttackEventReceipt
{
    public enum SENDER {None, Player, Boss}
    public SENDER sender;

    public enum EVENT_DIRECTION {None, Attacking, Attacked }
    public EVENT_DIRECTION eventDirection;

    public bool advantageUsed; // ID 8
    public bool disadvantageUsed; // ID 9
    public int highestDie, lowestDie;
    public float totalDamage;

    public bool healthChanged; // ID 0
    public float oldHealth, newHealth;

    public bool armorUsed; // ID 6
    public int damageReduced;

    public bool returnedDamage; // ID 7
    public int damageTaken, damageReturned;

    public AttackEventReceipt(SENDER _sndr, EVENT_DIRECTION _evntDir, bool _adv, bool _dsAdv, int firDie, int secDie, float _ttlDmg, bool hpChng, float oldHp, float newHp, bool armUse, int dmgRed, bool rtrnDmg, int dmgTkn, int dmgRtrn)
    {
        sender = _sndr;
        eventDirection = _evntDir;

        advantageUsed = _adv;
        disadvantageUsed = _dsAdv;
        highestDie = firDie;
        lowestDie = secDie;
        totalDamage = _ttlDmg;

        healthChanged = hpChng;
        oldHealth = oldHp;
        newHealth = newHp;

        armorUsed = armUse;
        damageReduced = dmgRed;

        returnedDamage = rtrnDmg;
        damageTaken = dmgTkn;
        damageReturned = dmgRtrn;
}
}
