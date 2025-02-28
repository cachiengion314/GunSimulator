using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using HoangNam;

public class UILose2LevelRoot : BaseUIRoot
{

  [SerializeField] Button _buttonRePlay;
  [SerializeField] AnimButtonLoop _animReplayLoop;
  int _Score; // chuyền vào Score
  float _HighScore;// chuyền vào 2Score
  [SerializeField] TMP_Text _textScore;
  [SerializeField] TMP_Text _textHighScore;
  void Awake()
  {
    // SetupStartAnimLose();

  }
  void OnEnable()
  {
    // ShowAnimLose(); // dùng để test
  }
  void SetupStartAnimLose()
  {
    _animReplayLoop.enabled = false;
    _buttonRePlay.interactable = false;
    _buttonRePlay.gameObject.SetActive(false);
    _textScore.text = "0";
    // _textHighScore.text = GameSystem.Instance.CurrentHighScore.ToString();
    // _HighScore = GameSystem.Instance.CurrentHighScore;
  }

  void ShowAnimLose()//con dấu nhỏ đến đúng vị trí r nổ pháo + show 2 button
  {
    // SetupStartAnimLose();
    int currentScore = 0; // Thời gian bắt đầu từ 0

    Sequence sequence = DOTween.Sequence();
    sequence.Join(DOTween.To(() => currentScore, x => currentScore = x, _Score, 0.5f) // 0.5 giây để chạy từ 0 -> targetTime //textTime
        .SetEase(Ease.Linear)
        .OnUpdate(() =>
        {
          // Chuyển đổi thời gian thành định dạng "hh:mm:ss"
          _textScore.text = currentScore.ToString();
        }));
    sequence.OnComplete(() =>
        {
          if (_Score > _HighScore)
          {
            // GameSystem.Instance.CurrentHighScore = _Score;
            // _textHighScore.text = GameSystem.Instance.CurrentHighScore.ToString();

          }
          ShowButonReplay();
        });


  }

  void ShowButonReplay()
  {
    _buttonRePlay.transform.localScale = Vector3.zero;
    _buttonRePlay.gameObject.SetActive(true);
    _buttonRePlay.transform.DOScale(Vector3.one, 0.5f) // Chuyển từ zero -> one trong 0.5 giây
        .SetEase(Ease.OutBack) // Tạo hiệu ứng bật nảy nhẹ khi chuyển đổi
        .OnComplete(() =>
        {
          _animReplayLoop.enabled = true;
          _buttonRePlay.interactable = true;
        });
  }

  protected override void OnShowCompleted()
  {
    base.OnShowCompleted(); // Gọi base để đảm bảo logic cơ bản vẫn chạy
                            // ShowAnimLose(); // Thực hiện anim win sau khi OnShow hoàn tất
  }
  public void BtnExit()
  {
    SoundSystem.Instance.PlayHidePopupSfx();
    DOTween.KillAll();
#if !UNITY_EDITOR
    LevelPlayAds.Instance.ShowInterstitialAd((levelPlayAdInfo) =>
    {
        GameSystem.Instance.LoadSceneByName(KeyStr.NAME_SCENE_LOBBY);
    }, "Continue", null,
    () =>
    {
        GameSystem.Instance.LoadSceneByName(KeyStr.NAME_SCENE_LOBBY);
    });
#else
    GameSystem.Instance.LoadSceneByName(KeyStr.NAME_SCENE_LOBBY);
#endif
  }

  public void ButtonTryAgin()
  {
    SoundSystem.Instance.PlayButtonSfx();
    GameSystem.Instance.PlayGame();
  }
}
