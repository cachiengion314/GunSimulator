using UnityEngine;
using System.IO;
using UnityEditor; // Cần thêm để dùng AssetDatabase
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using NaughtyAttributes;
using HoangNam;

public class UIDailySignInModal : BaseUIRoot
{
  [SerializeField] private SignInData _signInData;
  public SignDataJsonBase _signInDataJsonBase;
  [Header("----ButtonDay----")]
  [SerializeField] Button[] _buttonDays;
  [Header("----Success----")]
  [SerializeField] GameObject _parentSuccessDay;
  [SerializeField] GameObject _parentSuccessDayReward;
  [Header("ButtonClaim")]
  [SerializeField] Button _claimx2;
  [SerializeField] Button _claim;
  private int _currentDay = 0;

  void Awake()
  {
    SetupDataSignIn();
    CheckDailySignInStatus();
    SetUpAnimSignIn();

  }

  void SetUpAnimSignIn()
  {
    foreach (Transform child in _parentSuccessDay.transform) // tắt tất cả phần thưởng
    {
      if (child.CompareTag(tag))
      {
        child.gameObject.transform.localScale = Vector3.zero; // thu nhỏ lại
        child.gameObject.SetActive(false);
      }
    }
    _parentSuccessDay.gameObject.SetActive(false);

    foreach (Transform child in _parentSuccessDayReward.transform) // tắt tất cả phần thưởng
    {
      if (child.CompareTag(tag))
      {
        child.gameObject.transform.localScale = Vector3.zero; // thu nhỏ lại
        child.gameObject.SetActive(false);
      }
    }
    _parentSuccessDayReward.gameObject.SetActive(false);
  }
  void FirstTimeSignIn()
  {
    string today = DateTime.Now.ToString("yyyyMMdd");

    PlayerPrefs.SetString("todaySignIn", today);
    PlayerPrefs.SetInt("IntToDaySignIn", 0);
  }
  void CheckDailySignInStatus()
  {
    string STRToday = DateTime.Now.ToString("yyyyMMdd");
    if (PlayerPrefs.GetString("todaySignIn") == "")
    {
      PlayerPrefs.SetString("todaySignIn", STRToday);
    }
    string STRtodayData = PlayerPrefs.GetString("todaySignIn");
    int _intToday = PlayerPrefs.GetInt("IntToDaySignIn");
    if (STRtodayData != STRToday)
    {
      _intToday++;
      PlayerPrefs.SetInt("IntToDaySignIn", _intToday);
      PlayerPrefs.SetString("todaySignIn", STRToday);
    }
    if (_intToday > 6)
    {
      ChangeScriptableObjectToJson();
      _intToday = 0;
      PlayerPrefs.SetInt("IntToDaySignIn", _intToday);
    }
    foreach (Button _button in _buttonDays)
    {
      if (_button != null) // Kiểm tra button không null để tránh lỗi
      {
        // _button.interactable = false; // Tắt button
        _claimx2.gameObject.SetActive(false);
        _claim.gameObject.SetActive(false);
        GameObject _objblinh = _button.transform.GetChild(_button.transform.childCount - 2).gameObject; // Tắt hiệu ứng phát sáng
        _objblinh.SetActive(false);
      }

    }
    _currentDay = _intToday;

    SetButtonDaiLySignIn(_intToday);
  }
  void SetButtonDaiLySignIn(int _idDay) // dữ liệu ngày hôm đó (Id Day , isSuccess)
  {

    Button TodayButton = _buttonDays[_idDay];

    for (int i = 0; i <= _idDay; i++) // Duyệt qua các ngày từ 0 đến _idDay duyệt qua các ngày để check trạng thái nhận thưởng
    {
      Button currentButton = _buttonDays[i];
      if (currentButton == null)
      {

        continue;
      }

      // Nếu ngày đã nhận phần thưởng
      if (_signInDataJsonBase.days[i].isSuccess == true) // các ngày trước đó nếu nhận sẽ hiện tick
      {
        if (i == _idDay)
        {
          if (DailyNoticeManager.Instance != null)
          {
            DailyNoticeManager.Instance._isDayLySginIn = false;
          }
        }

        // Bật tick (người con cuối cùng)
        Transform lastChild = currentButton.transform.GetChild(currentButton.transform.childCount - 1);
        if (lastChild != null)
        {
          lastChild.transform.GetChild(0).gameObject.SetActive(true); // hiện tick
          lastChild.gameObject.SetActive(true);
        }
        else
        {

        }
      }
      else
      {
        if (i != _idDay) // các ngày trước đó nếu k nhận sẽ tắt tick
        {
          Transform lastChild = currentButton.transform.GetChild(currentButton.transform.childCount - 1);
          if (lastChild != null)
          {
            lastChild.transform.GetChild(0).gameObject.SetActive(false);
            lastChild.gameObject.SetActive(true); // tắt tick
          }
        }
      }
    }

    if (TodayButton != null) // bắt đầu xét đến ngày hiện tại
    {
      // Cập nhật giao diện nút của ngày hôm nay
      if (_idDay < 6)
      {

      }
      if (_idDay == 6)
      {

      }
      // TodayButton.transform.GetChild(TodayButton.transform.childCount - 2).gameObject.SetActive(true);
      TodayButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "TODAY"; // thay text tile button

      // Nếu chưa nhận phần thưởng, bật tương tác
      if (_signInDataJsonBase.days[_idDay].isSuccess == false)
      {
        // TodayButton.interactable = true;
        _claim.gameObject.SetActive(true);
        _claimx2.gameObject.SetActive(true);
        TodayButton.transform.GetChild(TodayButton.transform.childCount - 2).gameObject.SetActive(true); // Tắt hiệu ứng phát sáng
      }

    }
    else
    {

    }
  }
  void SetupDataSignIn()
  {
    if (!File.Exists(SaveSystem.GetSaveDataPathFrom("SignInData")))
    {

      ChangeScriptableObjectToJson();
    }
    else
    {

      LoadDataJsonSignIn();
    }
  }
  void ChangeScriptableObjectToJson() // chuyển đổi dữ liệu từ ScriptableObject sang class SignDataBase rồi chuyển sang file Json
  {

    FirstTimeSignIn();

    // Tạo mới _signInDataJsonBase
    _signInDataJsonBase = new SignDataJsonBase();

    // Chuyển dữ liệu từ _signInData sang _signInDataJsonBase
    foreach (var day in _signInData.days)
    {
      _signInDataJsonBase.days.Add(new SignDataJsonBase.SignInDay
      {
        IdDayName = day.IdDayName,

        isSuccess = day.isSuccess
      });
    }

    // Lưu dữ liệu bằng SaveSystem
    SaveDataJsonSignIn();
  }
  void SaveDataJsonSignIn()
  {
    SaveSystem.SaveWith(_signInDataJsonBase, "SignInData");

  }

