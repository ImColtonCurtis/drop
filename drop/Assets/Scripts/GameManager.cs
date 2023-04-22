using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool levelStarted, levelFailed, levelPassed, shouldRestart, isRestarting;

    [SerializeField] Transform levelObjectsFolder;

    [SerializeField] Camera myCam;

    [SerializeField] TextMeshPro levelScoreTMP;
    [SerializeField] SpriteRenderer pressText, pressBG, dropText, dropBGIMG, fullSquare, retryText, retryBG;
    [SerializeField] GameObject passedObj;

    bool restartFadedIn, startedLevel, shouldPass;

    [SerializeField] Animator cameraShakeAnim;

    public static int levelCount;

    [SerializeField] SpriteRenderer[] soundIcons;

    // Sounds: PlayerController.cs
    // 
    private void Awake()
    {
        Application.targetFrameRate = 60;

        levelCount = 30;

        StartCoroutine(FadeImageOut(fullSquare, 30));

        restartFadedIn = false;
        shouldRestart = false;
        levelStarted = false;
        levelFailed = false;
        startedLevel = false;
        shouldPass = false;
        levelPassed = false;
        passedObj.SetActive(false);
        isRestarting = false;

        int adder = PlayerPrefs.GetInt("levelScore", 1) + PlayerPrefs.GetInt("fakeTracker", 0);

        levelScoreTMP.text = "Level " + adder;
    }

    private void Update()
    {
        if (levelStarted && !startedLevel)
        {
            foreach (SpriteRenderer sprite in soundIcons)
            {
                StartCoroutine(FadeImageOut(sprite, 24));
            }

            StartCoroutine(FadeImageOut(pressText, 24));
            StartCoroutine(FadeImageOut(pressBG, 24));
            StartCoroutine(FadeImageOut(dropText, 24));
            StartCoroutine(FadeImageOut(dropBGIMG, 24));
            startedLevel = true;
        }

        if (levelFailed && !restartFadedIn)
        {
            StartCoroutine(RestartWait());
            restartFadedIn = true;
        }

        if (levelFailed && shouldRestart)
        {
            StartCoroutine(RestartLevel(fullSquare));
            shouldRestart = false;
        }

        if (levelPassed && !shouldPass)
        {   StartCoroutine(PassLogic());
            shouldPass = true;         
        }
    }

    IEnumerator PassLogic()
    {
        passedObj.SetActive(true);
        yield return new WaitForSeconds(1.4f);
        StartCoroutine(NextLevel(fullSquare));
    }

    IEnumerator NextLevel(SpriteRenderer myImage)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myImage.color;
        myImage.enabled = true;
        while (timer <= totalTime)
        {
            myImage.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
        yield return new WaitForSeconds(0.1f);
        if (Application.CanStreamedLevelBeLoaded(PlayerPrefs.GetInt("levelScore", 1) - 1 + PlayerPrefs.GetInt("fakeTracker", 0)))
            SceneManager.LoadScene(PlayerPrefs.GetInt("levelScore", 1) - 1, LoadSceneMode.Single);
        else
            SceneManager.LoadScene(Random.Range(1, levelCount), LoadSceneMode.Single);
    }

    IEnumerator ShakeCamera()
    {
        yield return new WaitForSeconds(7f/60f);
        cameraShakeAnim.SetTrigger("shake");
    }

    IEnumerator RestartWait()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(FadeImageIn(retryText, 48));
        StartCoroutine(FadeImageIn(retryBG, 47));
    }

    IEnumerator RestartLevel(SpriteRenderer myImage)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myImage.color;
        myImage.enabled = true;
        while (timer <= totalTime)
        {
            myImage.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    IEnumerator FadeImageOut(SpriteRenderer myImage, float totalTime)
    {
        float timer = 0;
        Color startingColor = myImage.color;
        myImage.enabled = true;
        while (timer <= totalTime)
        {
            myImage.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 1), new Color(startingColor.r, startingColor.g, startingColor.b, 0), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
        myImage.enabled = false;
    }

    IEnumerator FadeImageIn(SpriteRenderer myImage, float totalTime)
    {
        float timer = 0;
        Color startingColor = myImage.color;
        myImage.enabled = true;
        while (timer <= totalTime)
        {
            myImage.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    IEnumerator FadeTextOut(TextMeshPro myTtext)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myTtext.color;
        while (timer <= totalTime)
        {
            myTtext.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 1), new Color(startingColor.r, startingColor.g, startingColor.b, 0), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    IEnumerator FadeTextIn(TextMeshPro myTtext)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myTtext.color;
        while (timer <= totalTime)
        {
            myTtext.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }
}
