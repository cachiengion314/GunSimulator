using System;
using DG.Tweening;
using HoangNam;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
  Gameplay,
  Pause,
}

public partial class GameSystem : MonoBehaviour
{
  public static GameSystem Instance { get; private set; }

  public HeartsSystem heartsSystem;
  [SerializeField] int maxlevelIndex = 24;

  [Header("Events")]
  public Action<GameState> onGameStateChanged;
  public Action<int> onLevelChanged;
  public Action onCoinChanged;
  public Action onBooster1Change;
  public Action onBooster2Change;
  public Action onBooster3Change;
  public Action onChangeRemoveads;
  public Action onChangeNotiFreeCoin;
  public Action<int> OnChangeCurrentHeart;
  public Action<int> OnChangeCurrentHeartInfinity;


  // [Header("User Settings")]
  GameState _gameState;

  public int maxHeart { get => 5; }

  public int CurrentHeart
  {
    get => PlayerPrefs.GetInt(KeyStr.KEY_CURRENT_HEART, maxHeart);
    set
    {
      if (value > maxHeart) value = maxHeart;
      if (value < 0) value = 0;
      // if (value < maxHeart && IsHeartFull) HeartSystem.Instance.countdownSystem.StartCountdown();
      PlayerPrefs.SetInt(KeyStr.KEY_CURRENT_HEART, value);
      OnChangeCurrentHeart?.Invoke(value);
    }
  }
  public bool IsHeartFull => CurrentHeart == maxHeart;
  public bool IsHeartEmpty => CurrentHeart == 0;

  public int CurrentHeartInfinity
  {
    get => PlayerPrefs.GetInt(KeyStr.KEY_CURRENT_HEART_INFINITY, 0);
    set
    {
      if (value < 0) value = 0;
      // if (!IsHeartInfinity && value > 0) HeartInfinitySystem.Instance.countdownSystem.StartCountdown();
      PlayerPrefs.SetInt(KeyStr.KEY_CURRENT_HEART_INFINITY, value);
      OnChangeCurrentHeartInfinity?.Invoke(value);
    }
  }
  public bool IsHeartInfinity => CurrentHeartInfinity > 0;

  private bool _isRemoveAds;
  public bool IsRemoveAds
  {
    get
    {
      return _isRemoveAds;
    }
    set
    {
      _isRemoveAds = value;
      var _intOn = _isRemoveAds ? 1 : 0;
      PlayerPrefs.SetInt(HoangNam.KeyStr.KEY_IS_REMOVE_ADS, _intOn);
      onChangeRemoveads?.Invoke();
    }
  }

  private bool _isSoundOn;
  public bool IsSoundOn
  {
    get
    {
      return _isSoundOn;
    }
    set
    {
      _isSoundOn = value;
      var _intOn = _isSoundOn ? 1 : 0;
      PlayerPrefs.SetInt(HoangNam.KeyStr.KEY_IS_SOUND_ON, _intOn);
    }
  }

  private bool _isMusicOn;
  public bool IsMusicOn
  {
    get
    {
      return _isMusicOn;
    }
    set
    {
      _isMusicOn = value;
      if (value == false)
      {
        SoundSystem.Instance.StopMainThemeSfx();
      }
      else
      {
        SoundSystem.Instance.PlayMainThemeSfx();
      }

      var _intOn = _isMusicOn ? 1 : 0;
      PlayerPrefs.SetInt(HoangNam.KeyStr.KEY_IS_MUSIC_ON, _intOn);
    }
  }

  private bool _isHapticOn;
  public bool IsHapticOn
  {
    get
    {
      return _isHapticOn;
    }
    set
    {
      _isHapticOn = value;
      var _intOn = _isHapticOn ? 1 : 0;
      PlayerPrefs.SetInt(HoangNam.KeyStr.KEY_IS_HAPTIC_ON, _intOn);
    }
  }

  [Header("User Progress")]

  int _currentCoin;
  public int CurrentCoin
  {
    get
    {
      return _currentCoin;
    }
    set
    {
      _currentCoin = value;
      PlayerPrefs.SetInt(HoangNam.KeyStr.KEY_CURRENT_COIN, _currentCoin);
      onCoinChanged?.Invoke();
    }
  }

  int _currentLevelIndex;
  public int CurrentLevelIndex
  {
    get
    {

      return _currentLevelIndex;
    }
    set
    {
      _currentLevelIndex = value > maxlevelIndex ? maxlevelIndex : value;
      PlayerPrefs.SetInt(KeyStr.KEY_CURRENT_LEVELINDEX, _currentLevelIndex);
      onLevelChanged?.Invoke(_currentLevelIndex);
    }
  }

  int _appOpenadTodayCount;
  public int AppOpenadTodayCount
  {
    get
    {
      return _appOpenadTodayCount;
    }
    set
    {
      _appOpenadTodayCount = value;
      PlayerPrefs.SetInt(KeyStr.KEY_APP_OPENAD_TODAY_COUNT, value);
    }
  }

  int _retryAmount;
  public int RetryAmount
  {
    get
    {
      return _retryAmount;
    }
    set
    {
      _retryAmount = value;
      PlayerPrefs.SetInt(KeyStr.KEY_AMOUNT_RETRY, value);
    }
  }

  private int _amountInterAds = 0;
  private int _firstTimeSignIn;
  public int FirstTimeSignIn
  {
    get
    {
      return _firstTimeSignIn;
    }
    set
    {
      _firstTimeSignIn = value;
      PlayerPrefs.SetInt(HoangNam.KeyStr.KEY_FIRST_DAILY_SIGNIN_DATE, _firstTimeSignIn);
    }
  }