  void LoadDataJsonSignIn()
  {
    _signInDataJsonBase = SaveSystem.LoadWith<SignDataJsonBase>("SignInData");
  }

  public void Btnday1()
  {
    // _buttonDays[0].interactable = false;
    // _signInDataJsonBase.days[0].isSuccess = true;
    // SaveDataJsonSignIn();
    // // ShowSuccessDay(0);
    // GameSystem.Instance.CurrentCoin += 50; // đoạn nhận quà
  }
  public void Btnday2()
  {
    // _buttonDays[1].interactable = false;
    // _signInDataJsonBase.days[1].isSuccess = true;
    // SaveDataJsonSignIn();
    // // ShowSuccessDay(1);
    // GameSystem.Instance.CurrentBooster1++;
  }
  public void Btnday3()
  {
    // _buttonDays[2].interactable = false;
    // _signInDataJsonBase.days[2].isSuccess = true;
    // SaveDataJsonSignIn();
    // // ShowSuccessDay(2);
    // GameSystem.Instance.CurrentCoin += 100; // đoạn nhận quà
  }
  public void Btnday4()
  {
    // _buttonDays[3].interactable = false;
    // _signInDataJsonBase.days[3].isSuccess = true;
    // SaveDataJsonSignIn();
    // // ShowSuccessDay(3);
    // GameSystem.Instance.CurrentBooster2++;
  }
  public void Btnday5()
  {
    // _buttonDays[4].interactable = false;
    // _signInDataJsonBase.days[4].isSuccess = true;
    // SaveDataJsonSignIn();
    // // ShowSuccessDay(4);
    // GameSystem.Instance.CurrentCoin += 150; // đoạn nhận quà
  }
  public void Btnday6()
  {
    // _buttonDays[5].interactable = false;
    // _signInDataJsonBase.days[5].isSuccess = true;
    // SaveDataJsonSignIn();
    // // ShowSuccessDay(5);
    // GameSystem.Instance.CurrentBooster2++;
  }
  public void Btnday7()
  {
    // _buttonDays[6].interactable = false;
    // _signInDataJsonBase.days[6].isSuccess = true;
    // SaveDataJsonSignIn();
    // // ShowSuccessDay(6);
    // GameSystem.Instance.CurrentBooster1 += 3;
    // GameSystem.Instance.CurrentBooster2 += 3;
    // GameSystem.Instance.CurrentBooster3 += 3;
  }
  public void ShowSuccessDay(int _intToday, bool _isReward)
  {
    _claimx2.gameObject.SetActive(false);
    _claim.gameObject.SetActive(false);
    // SoundSystem.Instance.PlayCollectRewardSfx();
    GameObject ButtonToday = _buttonDays[_intToday].gameObject;
    ButtonToday.transform.GetChild(ButtonToday.transform.childCount - 2).gameObject.SetActive(false);// tắt hiệu ứng phát sáng

    GameObject ObjParentSuccess = GetGameobj_sugcees(_isReward);
    ObjParentSuccess.gameObject.SetActive(true);

    GameObject _SuccessToday = GetGameobj_sugcees(_isReward).transform.GetChild(_intToday).gameObject; // lựa chọn dạng reward để show vật phẩm
                                                                                                       // _SuccessToday.transform.GetChild(0).gameObject.SetActive(false);
    _SuccessToday.SetActive(true);


    // update UI Value Item
    // _SuccessToday.get


    Sequence sequence = DOTween.Sequence();
    sequence.Join(_SuccessToday.transform.DOScale(Vector3.one, 0.3f)
    .SetEase(Ease.Linear) // phóng to vật phẩm
    .OnComplete(() =>
    {
      GameObject ObjTickButtonToday = _buttonDays[_intToday].transform.GetChild(_buttonDays[_intToday].transform.childCount - 1).gameObject;

      // StartCoroutine(HoldSuccessDay());
      ObjTickButtonToday.gameObject.SetActive(true);
      ObjTickButtonToday.transform.GetChild(0).gameObject.SetActive(true); // bật tick
      DailyNoticeManager.Instance._isDayLySginIn = false;
    }));
  }
  GameObject GetGameobj_sugcees(bool _isReward)
  {
    if (_isReward == true)
    {
      return _parentSuccessDayReward;
    }
    else
    {
      return _parentSuccessDay;
    }
  }

