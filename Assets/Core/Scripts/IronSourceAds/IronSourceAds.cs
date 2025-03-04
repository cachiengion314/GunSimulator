using System;
using UnityEngine;
using com.unity3d.mediation;
using Firebase.Analytics;
using System.Collections;
using HoangNam;

// Example for IronSource Unity.
public class LevelPlayAds : MonoBehaviour
{
  public static LevelPlayAds Instance { get; private set; }

  Action userRewardEarnedCallback;
  Action onShowRewardFail;
  Action onShowInterstitialFail;
  Action<LevelPlayAdInfo> onWatchedInterstitialClose;
  Action<LevelPlayAdInfo> onWatchedInterstitialSuccess;

  private LevelPlayBannerAd bannerAd;
  private LevelPlayInterstitialAd interstitialAd;

  [SerializeField] string appIOSKey;
  [SerializeField] string bannerAdUnitIOSId;
  [SerializeField] string interstitialAdUnitIOSId;
  [SerializeField] string appAndroidKey;
  [SerializeField] string bannerAdUnitAndroidId;
  [SerializeField] string interstitialAdUnitAndroidId;

  string _appKey;
  string _bannerAdUnitId;
  string _interstitialAdUnitId;


  bool isMusicOn;
  private bool _isSucceed;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;

#if UNITY_ANDROID
      _appKey = appAndroidKey;
      _bannerAdUnitId = bannerAdUnitAndroidId;
      _interstitialAdUnitId = interstitialAdUnitAndroidId;
#elif UNITY_IPHONE
      _appKey = appIOSKey;
      _bannerAdUnitId = bannerAdUnitIOSId;
      _interstitialAdUnitId = interstitialAdUnitIOSId;
#else
      _appKey = "unexpected_platform";
      _bannerAdUnitId = "unexpected_platform";
      _interstitialAdUnitId = "unexpected_platform";
#endif

