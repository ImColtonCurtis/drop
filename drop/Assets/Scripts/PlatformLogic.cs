using UnityEngine;

public class PlatformLogic : MonoBehaviour
{
    [SerializeField] Animator platformAnim;
    [SerializeField] bool isInverse;
    bool platformMoved;

    float counter;

    [SerializeField] float animationLength;

    private void Awake()
    {
        platformMoved = false;
        counter = 0;

        if (isInverse)
            platformAnim.Play("Platform_a", 0, 1);
        else
            platformAnim.Play("Platform_b", 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlsLogic.touchedDown && !platformMoved)
        {
            // move forward
            if (!isInverse)
            {
                platformAnim.Play("Platform_a", 0, counter/ animationLength);
            }
            // move backward
            else
            {
                platformAnim.Play("Platform_b", 0, counter / animationLength);
            }
            platformMoved = true;
        }
        if (ControlsLogic.touchedDown)
        {
            if (counter < animationLength)
                counter++;
        }
        if (!ControlsLogic.touchedDown && platformMoved)
        {
            float fakeCounter = counter;
            fakeCounter -= animationLength;
            fakeCounter *= -1;
            // move forward
            if (!isInverse)
            {
                platformAnim.Play("Platform_b", 0, fakeCounter / animationLength); // other way
            }
            // move backward
            else
            {
                platformAnim.Play("Platform_a", 0, fakeCounter / animationLength);
            }                        
            platformMoved = false;
        }
        if (!ControlsLogic.touchedDown)
        {
            if (counter > 0)
                counter--;
        }
    }
}