  public void BTNExitSuccessDay()// Hide ở sugge bình thường
  {
    // Tìm người con đang active
    Transform activeChild = null;

    foreach (Transform child in _parentSuccessDay.transform)
    {
      if (child.gameObject.activeSelf)
      {
        activeChild = child;
        break;
      }
    }
    if (activeChild != null)
    {
      activeChild.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.Linear) // anim thu nhỏ vật
      .OnComplete(() =>
      {
        _parentSuccessDay.gameObject.SetActive(false);
      });
    }
  }
  public void BTNExitSuccessDayReward()// Hide ở susscess reward
  {
    // Tìm người con đang active
    Transform activeChild = null;

    foreach (Transform child in _parentSuccessDayReward.transform)
    {
      if (child.gameObject.activeSelf)
      {
        activeChild = child;
        break;
      }
    }
    if (activeChild != null)
    {
      activeChild.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.Linear) // anim thu nhỏ vật
      .OnComplete(() =>
      {
        _parentSuccessDayReward.gameObject.SetActive(false);
      });
    }
  }
  public void ClaimReward(bool isDoubleReward) // true là nhận quà x2 fasle là nhận quà bình thường (quà ở phần switch)
  {
    if (_currentDay < 0 || _currentDay >= _signInDataJsonBase.days.Count)
      return;

    if (_signInDataJsonBase.days[_currentDay].isSuccess)
      return; // Nếu đã nhận quà, không làm gì cả

    // Đánh dấu ngày hiện tại đã nhận
    _signInDataJsonBase.days[_currentDay].isSuccess = true;

    // Xử lý nhận quà
    switch (_currentDay)
    {
      case 0:
        GameSystem.Instance.CurrentCoin += isDoubleReward ? 100 : 50;
        break;
      case 1:
        GameSystem.Instance.CurrentBooster1 += isDoubleReward ? 2 : 1;
        break;
      case 2:
        GameSystem.Instance.CurrentCoin += isDoubleReward ? 200 : 100;
        break;
      case 3:
        GameSystem.Instance.CurrentBooster2 += isDoubleReward ? 2 : 1;
        break;
      case 4:
        GameSystem.Instance.CurrentCoin += isDoubleReward ? 500 : 250;
        break;
      case 5:
        GameSystem.Instance.CurrentBooster2 += isDoubleReward ? 2 : 1;
        break;
      case 6:
        GameSystem.Instance.CurrentBooster1 += isDoubleReward ? 6 : 3;
        GameSystem.Instance.CurrentBooster2 += isDoubleReward ? 6 : 3;
        GameSystem.Instance.CurrentBooster3 += isDoubleReward ? 6 : 3;
        break;
    }

    // Lưu trạng thái
    SaveDataJsonSignIn();

    // Hiển thị giao diện thành công
    ShowSuccessDay(_currentDay, isDoubleReward);
  }
  public void BTNClaim()
  {
    ClaimReward(false); // Nhận quà bình thường
  }
  public void BTNClaimX2()
  {
    LevelPlayAds.Instance.ShowRewardedAd(() =>
    {
      ClaimReward(true); // Nhận quà x2
    }, "GetX2RewardTask", () =>
    {
      UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("no ads available");
    });

  }

}


[System.Serializable]
public class SignDataJsonBase
{
  public List<SignInDay> days = new List<SignInDay>();

  [System.Serializable]
  public class SignInDay
  {
    public int IdDayName; // Tên ngày (e.g., "Day 1", "Day 2")

    public bool isSuccess; // Trạng thái đã nhận

  }
}
