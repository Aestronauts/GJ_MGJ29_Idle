using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpAnimation : MonoBehaviour
{
    //------------------scale lerping
    [Tooltip("Check if we want to lerp the scale on the Y axis")]
    [SerializeField] private bool lerpScale;
    [Tooltip("A = Min Scale ... B = Max Scale ... Offset is for adjusting speed")]
    [SerializeField] private float scLeVa_a, scLeVa_b, scLeVa_tOffset;
    private float scLeVa_t;
    private bool upScale;
    private float scaleLerpVal;


    // Start is called before the first frame update
    void Start()
    {
        if (scLeVa_tOffset == 0)
            scLeVa_tOffset = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpScale)
            LerpOurScale();
    }

    //scale lerping
    private void LerpOurScale()
    {
       //check time scale
        if (scLeVa_t >= 1)
            upScale = false;
        if (scLeVa_t <= 0)
            upScale = true;

        //increase or decrease scale lerp
        if (upScale)
            scLeVa_t += Time.deltaTime * scLeVa_tOffset;
        if (!upScale)
            scLeVa_t -= Time.deltaTime * scLeVa_tOffset;

        //lerp scale
        scaleLerpVal = Mathf.Lerp(scLeVa_a, scLeVa_b, scLeVa_t);
        //apply it to this obj
        transform.localScale = new Vector3(transform.localScale.x, scaleLerpVal, transform.localScale.z);
    }//end of scale lerping


}//end of lerp animations
