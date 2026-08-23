using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIAnimationPlayer : MonoBehaviour
{
    public Image targetUIDisplay;
    public Sprite[] keyframesInOrder;

    public int frameID;
    [Range(0, 200)]
    public float framerate = 24f;
    public bool loop;
    public int loopFromFrame;

    private float lastframeTimeStamp;
    private float animationTime;

    public UnityEvent onEnableEvents, onAnimationFinishEvent;


    public void OnEnable()
    {
        onEnableEvents?.Invoke();
    }

    void LateUpdate()
    {
        if (!targetUIDisplay || keyframesInOrder.Length == 0 || framerate == 0)
            return;

        PlayAnimation();
    }

    private void PlayAnimation()
    {
        if (frameID < keyframesInOrder.Length-1)
        {
            animationTime += Time.deltaTime * framerate;
            frameID = (int)animationTime % keyframesInOrder.Length;
        }
        else
        {
            if (loop)
            { frameID = loopFromFrame; animationTime = loopFromFrame; }
            else
            {
                frameID = keyframesInOrder.Length - 1;
                onAnimationFinishEvent?.Invoke();
            }
        }

        targetUIDisplay.sprite = keyframesInOrder[frameID];
    }
}
