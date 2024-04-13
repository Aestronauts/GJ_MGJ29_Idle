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

public class PersistentData : MonoBehaviour
{
    public List<Boss> LevelBossConfig;
    public static PersistentData instance;
    [SerializeField]
    private int LevelNumber = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
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
