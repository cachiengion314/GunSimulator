using UnityEngine;
using Firebase.Extensions;
using Firebase.Analytics;
using System.Threading.Tasks;
using Firebase.Crashlytics;
using System;
using Firebase.RemoteConfig;
using HoangNam;

public class FirebaseSetup : MonoBehaviour
{
  public static Action onNeedUpdate;

  public static FirebaseSetup Instance { get; private set; }
  public bool IsFirebaseReady { get; private set; }
  private Firebase.FirebaseApp app;

  [HideInInspector] public bool IsCompletedUpdate;

  [Header("External Dependences")]
  [SerializeField] RectTransform updateModal;

  [Header("Internal Dependences")]
  [SerializeField] FirebaseRemoteData _firebaseRemoteData;
  // [SerializeField] UpdateConfigData updateConfigData;
  public FirebaseRemoteData FirebaseRemoteData
  {
    get { return _firebaseRemoteData; }
  }


  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;

#if !UNITY_EDITOR
      if (!Debug.isDebugBuild)
      {
        Init();
      } 
      else 
      {
        IsCompletedUpdate = true;
      }
#endif
    }
    else Destroy(gameObject);
    DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
    IsCompletedUpdate = true;
#endif
  }

  void Init()
  {
    Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
    {
      var dependencyStatus = task.Result;
      if (dependencyStatus == Firebase.DependencyStatus.Available)
      {
        // Create and hold a reference to your FirebaseApp,
        // where app is a Firebase.FirebaseApp property of your application class.
        app = Firebase.FirebaseApp.DefaultInstance;

        // When this property is set to true, Crashlytics will report all
        // uncaught exceptions as fatal events. This is the recommended behavior.
        if (Debug.isDebugBuild)
        {
          Crashlytics.ReportUncaughtExceptionsAsFatal = false;
        }
        else
        {
          Crashlytics.ReportUncaughtExceptionsAsFatal = true;
        }

        IsFirebaseReady = true;

        // Set a flag here to indicate whether Firebase is ready to use by your app.
        InitializeFirebaseMessage();

        int firstOpen = PlayerPrefs.GetInt(KeyStr.FIREBASE_FIRST_OPEN, 0);
        if (firstOpen == 0)
        {
          // first time open app
          PlayerPrefs.SetInt(KeyStr.FIREBASE_FIRST_OPEN, 1);
          if (FirebaseSetup.Instance.IsFirebaseReady)
          {
            FirebaseAnalytics.LogEvent(KeyStr.FIREBASE_FIRST_OPEN);
          }
        }
        // 
        FetchDataAsync();
      }
      else
      {
        IsCompletedUpdate = true;
        Debug.LogError(System.String.Format(
              "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        // Firebase Unity SDK is not safe to use here.
      }
    });
  }

  // Setup message event handlers.
  private string topic = "TestTopic";
  void InitializeFirebaseMessage()
  {
    Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
    Firebase.Messaging.FirebaseMessaging.SubscribeAsync(topic).ContinueWithOnMainThread(task =>
    {
      LogTaskCompletion(task, "SubscribeAsync");
    });
    Debug.Log("Firebase Messaging Initialized");

    // On iOS, this will display the prompt to request permission to receive
    // notifications if the prompt has not already been displayed before. (If
    // the user already responded to the prompt, thier decision is cached by
    // the OS and can be changed in the OS settings).
    // On Android, this will return successfully immediately, as there is no
    // equivalent system logic to run.
    Firebase.Messaging.FirebaseMessaging.RequestPermissionAsync().ContinueWithOnMainThread(
      task =>
      {
        LogTaskCompletion(task, "RequestPermissionAsync");
      }
    );
  }

  public virtual void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
  {
    Debug.Log("Received a new message");
    var notification = e.Message.Notification;
    if (notification != null)
    {
      Debug.Log("title: " + notification.Title);
      Debug.Log("body: " + notification.Body);
      var android = notification.Android;
      if (android != null)
      {
        Debug.Log("android channel_id: " + android.ChannelId);
      }
    }
    if (e.Message.From.Length > 0)
      Debug.Log("from: " + e.Message.From);
    if (e.Message.Link != null)
    {
      Debug.Log("link: " + e.Message.Link.ToString());
    }
    if (e.Message.Data.Count > 0)
    {
      Debug.Log("data:");
      foreach (System.Collections.Generic.KeyValuePair<string, string> iter in
               e.Message.Data)
      {
        Debug.Log("  " + iter.Key + ": " + iter.Value);
      }
    }
  }

  public virtual void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
  {
    Debug.Log("Received Registration Token: " + token.Token + ";token end here;");
  }

  // [START fetch_async]
  // Start a fetch request.
  // FetchAsync only fetches new data if the current data is older than the provided
  // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
  // By default the timespan is 12 hours, and for production apps, this is a good
  // number. For this example though, it's set to a timespan of zero, so that
  // changes in the console will always show up immediately.
  public Task FetchDataAsync()
  {
    Debug.Log("Fetching data...");
    System.Threading.Tasks.Task fetchTask =
    Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
        System.TimeSpan.Zero);
    return fetchTask.ContinueWithOnMainThread(FetchComplete);
  }
  //[END fetch_async]

  void FetchComplete(Task fetchTask)
  {
    if (fetchTask.IsCanceled)
    {
      Debug.Log("Fetch canceled.");
    }
    else if (fetchTask.IsFaulted)
    {
      Debug.Log("Fetch encountered an error.");
    }
    else if (fetchTask.IsCompleted)
    {
      Debug.Log("Fetch completed successfully!");
    }

    var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
    switch (info.LastFetchStatus)
    {
      case Firebase.RemoteConfig.LastFetchStatus.Success:
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
        .ContinueWithOnMainThread(task =>
        {
          Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                               info.FetchTime));
          ConvertDataFromRemote();
        });

        break;
      case Firebase.RemoteConfig.LastFetchStatus.Failure:
        switch (info.LastFetchFailureReason)
        {
          case Firebase.RemoteConfig.FetchFailureReason.Error:
            Debug.Log("Fetch failed for unknown reason");
            IsCompletedUpdate = true;
            break;
          case Firebase.RemoteConfig.FetchFailureReason.Throttled:
            Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
            IsCompletedUpdate = true;
            break;
          default:
            IsCompletedUpdate = true;
            break;
        }
        break;
      case Firebase.RemoteConfig.LastFetchStatus.Pending:
        Debug.Log("Latest Fetch call still pending.");
        IsCompletedUpdate = true;
        break;
    }
  }


  public void LogMeButton()
  {
    FirebaseAnalytics.LogEvent("LogMe_button_pressed");
  }

  public void PressNumberButton(int number)
  {
    FirebaseAnalytics.LogEvent("Press_Number_button_pressed", new Parameter[] {
            new("ButtonNumber", number),
            new("ButtonNumber", number),
        });
  }

  #region Handle Function
  private void ConvertDataFromRemote()
  {
    var firebaseRemoteData = _firebaseRemoteData;
    // var parameterGroupsJson = FirebaseRemoteConfig.DefaultInstance.GetValue(KeyString.FIREBASE_REMOTECONFIG_PARAMETERGROUPS).StringValue;
    var updateconfigJson = FirebaseRemoteConfig.DefaultInstance.GetValue(KeyStr.FIREBASE_REMOTECONFIG_UPDATECONFIG).StringValue;
    var configGameJson = FirebaseRemoteConfig.DefaultInstance.GetValue(KeyStr.FIREBASE_REMOTECONFIG_CONFIGGAME).StringValue;
    var admobJson = FirebaseRemoteConfig.DefaultInstance.GetValue(KeyStr.FIREBASE_REMOTECONFIG_ADMOB).StringValue;

    if (!updateconfigJson.Equals(""))
    {
      var updateConfig = JsonUtility.FromJson<UpdateConfigData>(updateconfigJson);

      firebaseRemoteData.parameterGroups.Lobby.parameters.update_config = updateConfig;
    }

    if (!configGameJson.Equals(""))
    {
      var configGame = JsonUtility.FromJson<ConfigGameData>(configGameJson);

      firebaseRemoteData.parameterGroups.Lobby.parameters.config_game = configGame;
    }

    if (!admobJson.Equals(""))
    {
      var admob = JsonUtility.FromJson<AdmobData>(admobJson);

      firebaseRemoteData.parameterGroups.Lobby.parameters.Admob = admob;
    }

    _firebaseRemoteData = firebaseRemoteData;
    CheckVersion();
  }

  private void CheckVersion()
  {
    var version = "";
    var reward = 0;

#if UNITY_ANDROID
    version = _firebaseRemoteData.parameterGroups.Lobby.parameters.update_config.android.version;
    reward = _firebaseRemoteData.parameterGroups.Lobby.parameters.update_config.android.reward;
#elif UNITY_IOS
    version = _firebaseRemoteData.parameterGroups.Lobby.parameters.update_config.ios.version;
    reward = _firebaseRemoteData.parameterGroups.Lobby.parameters.update_config.ios.reward;
#endif

    var keyVersion = "version" + version;
    if (!IsNeedUpdateWith(version))
    {
      Debug.Log("Play");
      if (PlayerPrefs.GetInt(keyVersion, 0) == 0)
      {
        PlayerPrefs.SetInt(keyVersion, 1);
        ClaimReward(reward);
      }
      updateModal.gameObject.SetActive(false);
      IsCompletedUpdate = true;
    }
    else
    {
      Debug.Log("Update");
      updateModal.gameObject.SetActive(true);
      onNeedUpdate?.Invoke();
    }
  }

  private void ClaimReward(int coinReward)
  {
    var coin = PlayerPrefs.GetInt(KeyStr.KEY_CURRENT_COIN, 100);
    coin += coinReward;

    PlayerPrefs.SetInt(KeyStr.KEY_CURRENT_COIN, coin);
  }

  // Log the result of the specified task, returning true if the task
  // completed successfully, false otherwise.
  protected bool LogTaskCompletion(Task task, string operation)
  {
    bool complete = false;
    if (task.IsCanceled)
    {
      Debug.Log(operation + " canceled.");
    }
    else if (task.IsFaulted)
    {
      Debug.Log(operation + " encounted an error.");
      foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
      {
        string errorCode = "";
        Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
        if (firebaseEx != null)
        {
          errorCode = String.Format("Error.{0}: ",
            ((Firebase.Messaging.Error)firebaseEx.ErrorCode).ToString());
        }
        Debug.Log(errorCode + exception.ToString());
      }
    }
    else if (task.IsCompleted)
    {
      Debug.Log(operation + " completed");
      complete = true;
    }
    return complete;
  }

  #endregion

  #region Check Function
  private bool IsNeedUpdateWith(string remoteVersion)
  {
    var currentVersion = Application.version;

    if (currentVersion.Equals(remoteVersion)) return false;

    var curVers = currentVersion.Split('.');
    var remoteVers = remoteVersion.Split('.');

    if (remoteVers.Length == 0) return false;
    if (curVers.Length <= remoteVers.Length)
    {
      for (int i = 0; i < curVers.Length; i++)
      {
        var curVer = int.Parse(curVers[i]);
        var remoteVer = int.Parse(remoteVers[i]);

        if (curVer < remoteVer) return true;
        if (curVer > remoteVer) return false;

        continue;
      }

      return true;
    }

    if (curVers.Length > remoteVers.Length)
    {
      for (int i = 0; i < remoteVers.Length; i++)
      {
        var curVer = int.Parse(curVers[i]);
        var remoteVer = int.Parse(remoteVers[i]);

        if (curVer < remoteVer) return true;
        if (curVer > remoteVer) return false;

        continue;
      }

      return false;
    }

    return false;
  }

  #endregion
}

[Serializable]
public struct FirebaseRemoteData
{
  public ParameterGroups parameterGroups;
}

[Serializable]
public struct ParameterGroups
{
  public LobbyData Lobby;
}
