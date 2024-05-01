using UnityEngine;
using TMPro;

public class MoveUpByTime : MonoBehaviour
{
    public Transform objectToMove;
    public float speedOffset = 1;
    public TextMeshProUGUI text_Timescale;

    private void Awake()
    {
        float currentTimescale = Time.timeScale;
        if (text_Timescale)
            text_Timescale.text = currentTimescale.ToString() + "";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (objectToMove)
            objectToMove.Translate(Vector2.up * Time.deltaTime * speedOffset);
    }

    public void ChangeTimeScale()
    {
        float currentTimescale = Time.timeScale;
        print($"Timescale Currently: {currentTimescale}");


        switch (currentTimescale)
        {
            case 0:
                Time.timeScale = 0;
                break;
            case 1:
                Time.timeScale = 2;
                break;
            case 2:
                Time.timeScale = 4;
                break;
            case 4:
                Time.timeScale = 6;
                break;
            case 6:
                Time.timeScale = 1;
                break;
            default:
                Time.timeScale = 1;
                break;
        }
       

        currentTimescale = Time.timeScale;
        if (text_Timescale)
            text_Timescale.text = currentTimescale.ToString() + "";
        print($"Timescale Now: {currentTimescale}");
    }
}
