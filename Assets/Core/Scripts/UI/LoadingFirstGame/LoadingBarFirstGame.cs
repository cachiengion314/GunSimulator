using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HoangNam;

public class LoadingBarFirstGame : MonoBehaviour
{
  [SerializeField] Slider loadingBar;
  public static string sceneName = KeyStr.NAME_SCENE_LOBBY;
  private void Awake()
  {
    if (loadingBar == null) loadingBar = GetComponent<Slider>();
  }
  private void Start()
  {
    float timeLoading = Random.Range(0.6f, 1f);
    StartCoroutine(LoadSceneAysnc(timeLoading));
  }
  IEnumerator LoadSceneAysnc(float duration, float elapsed = 0f)
  {
    loadingBar.value = 0;
    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
    asyncOperation.allowSceneActivation = false;

    yield return new WaitUntil(() => FirebaseSetup.Instance.IsCompletedUpdate);

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
}