  int _currentBooter1;
  public int CurrentBooster1
  {
    get
    {
      return _currentBooter1;
    }
    set
    {
      _currentBooter1 = value;
      PlayerPrefs.SetInt(HoangNam.KeyStr.KEY_CURRENT_BOOSTER_1, _currentBooter1);
      onBooster1Change?.Invoke();
    }
  }

  int _currentBooter2;
  public int CurrentBooster2
  {
    get
    {
      return _currentBooter2;
    }
    set
    {
      _currentBooter2 = value;
      PlayerPrefs.SetInt(HoangNam.KeyStr.KEY_CURRENT_BOOSTER_2, _currentBooter2);
      onBooster2Change?.Invoke();
    }
  }

  int _currentBooter3;
  public int CurrentBooster3
  {
    get
    {
      return _currentBooter3;
    }
    set
    {
      _currentBooter3 = value;
      PlayerPrefs.SetInt(HoangNam.KeyStr.KEY_CURRENT_BOOSTER_3, _currentBooter3);
      onBooster3Change?.Invoke();
    }
  }

  /// <summary>
  /// Event section
  /// </summary>
  private void Awake()
  {
    heartsSystem = new HeartsSystem();

    if (Instance == null)
    {
      Instance = this;

      SceneManager.activeSceneChanged += ChangedActiveScene;
      SceneManager.sceneLoaded += OnSceneLoaded;

      InvokeOnce();
    }
    else
    {
      Destroy(gameObject);
    }
    DontDestroyOnLoad(gameObject);
  }

  private void InvokeOnce()
  {
    InitDailyUserProgressData();
  }

  private void OnDestroy()
  {
    SceneManager.activeSceneChanged -= ChangedActiveScene;
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  private void ChangedActiveScene(Scene current, Scene next)
  {

  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    if (scene.name == "Lobby")
    {
      IdFireModes = 0;
    }
  }

  /// <summary>
  /// Game State Section
  /// </summary>
  /// <returns></returns>
  public GameState GetGameState()
  {
    return _gameState;
  }

  public void SetGameState(GameState state)
  {
    _gameState = state;
    onGameStateChanged?.Invoke(state);
  }

  public void LoadSceneFrom(int buildIndex)
  {
    SceneManager.LoadScene(buildIndex);
  }

  public void LoadSceneFrom(string sceneName)
  {
    SceneManager.LoadScene(sceneName);
  }

  ///
  //
  //
  private void InitDailyUserProgressData()
  {
    _appOpenadTodayCount = PlayerPrefs.GetInt(HoangNam.KeyStr.KEY_APP_OPENAD_TODAY_COUNT, 0);
    var latestOpenedDate = PlayerPrefs.GetString(HoangNam.KeyStr.KEY_LATEST_OPENED_DATE, "");
    var currentDate = HoangNam.Utility.GetCurrentDate();
    if (currentDate != latestOpenedDate)
    {
      AppOpenadTodayCount = 0;
      PlayerPrefs.SetInt("NextDay", 0);
    }

    PlayerPrefs.SetString(KeyStr.KEY_LATEST_OPENED_DATE, currentDate);

    _currentLevelIndex = PlayerPrefs.GetInt(HoangNam.KeyStr.KEY_CURRENT_LEVELINDEX, 0);
    _retryAmount = PlayerPrefs.GetInt(KeyStr.KEY_AMOUNT_RETRY, 0);
    _currentBooter1 = PlayerPrefs.GetInt(KeyStr.KEY_CURRENT_BOOSTER_1, 1);
    _currentBooter2 = PlayerPrefs.GetInt(KeyStr.KEY_CURRENT_BOOSTER_2, 1);
    _currentBooter3 = PlayerPrefs.GetInt(KeyStr.KEY_CURRENT_BOOSTER_3, 1);

    CurrentCoin = PlayerPrefs.GetInt(HoangNam.KeyStr.KEY_CURRENT_COIN, 200);

    var _intSound = PlayerPrefs.GetInt(HoangNam.KeyStr.KEY_IS_SOUND_ON, 1);
    var _intMusic = PlayerPrefs.GetInt(HoangNam.KeyStr.KEY_IS_MUSIC_ON, 1);
    var _intHaptic = PlayerPrefs.GetInt(HoangNam.KeyStr.KEY_IS_HAPTIC_ON, 1);
    var _intRemoveAds = PlayerPrefs.GetInt(HoangNam.KeyStr.KEY_IS_REMOVE_ADS, 0);

    _isSoundOn = _intSound == 1;
    _isMusicOn = _intMusic == 1;
    _isHapticOn = _intHaptic == 1;
    _isRemoveAds = _intRemoveAds == 1;

    _firstTimeSignIn = PlayerPrefs.GetInt(HoangNam.KeyStr.KEY_FIRST_DAILY_SIGNIN_DATE, 0);
  }

  private void OnApplicationQuit()
  {
    ResetAmount();
    if (SceneManager.GetActiveScene().name.Equals(KeyStr.NAME_SCENE_GAMEPLAY))
    {
      heartsSystem.LostHearts(1);
    }
  }

  private void ResetAmount()
  {
    PlayerPrefs.SetInt(KeyStr.KEY_AMOUNT_RETRY, 0);
    PlayerPrefs.SetInt(KeyStr.KEY_AMOUNT_NEXTLEVEL, 0);
    PlayerPrefs.SetInt(KeyStr.KEY_APP_OPENAD_TODAY_COUNT, 0);
  }

  public void LoadSceneByName(string sceneName)
  {
    LoadingBar.sceneName = sceneName;
    SceneManager.LoadScene(KeyStr.NAME_SCENE_LOADING);
  }

  public void PlayGame()
  {
    LoadSceneByName(KeyStr.NAME_SCENE_GAMEPLAY);
  }
}
