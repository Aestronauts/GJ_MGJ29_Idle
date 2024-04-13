using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int Money;
    
    // a signal to send when a player clicks on this object. It will call any Actors listening
    // for more on events and signal/listers see <https://youtu.be/TdiN18PR4zk?si=xeBVjO72MGBHy51P&t=177>
    public delegate void TradeMoney(int _moneyTraded);
    public static event TradeMoney tradeMoney;



}// end of MoneyManager class
