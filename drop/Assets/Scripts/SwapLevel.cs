using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.GetInt("levelScore", 1) != 1)
        {
            if (Application.CanStreamedLevelBeLoaded(PlayerPrefs.GetInt("levelScore", 1) - 1 + PlayerPrefs.GetInt("fakeTracker", 0)))
                SceneManager.LoadScene(PlayerPrefs.GetInt("levelScore", 1) - 1, LoadSceneMode.Single);
            else
                SceneManager.LoadScene(Random.Range(1, GameManager.levelCount), LoadSceneMode.Single);
        }
    }
}