      InitIronSource();
    }
    else
    {
      Destroy(gameObject);
    }

    DontDestroyOnLoad(gameObject);
  }

  public void InitIronSource()
  {
    EnableAds();

    Debug.Log("unity-script: IronSource.Agent.validateIntegration");
    IronSource.Agent.validateIntegration();

    Debug.Log("unity-script: unity version" + IronSource.unityVersion());

    // SDK init
    Debug.Log("unity-script: LevelPlay SDK initialization");
    LevelPlay.Init(_appKey, adFormats: new[] { LevelPlayAdFormat.REWARDED });

    LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
    LevelPlay.OnInitFailed += SdkInitializationFailedEvent;
  }

  void OnDestroy()
  {
    DisableAds();
  }

  void EnableAds()
  {
    //Add ImpressionSuccess Event
    IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;

    //Add AdInfo Rewarded Video Events
    IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
    IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
    IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
    IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
    IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
    IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
    IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

    bannerAd = new LevelPlayBannerAd(_bannerAdUnitId, LevelPlayAdSize.CreateAdaptiveAdSize(), LevelPlayBannerPosition.BottomCenter);

    // Register to Banner events
    bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
    bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
    bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
    bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
    bannerAd.OnAdClicked += BannerOnAdClickedEvent;
    bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
    bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
    bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;

    // Create Interstitial object
    interstitialAd = new LevelPlayInterstitialAd(_interstitialAdUnitId);

    // Register to Interstitial events
    interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
    interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
    interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
    interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
    interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
    interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
    interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;
  }

  void DisableAds()
  {
    //Add ImpressionSuccess Event 
    IronSourceEvents.onImpressionDataReadyEvent -= ImpressionDataReadyEvent;

    //Add AdInfo Rewarded Video Events 
    IronSourceRewardedVideoEvents.onAdOpenedEvent -= RewardedVideoOnAdOpenedEvent;
    IronSourceRewardedVideoEvents.onAdClosedEvent -= RewardedVideoOnAdClosedEvent;
    IronSourceRewardedVideoEvents.onAdAvailableEvent -= RewardedVideoOnAdAvailable;
    IronSourceRewardedVideoEvents.onAdUnavailableEvent -= RewardedVideoOnAdUnavailable;
    IronSourceRewardedVideoEvents.onAdShowFailedEvent -= RewardedVideoOnAdShowFailedEvent;
    IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
    IronSourceRewardedVideoEvents.onAdClickedEvent -= RewardedVideoOnAdClickedEvent;

    // Register to Banner events 
    if (bannerAd != null)
    {
      bannerAd.OnAdLoaded -= BannerOnAdLoadedEvent;
      bannerAd.OnAdLoadFailed -= BannerOnAdLoadFailedEvent;
      bannerAd.OnAdDisplayed -= BannerOnAdDisplayedEvent;
      bannerAd.OnAdDisplayFailed -= BannerOnAdDisplayFailedEvent;
      bannerAd.OnAdClicked -= BannerOnAdClickedEvent;
      bannerAd.OnAdCollapsed -= BannerOnAdCollapsedEvent;
      bannerAd.OnAdLeftApplication -= BannerOnAdLeftApplicationEvent;
      bannerAd.OnAdExpanded -= BannerOnAdExpandedEvent;
    }

    // Register to Interstitial events 
    if (interstitialAd != null)
    {
      interstitialAd.OnAdLoaded -= InterstitialOnAdLoadedEvent;
      interstitialAd.OnAdLoadFailed -= InterstitialOnAdLoadFailedEvent;
      interstitialAd.OnAdDisplayed -= InterstitialOnAdDisplayedEvent;
      interstitialAd.OnAdDisplayFailed -= InterstitialOnAdDisplayFailedEvent;
      interstitialAd.OnAdClicked -= InterstitialOnAdClickedEvent;
      interstitialAd.OnAdClosed -= InterstitialOnAdClosedEvent;
      interstitialAd.OnAdInfoChanged -= InterstitialOnAdInfoChangedEvent;
    }
  }

  void OnApplicationPause(bool isPaused)
  {
    Debug.Log("unity-script: OnApplicationPause = " + isPaused);
    IronSource.Agent.onApplicationPause(isPaused);
  }

  #region Init callback handlers

  void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
  {
    Debug.Log("unity-script: I got SdkInitializationCompletedEvent with config: " + config);

    interstitialAd.LoadAd();
    bannerAd.LoadAd();
  }

  void SdkInitializationFailedEvent(LevelPlayInitError error)
  {
    Debug.Log("unity-script: I got SdkInitializationFailedEvent with error: " + error);
  }

  #endregion

  #region AdInfo Rewarded Video
  void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
  {
    Debug.Log("unity-script: I got RewardedVideoOnAdOpenedEvent With AdInfo " + adInfo);
  }

  void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
  {
    Debug.Log("unity-script: I got RewardedVideoOnAdClosedEvent With AdInfo " + adInfo);

    BackToNormal();
    HoangNam.Utility.Print("RewardedVideoOnAdClosedEvent.info: " + adInfo);

    if (_isSucceed)
    {
      _isSucceed = false;
      userRewardEarnedCallback?.Invoke();
    }
    else
    {
      onShowRewardFail?.Invoke();
    }
  }

  void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
  {
    Debug.Log("unity-script: I got RewardedVideoOnAdAvailable With AdInfo " + adInfo);
  }

  void RewardedVideoOnAdUnavailable()
  {
    Debug.Log("unity-script: I got RewardedVideoOnAdUnavailable");
  }

  void RewardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
  {
    Debug.Log("unity-script: I got RewardedVideoOnAdShowFailedEvent With Error" + ironSourceError + "And AdInfo " + adInfo);

    if (FirebaseSetup.Instance.IsFirebaseReady)
      FirebaseAnalytics.LogEvent(KeyStr.FIREBASE_ADS_REWARD_FAIL);

    onShowRewardFail?.Invoke();
  }

  void RewardedVideoOnAdRewardedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
  {
    Debug.Log("unity-script: I got RewardedVideoOnAdRewardedEvent With Placement" + ironSourcePlacement + "And AdInfo " + adInfo);

    _isSucceed = true;
  }

  void RewardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
  {
    Debug.Log("unity-script: I got RewardedVideoOnAdClickedEvent With Placement" + ironSourcePlacement + "And AdInfo " + adInfo);

    if (FirebaseSetup.Instance.IsFirebaseReady)
      FirebaseAnalytics.LogEvent(KeyStr.FIREBASE_ADS_REWARD_CLICK);

    _isSucceed = true;
  }

  #endregion
  #region AdInfo Interstitial

  void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo)
  {
    Debug.Log("unity-script: I got InterstitialOnAdLoadedEvent With AdInfo " + adInfo);

    if (FirebaseSetup.Instance.IsFirebaseReady)
      FirebaseAnalytics.LogEvent(KeyStr.FIREBASE_AD_INTER_LOAD);
  }

  void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
  {
    Debug.Log("unity-script: I got InterstitialOnAdLoadFailedEvent With Error " + error);

    if (FirebaseSetup.Instance.IsFirebaseReady)
      FirebaseAnalytics.LogEvent(KeyStr.FIREBASE_AD_INTER_FAIL);
  }

  void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
  {
    Debug.Log("unity-script: I got InterstitialOnAdDisplayedEvent With AdInfo " + adInfo);
  }

  void InterstitialOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError infoError)
  {
    Debug.Log("unity-script: I got InterstitialOnAdDisplayFailedEvent With InfoError " + infoError);

    onShowInterstitialFail?.Invoke();
    StartCoroutine(TryLoadInterstitial());
  }

  void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo)
  {
    Debug.Log("unity-script: I got InterstitialOnAdClickedEvent With AdInfo " + adInfo);

    if (FirebaseSetup.Instance.IsFirebaseReady)
      FirebaseAnalytics.LogEvent(KeyStr.FIREBASE_AD_INTER_CLICK);
  }

  void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
  {
    Debug.Log("unity-script: I got InterstitialOnAdClosedEvent With AdInfo " + adInfo);

    BackToNormal();

    StartCoroutine(TryLoadInterstitial());

    onWatchedInterstitialClose?.Invoke(adInfo);

    if (FirebaseSetup.Instance.IsFirebaseReady)
      FirebaseAnalytics.LogEvent(KeyStr.FIREBASE_AD_INTER_LOAD);
  }

  void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
  {
    Debug.Log("unity-script: I got InterstitialOnAdInfoChangedEvent With AdInfo " + adInfo);
  }

  #endregion

  #region Banner AdInfo

  void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo)
  {
    Debug.Log("unity-script: I got BannerOnAdLoadedEvent With AdInfo " + adInfo);
  }

  void BannerOnAdLoadFailedEvent(LevelPlayAdError ironSourceError)
  {
    Debug.Log("unity-script: I got BannerOnAdLoadFailedEvent With Error " + ironSourceError);

    StartCoroutine(TryLoadBanner());
  }

  void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo)
  {
    Debug.Log("unity-script: I got BannerOnAdClickedEvent With AdInfo " + adInfo);
  }

  void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
  {
    Debug.Log("unity-script: I got BannerOnAdDisplayedEvent With AdInfo " + adInfo);
  }

  void BannerOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError adInfoError)
  {
    Debug.Log("unity-script: I got BannerOnAdDisplayFailedEvent With AdInfoError " + adInfoError);

    StartCoroutine(TryLoadBanner());
  }

  void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo)
  {
    Debug.Log("unity-script: I got BannerOnAdCollapsedEvent With AdInfo " + adInfo);
  }

  void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo)
  {
    Debug.Log("unity-script: I got BannerOnAdLeftApplicationEvent With AdInfo " + adInfo);
  }

  void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo)
  {
    Debug.Log("unity-script: I got BannerOnAdExpandedEvent With AdInfo " + adInfo);
  }

  #endregion

  #region ImpressionSuccess callback handler

  void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
  {
    Debug.Log("unity - script: I got ImpressionDataReadyEvent ToString(): " + impressionData.ToString());
    Debug.Log("unity - script: I got ImpressionDataReadyEvent allData: " + impressionData.allData);

    if (impressionData != null)
    {
      Parameter[] AdParameters = {
        new Parameter("ad_platform", "ironSource"),
        new Parameter("ad_source", impressionData.adNetwork),
        new Parameter("ad_unit_name", impressionData.adFormat),
        new Parameter("ad_format", impressionData.instanceName),
        new Parameter("currency","USD"),
        new Parameter("value", impressionData.revenue.Value)
      };
      FirebaseAnalytics.LogEvent(KeyStr.FIREBASE_AD_IMPRESSION, AdParameters);
      FirebaseAnalytics.LogEvent(KeyStr.FIREBASE_AD_MAX);
    }
  }

  #endregion

  public void ShowRewardedAd(Action userRewardEarned, string placement, Action fallback = null)
  {
    this.userRewardEarnedCallback = userRewardEarned;
    this.onShowRewardFail = fallback;

    PauseGame();

    if (IronSource.Agent.isRewardedVideoAvailable())
    {
      IronSource.Agent.showRewardedVideo();
      if (FirebaseSetup.Instance.IsFirebaseReady)
        FirebaseAnalytics.LogEvent(KeyStr.FIREBASE_ADS_REWARD_SHOW);
    }
    else
    {
      BackToNormal();
      HoangNam.Utility.Print("Ironsource.Rewarded video is not ready");
      fallback?.Invoke();

      // RewardCustomAd.Instance.ShowRewardedAd(userRewardEarned, placement, fallback);
    }

    if (FirebaseSetup.Instance.IsFirebaseReady)
    {
      FirebaseAnalytics.LogEvent(KeyStr.FIREBASE_ADS_REWARD_OFFER);
      FirebaseAnalytics.LogEvent(KeyStr.FIREBASE_REWARD_PLACE_ + placement);
      FirebaseAnalytics.LogEvent(
        KeyStr.FIREBASE_WATCH_VIDEO_GAME,
        new Parameter[] {
            new("actionWatch", "ShowRewardedAd"),
            new("has_ads", IronSource.Agent.isRewardedVideoAvailable() == true ? 1: 0),
            new("has_internet", CheckInternet.HasNetwork() == true ? 1: 0),
            new("placement", placement),
        }
      );
    }
  }

  public void ShowInterstitialAd(Action<LevelPlayAdInfo> onSuccess, string placement, Action<LevelPlayAdInfo> onClose = null, Action fallback = null)
  {
    this.onWatchedInterstitialClose = onClose;
    this.onWatchedInterstitialSuccess = onSuccess;
    this.onShowInterstitialFail = fallback;

    PauseGame();

    if (interstitialAd.IsAdReady())
    {
      interstitialAd.ShowAd();
    }
    else
    {
      BackToNormal();
      HoangNam.Utility.Print("Ironsource.Interstitial video is not ready");
      fallback?.Invoke();

      // InterstitialCustomAd.Instance.ShowInterstitialAd(fallback, placement, fallback);
    }

    if (FirebaseSetup.Instance.IsFirebaseReady)
    {
      FirebaseAnalytics.LogEvent(
        KeyStr.FIREBASE_WATCH_VIDEO_GAME,
        new Parameter[] {
        new("actionWatch", "ShowInterstitialAd"),
        new("has_ads", IronSource.Agent.isRewardedVideoAvailable() == true ? 1: 0),
        new("has_internet", CheckInternet.HasNetwork() == true ? 1: 0),
        new("placement", placement),
        }
      );
    }
  }

  void PauseGame()
  {
    isMusicOn = GameSystem.Instance.IsMusicOn;
    GameSystem.Instance.IsMusicOn = false;
    // Time.timeScale = 1;
  }

  void BackToNormal()
  {
    GameSystem.Instance.IsMusicOn = isMusicOn;
    // Time.timeScale = 1;
  }

  private void OnDisable()
  {
    bannerAd?.DestroyAd();
    interstitialAd?.DestroyAd();
  }

  IEnumerator TryLoadInterstitial()
  {
    yield return new WaitForSeconds(5);

    interstitialAd.LoadAd();
  }

  IEnumerator TryLoadBanner()
  {
    yield return new WaitForSeconds(5);

    bannerAd.LoadAd();
  }
}
