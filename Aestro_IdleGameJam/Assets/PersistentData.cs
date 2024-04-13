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
}

[Serializable]
public class Upgrade
{
    public int ID;
    public string Name;
    public string Description;
}

public class PersistentData : MonoBehaviour
{
    public List<Boss> LevelBossConfig;
    public List<Upgrade> UpgradeConfig;
    public static PersistentData instance;
    [SerializeField]
    private int LevelNumber = 1;

    [Header("Public Player Stats")]
    public float ChargePerFrame;
    public float AttackBonus;
    public int Dice = 1;

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
