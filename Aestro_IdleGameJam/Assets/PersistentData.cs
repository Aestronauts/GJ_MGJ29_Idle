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
    public Vector3 mouthPos;
}

[Serializable]
public class Upgrade
{
    public int ID;
    public int Grade;
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

    [Header("Public Player Stats")] // These are the IDs that will be referenced
    public float MaxHP; // 0
    public float ChargePerFrame; // 1
    public float AttackBonus; // 2
    public int Dice; // 3
    public float Vamp; // 4
    public float SizeBuff; // 5
    public float Armor; // 6
    public float ReturnDamage; // 7
    public int RollwithAdvantage; // 8
    public int RollwithDisadvantage; // 9


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
        LevelNumber = 1;
        MaxHP = 10;//
        ChargePerFrame = 0.225f;//
        AttackBonus = 1;//
        Dice = 0;//
        Vamp = 0;//
        SizeBuff = 1;//
        Armor = 0.2f;//
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
