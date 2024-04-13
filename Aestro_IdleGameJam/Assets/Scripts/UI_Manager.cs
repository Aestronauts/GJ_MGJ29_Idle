using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    public TMPro.TextMeshProUGUI HPText;
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
    }
}
