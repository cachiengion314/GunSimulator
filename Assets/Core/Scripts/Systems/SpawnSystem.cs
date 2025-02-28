using System;
using System.Collections;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
  public static SpawnSystem Instance { get; private set; }

  [Header("Datas")]
  bool _isCompleted;
  public bool IsCompleted { get { return _isCompleted; } }

  [Header("Events")]
  public Action onCompleted;

  [Header("Dependencies")]
  [SerializeField] GameObject[] prefabSpawns;
  [SerializeField] LevelEditor _levelEditor;

  void Awake()
  {
    if (Instance == null) Instance = this;
    else Destroy(gameObject);
  }

  IEnumerator Start()
  {
    var count = 0;
    foreach (var prefab in prefabSpawns)
    {
      var obj = Instantiate(prefab, transform);

      if (obj.TryGetComponent(out Canvas canvas))
      {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
      }

      if (obj.TryGetComponent(out LevelSystem level))
      {
        level.InjectLevelEditor(_levelEditor);
      }

      count++;
    }

    yield return new WaitUntil(() => count == prefabSpawns.Length);
    _isCompleted = true;
    onCompleted?.Invoke();
  }
}
