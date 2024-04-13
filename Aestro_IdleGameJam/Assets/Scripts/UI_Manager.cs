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
}
