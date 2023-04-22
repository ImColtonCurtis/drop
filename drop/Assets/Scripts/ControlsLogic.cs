using Unity.Services.Mediation.Samples;
using UnityEngine;

public class ControlsLogic : MonoBehaviour
{
    public static bool touchedDown;

    [SerializeField] GameObject noIcon;

    [SerializeField] Animator soundAnim;

    void Awake()
    {
        touchedDown = false;
        
        if (PlayerPrefs.GetInt("SoundStatus", 1) == 1)
        {
            noIcon.SetActive(false);
            AudioListener.volume = 1;
        }
        else
        {
            noIcon.SetActive(true);
            AudioListener.volume = 0;
        }
    }

    void OnTouchDown(Vector3 point)
    {
        if (!touchedDown && !GameManager.levelPassed)
        {
            if (ShowAds.poppedUp)
            {
                if (point.x <= 0)
                    ShowAds.shouldShowRewardedAd = true;
                else
                    ShowAds.dontShow = true;
            }
            else
            {
                if (!GameManager.levelStarted && point.x <= -0.01f && point.y <= 7.92f) // bottom left button clicked
                {
                    if (PlayerPrefs.GetInt("SoundStatus", 1) == 1)
                    {
                        PlayerPrefs.SetInt("SoundStatus", 0);
                        noIcon.SetActive(true);
                        AudioListener.volume = 0;
                    }
                    else
                    {
                        PlayerPrefs.SetInt("SoundStatus", 1);
                        noIcon.SetActive(false);
                        AudioListener.volume = 1;
                    }
                    soundAnim.SetTrigger("Blob");
                }
                else
                {
                    if (!GameManager.levelFailed)
                    {
                        touchedDown = true;
                        if (!GameManager.levelStarted)
                            GameManager.levelStarted = true;
                    }
                }
            }
            if (GameManager.levelFailed && !GameManager.isRestarting)
            {
                GameManager.isRestarting = true;
                GameManager.shouldRestart = true;
            }
        }
    }

    void OnTouchUp()
    {
        if (touchedDown)
        {
            touchedDown = false;        
        }
    }

    void OnTouchExit()
    {
        if (touchedDown)
        {
            touchedDown = false;          
        }
    }
}
