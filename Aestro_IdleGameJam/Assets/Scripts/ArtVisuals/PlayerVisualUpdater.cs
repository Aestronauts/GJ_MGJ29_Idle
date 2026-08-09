using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// <para> Meant to handle visuals for the player as needed programatically... This subscribes to GameManager</para>
/// </summary>
public class PlayerVisualUpdater : MonoBehaviour
{
    public MeshFilter diceAboveWand;
    public bool rollingDice;
    private float textTimeStamp;
    public TextMeshProUGUI diceText;
    public RotatingObject diceSpinner;
    public int maxDiceOption = 4;

    // Start is called before the first frame update
    void Start()
    {
        if (Game_Manager.instance) Game_Manager.instance.InjectPVUcs(this);
    }

    public void SetDiceMesh(Mesh _mesh)
    {
        if (!diceAboveWand) { Debug.LogWarning("PlayerVisualUpdater - missing mesh filter reference to dice"); return; }

        diceAboveWand.mesh = _mesh;
    }

    public void SetDiceText(string _text)
    {
        if (diceText) diceText.text = _text;
    }

    public void ChangeDiceSpeed(bool _fast)
    {
        if (diceSpinner)
        {
            float speed = 1;
            if (_fast) speed = 6;
            diceSpinner.UpdateMultiplierSpeed(speed);
        }
    }

    private void LateUpdate()
    {
        if (rollingDice && diceText && Time.time > textTimeStamp + 0.15f)
        {
            // make number spasm between 1-max
            int randomNum = Random.Range(1, maxDiceOption+1);
            SetDiceText(randomNum.ToString());
            textTimeStamp = Time.time;
        }
    }


}
