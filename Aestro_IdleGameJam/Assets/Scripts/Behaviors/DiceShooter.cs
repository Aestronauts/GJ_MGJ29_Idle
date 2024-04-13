using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Shoots and handles the dice
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
            ThrowDice(manualDiceSettings.x, manualDiceSettings.y);
        }
    }

    public void ThrowDice(int _diceID, int _numberOfDice)// can add parameter to set what the dice will say
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
                Destroy(diceThrown, 10);
            }
        }

    }// end of ThrowDice();


}// end of dice shooter
