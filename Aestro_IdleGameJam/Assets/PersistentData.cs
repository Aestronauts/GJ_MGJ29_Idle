using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Boss
{
    public string Name;
    public GameObject BossMeshPrefab;
    public float MaxHP;
    public int AttackDice;
    public int AttackBonus;
    public float ChargePerFrame;
}

[Serializable]
public class Upgrade
{
    public int ID;
    public string Name;
    public Sprite Icon;
    public string Description;
}

[Serializable]
public class Dice
{
    public int NumberOfSides;
    public GameObject Prefab;
}


public class PersistentData : MonoBehaviour
{
    public List<Boss> LevelBossConfig;
    public List<Upgrade> UpgradeConfig;
    public List<Dice> DiceConfig;
    public static PersistentData instance;
    [SerializeField]
    public int LevelNumber = 1;

    [Header("Public Player Stats")]
    public float MaxHP;
    public float ChargePerFrame;
    public float AttackBonus;
    public int Dice;
    public float Vamp;
    public float SizeBuff;
    public float Armor;
    public float ReturnDamage;
    public int RollwithAdvantage;
    public int RollwithDisadvantage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void Initialize()
    {
        MaxHP = 50;//
        ChargePerFrame = 0.075f;//
        AttackBonus = 0;//
        Dice = 0;//
        Vamp = 0;//
        SizeBuff = 1;//
        Armor = 0;//
        ReturnDamage = 0;//
        RollwithAdvantage = 0;
        RollwithDisadvantage = 0;
    }

    public Boss ReturnBossData()
    {
        return LevelBossConfig[LevelNumber - 1];
    }
    public void IncreaseLevelNumber()
    {
        LevelNumber++;
    }
}
