using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    public TMPro.TextMeshProUGUI HPText;
    public Image HPBar;
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

    public void UpdateHPText(float HP, float MaxHP)
    {
        HPText.text = HP.ToString() + " / " + MaxHP.ToString();
        HPBar.fillAmount = HP / MaxHP;
    }

    public void UpdateAttackCharge(float Charge)
    {
        AttackChargeBar.fillAmount = Charge/100;
    }

    public void SpawnUpgrades()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject g = Instantiate(UpgradeCardPrefab, UpgradePanel.transform);
        }
    }

    public void SelectUpgrade(Upgrade info)
    {
        if (info.ID == 0)
        {
            PersistentData.instance.AttackBonus += 2;
        }
        else if (info.ID == 1)
        {
            PersistentData.instance.ChargePerFrame *= 1.25f;
        }
        else if (info.ID == 2)
        {
            PersistentData.instance.Dice += 1;
        }
        Game_Manager.instance.LoadNextScene();
    }
}
