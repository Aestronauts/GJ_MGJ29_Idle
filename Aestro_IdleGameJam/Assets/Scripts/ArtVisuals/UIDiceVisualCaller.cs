using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDiceVisualCaller : MonoBehaviour
{
    public GameObject[] diceObjs;

    public void ShowDice(bool _enable)
    {
        StartCoroutine(ShowDiceOnDelay(_enable));
    }

    private IEnumerator ShowDiceOnDelay(bool _enable)
    {
        if (_enable)
            yield return new WaitForSeconds(5f);

        for (int i =0; i <diceObjs.Length; i++)
        {
            if (diceObjs[i]) diceObjs[i].SetActive(_enable);
            if (_enable)
                yield return new WaitForSeconds(0.75f);
            
        }
    }
}
