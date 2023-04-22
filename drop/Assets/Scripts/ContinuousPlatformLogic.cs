using UnityEngine;

public class ContinuousPlatformLogic : MonoBehaviour
{
    [SerializeField] Animator platformAnim;
    [SerializeField] bool isInverse;
    bool platformMoved;

    float counter;
    [SerializeField] float animLength;

    private void Awake()
    {
        platformMoved = false;
        counter = 0;

        platformAnim.Play("Starting_a", 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlsLogic.touchedDown && !platformMoved)
        {
            // move forward
            if (!isInverse)
            {
                //platformAnim.SetTrigger("forward");
                platformAnim.Play("Platform_a", 0, counter / animLength);
            }
            // move backward
            else
            {
                //platformAnim.SetTrigger("back");
                platformAnim.Play("Platform_b", 0, counter / animLength);
            }
            platformMoved = true;
        }
        if (ControlsLogic.touchedDown)
        {
            if (counter < animLength)
                counter++;
            else
                counter = 0;
        }
        if (!ControlsLogic.touchedDown && platformMoved)
        {
            // move forward
            if (!isInverse)
            {
                //platformAnim.SetTrigger("back");
                platformAnim.PlayInFixedTime("Platform_b", 0, counter / animLength); // other way
            }
            // move backward
            else
            {
                //platformAnim.SetTrigger("forward");
                platformAnim.PlayInFixedTime("Platform_a", 0, counter / animLength);
            }
            platformMoved = false;
        }
    }
}
