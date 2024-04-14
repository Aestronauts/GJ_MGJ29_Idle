using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    [Header("BossUI")]
    public TMPro.TextMeshProUGUI HPText;
    public TMPro.TextMeshProUGUI BossNameText;
    public TMPro.TextMeshProUGUI LevelNumberText;
    public Image HPBar;
    public Image BossAttackChargeBar;
    [Header("PlayerUI")]
    public TMPro.TextMeshProUGUI PlayerHPText;
    public Image PlayerHPBar;
    public Image AttackChargeBar;
    public TMPro.TextMeshProUGUI AttackBonusStat;
    public TMPro.TextMeshProUGUI AttackSpeedStat;
    public GameObject UpgradePanel;
    public GameObject UpgradeCardPrefab;
    private void Awake()
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
        AttackSpeedStat.text = PersistentData.instance.ChargePerFrame.ToString();
        AttackBonusStat.text = PersistentData.instance.AttackBonus.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLevelNameandBossName(int Level, string BossName)
    {

        BossNameText.text = BossName;
        LevelNumberText.text = Level.ToString();
    }

    public void UpdateBossHPText(float HP, float MaxHP)
    {
        HPText.text = HP.ToString() + " / " + MaxHP.ToString();
        HPBar.fillAmount = HP / MaxHP;
    }

    public void UpdatePlayerHPText(float HP, float MaxHP)
    {
        PlayerHPText.text = HP.ToString() + " / " + MaxHP.ToString();
        PlayerHPBar.fillAmount = HP / MaxHP;
    }

    public void UpdateAttackCharge(float PlayerCharge, float BossCharge)
    {
        AttackChargeBar.fillAmount = PlayerCharge / 100;
        BossAttackChargeBar.fillAmount = BossCharge / 100;
    }

    public void SpawnUpgrades()
    {
        if (UpgradePanel.transform.childCount > 0)
        {
            foreach (GameObject g in UpgradePanel.transform)
            {
                Destroy(g);
            }
        }
        List<int> i = new List<int>();
        while (i.Count < 3)
        {
            int j = Random.Range(0, 10);
            if (!i.Contains(j))
            {
                i.Add(j);
            }
        }

        for (int z = 0; z < 3; z++)
        {
            GameObject g = Instantiate(UpgradeCardPrefab, UpgradePanel.transform);
            g.GetComponent<UI_Upgrade>().LoadInfo(PersistentData.instance.UpgradeConfig[i[z]]);
        }
    }

    public void SelectUpgrade(Upgrade info)
    {
        if (info.ID == 0) // attack bonus
        {
            PersistentData.instance.AttackBonus += 2;
        }
        else if (info.ID == 1) // attack speed bonus
        {
            PersistentData.instance.ChargePerFrame *= 1.5f;
        }
        else if (info.ID == 2) // level up dice
        {
            PersistentData.instance.Dice += 1;
        }
        else if (info.ID == 3) // vamp
        {
            PersistentData.instance.Vamp += 0.2f;
        }
        else if (info.ID == 4) // Big Boi
        {
            PersistentData.instance.SizeBuff *= 1.5f;
            PersistentData.instance.MaxHP *= 1.5f;
        }
        else if (info.ID == 5) // Armor
        {
            PersistentData.instance.Armor += 0.1f;
        }
        else if (info.ID == 6) // Spikes
        {
            PersistentData.instance.ReturnDamage += 0.1f;
        }
        else if (info.ID == 7) // RollwithAdvantage
        {
            PersistentData.instance.RollwithAdvantage += 1;
        }
        else if (info.ID == 8) // RollwithAdvantage
        {
            PersistentData.instance.RollwithDisadvantage += 1;
        }
        else if (info.ID == 9) // RollwithAdvantage
        {
            PersistentData.instance.SizeBuff *= 1.25f;
            PersistentData.instance.MaxHP *= 1.15f;
            PersistentData.instance.Vamp += 0.1f;
            PersistentData.instance.ChargePerFrame *= 1.2f;
            PersistentData.instance.AttackBonus += 0.5f;
            PersistentData.instance.Armor += 0.05f;
            PersistentData.instance.ReturnDamage += 0.05f;
        }
        else if (info.ID == 10)
        {
            PersistentData.instance.AttackBonus += 1;
        }
        else if (info.ID == 11) // attack speed bonus
        {
            PersistentData.instance.ChargePerFrame *= 1.25f;
        }
        else if (info.ID == 12) // vamp
        {
            PersistentData.instance.Vamp += 0.1f;
        }
        else if (info.ID == 13) // Big Boi
        {
            PersistentData.instance.SizeBuff *= 1.25f;
            PersistentData.instance.MaxHP *= 1.25f;
        }
        else if (info.ID == 14) // Big Boi
        {
            PersistentData.instance.SizeBuff *= 5;
            PersistentData.instance.MaxHP *= 2;
        }
        if (info.ID == 15) // attack bonus
        {
            PersistentData.instance.AttackBonus += 4;
        }
        else if (info.ID == 16) // Armor
        {
            PersistentData.instance.Armor += 0.2f;
        }
        else if (info.ID == 17) // Armor
        {
            PersistentData.instance.ChargePerFrame *= 2;
        }
        Game_Manager.instance.LoadNextScene();
    }
}
