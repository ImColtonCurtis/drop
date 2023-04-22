using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    bool grounded, audioStopped;
    Vector3 lastGroundPos = Vector3.zero;
    float fallDistance = 0;

    [SerializeField] Animator camShakeAnim;

    [SerializeField] SoundManagerLogic mySoundManager;

    int groundsTouching = 0;

    [SerializeField] Rigidbody myRB;
    [SerializeField] AudioSource ballRollingSource, ballTap;

    float pitchAdjuster = 1.8f, volumeAdjuster = 3f;

    private void Awake()
    {
        grounded = false;
        audioStopped = false;
        groundsTouching = 0;
    }

    private void Update()
    {
     // ball rolling sound effect
     if (GameManager.levelStarted && !GameManager.levelFailed && grounded && !GameManager.levelPassed && myRB.velocity.magnitude >= 0.65f)
        {
            if (!ballRollingSource.isPlaying)
                ballRollingSource.Play();
            ballRollingSource.pitch = Mathf.Clamp(myRB.velocity.magnitude / pitchAdjuster, 0.95f, 1.35f);
            ballRollingSource.volume = Mathf.Min(myRB.velocity.magnitude / volumeAdjuster, 0.73f);
            if (audioStopped)
                audioStopped = false;
        }
     else if (!audioStopped)
        {
            StartCoroutine(FadeSound(ballRollingSource));
            audioStopped = true;
        }
    }
    IEnumerator FadeSound(AudioSource myAudio)
    {
        float timer = 0, totalTime = 7;
        float startVolume = myAudio.volume;

        while (timer <= totalTime)
        {
            myAudio.volume = Mathf.Lerp(startVolume, 0, timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;

            if (!audioStopped)
                break;
        }
        if (audioStopped)
            myAudio.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Lose" && !GameManager.levelFailed)
        {
            GameManager.levelFailed = true;
            mySoundManager.Play("loseJingle"); // winning jingle
        }
    }

    private void OnCollisionEnter(Collision collision)
    {  
        if (collision.gameObject.tag == "Win")
        {
            mySoundManager.Play("ding"); // cup sound

            if (!GameManager.levelPassed)
            {
                mySoundManager.Play("winJingle"); // winning jingle

                if (PlayerPrefs.GetInt("levelScore", 1) < GameManager.levelCount)
                    PlayerPrefs.SetInt("levelScore", PlayerPrefs.GetInt("levelScore", 1) + 1);
                else
                    PlayerPrefs.SetInt("fakeTracker", PlayerPrefs.GetInt("fakeTracker", 0) + 1);
                GameManager.levelPassed = true;
            }
        }
        else if (GameManager.levelStarted && !GameManager.levelPassed && !grounded)
        {
            fallDistance = Vector3.Distance(transform.localPosition, lastGroundPos);
            if (fallDistance >= 1.35f && GameManager.levelStarted && camShakeAnim != null)
                camShakeAnim.SetTrigger("shake");

            grounded = true;
            ballTap.Play(); // land sound            
        }
        groundsTouching++;
    }

    private void OnCollisionExit(Collision collision)
    {
        groundsTouching--;

        if (groundsTouching <= 0)
        {
            grounded = false;
            lastGroundPos = transform.localPosition;
        }
    }
}
