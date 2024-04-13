using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// <para>
/// Shoots and handles the dice information
/// </para>
/// </summary>
public class DiceShooter : MonoBehaviour
{
    public float diceForce = 250;
    public Vector2Int manualDiceSettings = new Vector2Int(0, 1);
    public List<GameObject> DiceList = new List<GameObject>();
    float randomForceX, randomForceY, randomForceZ = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowDice(manualDiceSettings.x, manualDiceSettings.y, 1);
        }
    }

    public void ThrowDice(int _diceID, int _numberOfDice, int _diceLandsOn)// can add parameter to set what the dice will say
    {
        if (_numberOfDice < 1)
            _numberOfDice = 1;

        for (int i = 0; i < _numberOfDice; i++)
        {
            if (DiceList.Count > 0 && DiceList.Count >= _diceID)
            {
                Vector3 offset = transform.position + (transform.forward * (i + 1));
                GameObject diceThrown = Instantiate(DiceList[_diceID], transform.position, transform.rotation);
                if (!diceThrown.transform.GetComponent<Rigidbody>())
                {
                    diceThrown.AddComponent<Rigidbody>();
                }

                //random force
                randomForceX = Random.Range(0, 250); // https://www.youtube.com/watch?v=rg4rgDdWyPk 
                randomForceY = Random.Range(0, 250);
                randomForceZ = Random.Range(0, 250);

                diceThrown.transform.GetComponent<Rigidbody>().AddForce(transform.forward * diceForce);
                StartCoroutine(DiceRollHandler(diceThrown, _diceLandsOn.ToString())); // dice details
            }
        }

    }// end of ThrowDice();


    private IEnumerator DiceRollHandler(GameObject _diceObj, string _numberToShow)
    {
        yield return new WaitForSeconds(3);

        foreach(Transform _child in _diceObj.transform.GetChild(0))
        {
            if (_child.GetComponent<TMP_Text>())
                _child.GetComponent<TMP_Text>().text = _numberToShow;
        }
    }


}// end of dice shooter
