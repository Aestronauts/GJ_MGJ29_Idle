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
    private int LevelNumber = 1;

    [Header("Public Player Stats")]
    public float MaxHP;
    public float ChargePerFrame;
    public float AttackBonus;
    public int Dice = 0;

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
        MaxHP = 50;
        ChargePerFrame = 0.05f;
        AttackBonus = 0;
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
