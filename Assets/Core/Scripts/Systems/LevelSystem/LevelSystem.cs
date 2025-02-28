using System;
using DG.Tweening;
using Firebase.Analytics;
using HoangNam;
using Unity.Mathematics;
using UnityEngine;

public enum LevelDifficulty
{
  Noob,
  Beginner,
  Easy,
  Normal,
  Hard,
  VeryHard,
  Impossible,
}

public partial class LevelSystem : MonoBehaviour
{
  public static LevelSystem Instance { get; private set; }

  [Header("Level dependencies")]
  [SerializeField] LevelEditor levelEditor;
  public void InjectLevelEditor(LevelEditor levelEditor)
  {
    this.levelEditor = levelEditor;
  }

  [Header("Datas")]
  LevelDifficulty _levelDifficulty;
  public LevelDifficulty LevelDifficulty
  {
    get { return _levelDifficulty; }
    set
    {
      _levelDifficulty = value;
    }
  }

  [Tooltip("Using this value to mesure how many time has passed since start game moment")]
  public float GameplayStartTime;

  [Range(.2f, 10)]
  public float IntervalLength;
  public bool ShouldIntervalTimer = false;
  public float IntervalTimer { get; private set; }

  [Header("Events")]
  public Action onStartCurrentLevel;
  public static Action onInitedLevel;

  void Start()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else Destroy(gameObject);

    IntervalTimer = IntervalLength;

    TouchDetect.Instance.onTouchBegan += TouchDetect_onTouchBegan;
    TouchDetect.Instance.onTouchMoved += TouchDetect_onTouchMoved;
    TouchDetect.Instance.onTouchEnd += TouchDetect_onTouchEnd;

    SpawnSystem.Instance.onCompleted += OnSpawnedCompleted;
  }

  void Update()
  {
    if (!ShouldIntervalTimer) return;
    IntervalTimerInvoker();
  }

  void OnDestroy()
  {
    TouchDetect.Instance.onTouchBegan -= TouchDetect_onTouchBegan;
    TouchDetect.Instance.onTouchMoved -= TouchDetect_onTouchMoved;
    TouchDetect.Instance.onTouchEnd -= TouchDetect_onTouchEnd;

    SpawnSystem.Instance.onCompleted -= OnSpawnedCompleted;
  }

  private void OnSpawnedCompleted()
  {
    ShouldIntervalTimer = true;
    GameSystem.Instance.SetGameState(GameState.Gameplay);
    DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(1250, 100);

    SetupCurrentLevel();
  }

  public LevelDesignObj GetCurrentLevelDesign()
  {
    var _levelDesginObjs = levelEditor.LevelDesignObjs;
    if (
      GameSystem.Instance.CurrentLevelIndex >= 0
      && GameSystem.Instance.CurrentLevelIndex <= _levelDesginObjs.Length - 1
    )
      return _levelDesginObjs[GameSystem.Instance.CurrentLevelIndex];
    if (GameSystem.Instance.CurrentLevelIndex > _levelDesginObjs.Length - 1)
      return _levelDesginObjs[^1];
    return _levelDesginObjs[0];
  }

  void IntervalTimerInvoker()
  {
    IntervalTimer -= Time.deltaTime;
    if (IntervalTimer > 0) return;
    IntervalTimer = IntervalLength;
    OnIntervalInvoke();
  }

  void OnIntervalInvoke()
  {

  }

  public void ClearCurrentLevel()
  {

  }

  // Start
  public void SetupCurrentLevel()
  {
    var levelDesignObj = GetCurrentLevelDesign();
    switch (GameSystem.Instance.IdShopMode)
    {
      case 0:
        ItemSystem.Instance.SpawnGunBy(
          GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick
        );
        break;
      case 1:
        break;
      case 2:
        break;
      case 3:
        break;
    }

    /// events
    onInitedLevel?.Invoke();
    if (FirebaseSetup.Instance.IsFirebaseReady)
    {
      FirebaseAnalytics.LogEvent(
        HoangNam.KeyStr.FIREBASE_START_LEVEL_ + (GameSystem.Instance.CurrentLevelIndex + 1)
      );
    }
  }

  public void StartNextLevel()
  {
    GameSystem.Instance.CurrentLevelIndex++;

    ClearCurrentLevel();
    SetupCurrentLevel();
  }

  private void TouchDetect_onTouchBegan(float2 touchPos, Collider2D[] cols)
  {
    OnTouchBegan(touchPos, cols);
  }

  private void TouchDetect_onTouchMoved(float2 touchPos, float2 touchingDir)
  {
    OnTouchMoved(touchPos, touchingDir);
  }

  private void TouchDetect_onTouchEnd(float2 touchPos, float2 touchingDir)
  {
    OnTouchEnd(touchPos, touchingDir);
  }

  public void OnLoseLevel()
  {
    GameSystem.Instance.SetGameState(GameState.Pause);
    if (GameSystem.Instance.CurrentHeartInfinity == 0)
    {
      GameSystem.Instance.CurrentHeart--;
    }
    ShowLoseModal();

    if (FirebaseSetup.Instance.IsFirebaseReady)
    {
      FirebaseAnalytics.LogEvent(
        HoangNam.KeyStr.FIREBASE_LOSE_LEVEL_ + (GameSystem.Instance.CurrentLevelIndex + 1)
      );
    }
  }

  public void OnCompletedLevel()
  {
    GameSystem.Instance.SetGameState(GameState.Pause);
    ShowCompletedModal();

    if (FirebaseSetup.Instance.IsFirebaseReady)
    {
      FirebaseAnalytics.LogEvent(
        HoangNam.KeyStr.FIREBASE_WIN_LEVEL_ + (GameSystem.Instance.CurrentLevelIndex + 1)
      );
    }
  }

  public void ShowLoseModal()
  {
    Sequence seq = DOTween.Sequence();

    seq.AppendInterval(.9f).OnComplete(() =>
    {
      UIManager.Instance.Show(KeyStr.NAME_LOSE_2_LEVEL_MODAL);
    });
  }

  public void ShowCompletedModal()
  {
    SoundSystem.Instance.PlayLevelCompleteSfx();

    Sequence seq = DOTween.Sequence();
    seq.AppendInterval(0.0f).OnComplete(() =>
    {
      UIManager.Instance.Show(HoangNam.KeyStr.NAME_WIN_LEVEL_MODAL);
    });
  }
}