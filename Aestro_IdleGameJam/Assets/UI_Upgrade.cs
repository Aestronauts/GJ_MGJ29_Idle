using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Upgrade : MonoBehaviour
{
    Upgrade info;
    public TMPro.TextMeshProUGUI Title;
    public Image Icon;
    public TMPro.TextMeshProUGUI Description;

    public void LoadInfo(Upgrade _info)
    {
        info = _info;
        Title.text = info.Name;
        Icon.sprite = info.Icon;
        Description.text = info.Description;
    }

    public void Select()
    {
        UI_Manager.instance.SelectUpgrade(info);
    }
}
