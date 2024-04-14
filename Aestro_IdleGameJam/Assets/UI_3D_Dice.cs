using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_3D_Dice : MonoBehaviour
{
    public Transform Dice;
    public bool Rolling;

    // Update is called once per frame
    void Update()
    {
        if (Rolling)
        {
            Dice.Rotate(new Vector3(15, 30, 45) * 10 * Time.deltaTime);
        }
    }
}
