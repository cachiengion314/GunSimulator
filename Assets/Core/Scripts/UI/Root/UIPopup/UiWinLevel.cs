using DG.Tweening;
using HoangNam;
using UnityEngine;
using Spine.Unity;

public class UiWinLevel : BaseUIRoot
{
  [SerializeField] SkeletonGraphic _skeletonAnimation;

  void Awake()
  {
    _skeletonAnimation.gameObject.SetActive(false);
  }
  protected override void OnShowCompleted()
  {
    base.OnShowCompleted(); // Gọi base để đảm bảo logic cơ bản vẫn chạy
    // SoundSystem.Instance.PlayConfettiExplosionSfx();
    ShowAnim();
  }
  void ShowAnim()
  {
    _skeletonAnimation.gameObject.SetActive(true);
    _skeletonAnimation.AnimationState.SetAnimation(0, "Ui_win2", false);
    _skeletonAnimation.AnimationState.AddAnimation(0, "Ui_win4", true, 0);

  }

  public void Continue()
  {
    DOTween.KillAll();
    SoundSystem.Instance.PlayButtonSfx();

#if !UNITY_EDITOR
    LevelPlayAds.Instance.ShowInterstitialAd((info) =>
    {
      GameSystem.Instance.CurrentLevelIndex++;
      GameSystem.Instance.CurrentCoin += 25;
      GameSystem.Instance.PlayGame();
        TaskComplete();
    }, "Continue", null,
    () =>
    {
      GameSystem.Instance.CurrentLevelIndex++;
      GameSystem.Instance.CurrentCoin += 25;
      GameSystem.Instance.PlayGame();
       TaskComplete();
    });
#else
    GameSystem.Instance.CurrentLevelIndex++;
    GameSystem.Instance.CurrentCoin += 25;
    GameSystem.Instance.PlayGame();
    TaskComplete();
#endif
  }

  public void GetX2()
  {
    DOTween.KillAll();
    // chay ads
    SoundSystem.Instance.PlayButtonSfx();
    LevelPlayAds.Instance.ShowRewardedAd(() =>
        {
          GameSystem.Instance.CurrentLevelIndex++;
          GameSystem.Instance.CurrentCoin += 50;
          // GameSystem.Instance.LoadSceneByName(KeyStr.NAME_SCENE_LOADING);
          GameSystem.Instance.PlayGame();
          TaskComplete();
        }, "GetX2", () =>
      {
        UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("no ads available");
      });
  }

  public void BtnExit()
  {
    DOTween.KillAll();
    SoundSystem.Instance.PlayButtonSfx();
#if !UNITY_EDITOR
    LevelPlayAds.Instance.ShowInterstitialAd((info) =>
    {
      GameSystem.Instance.CurrentLevelIndex++;
      GameSystem.Instance.CurrentCoin += 25;
      GameSystem.Instance.LoadSceneByName(KeyStr.NAME_SCENE_LOBBY);
       TaskComplete();
    }, "Continue", null,
    () =>
    {
      GameSystem.Instance.CurrentLevelIndex++;
      GameSystem.Instance.CurrentCoin += 25;
      GameSystem.Instance.LoadSceneByName(KeyStr.NAME_SCENE_LOBBY);
       TaskComplete();
    });
#else
    GameSystem.Instance.CurrentLevelIndex++;
    GameSystem.Instance.CurrentCoin += 25;
    GameSystem.Instance.LoadSceneByName(KeyStr.NAME_SCENE_LOBBY);
    TaskComplete();
#endif
  }
  public void TaskComplete()
  {
    DailyTaskManager.Instance.UpdateTaskProgress(4, 1);
    DailyTaskManager.Instance.UpdateTaskProgress(7, 25);
  }
}
