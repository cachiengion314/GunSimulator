using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HoangNam;
using TMPro;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] Slider loadingBar;
    [SerializeField] private TMP_Text loadingText;
    public static string sceneName = KeyStr.NAME_SCENE_LOBBY;
    // static bool isStartGame = true;
    private void Awake()
    {
        if (loadingBar == null) loadingBar = GetComponent<Slider>();
    }
    private void Start()
    {
        // if (PlayerPrefs.GetInt(HoangNam.KeyStr.KEY_CURRENT_LEVEL, 0) == 0 && isStartGame)
        // {
        //     sceneName = KeyStr.NAME_SCENE_LOADING;
        //     isStartGame = false;
        // }
        float timeLoading = Random.Range(0.6f, 1f);
        StartCoroutine(LoadSceneAysnc(timeLoading));
        StartCoroutine(AnimateLoadingText());
    }
    IEnumerator LoadSceneAysnc(float duration, float elapsed = 0f)
    {
        loadingBar.value = 0;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            loadingBar.value = Mathf.MoveTowards(0, 1, elapsed / duration);
            elapsed += Time.deltaTime;

            if (loadingBar.value >= 0.99f)
            {
                loadingBar.value = 1;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    IEnumerator AnimateLoadingText()
    {
        while (loadingBar.value < 1)
        {
            loadingText.text = $"Loading {loadingBar.value * 100:F0}%";
            yield return new WaitForSeconds(0.1f);
            loadingText.text = $"Loading {loadingBar.value * 100:F0}%.";
            yield return new WaitForSeconds(0.1f);
            loadingText.text = $"Loading {loadingBar.value * 100:F0}%..";
            yield return new WaitForSeconds(0.1f);
            loadingText.text = $"Loading {loadingBar.value * 100:F0}%...";
            yield return new WaitForSeconds(0.1f);
        }
    }


}
