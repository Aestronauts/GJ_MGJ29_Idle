using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private static MoneyManager _instance;
    public static MoneyManager Instance_MoneyManager { get { return _instance; } }

    public static int PlayerMoney;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


}// end of MoneyManager class
