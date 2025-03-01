using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using HoangNam;

public class UIDailyTaskModal : BaseUIRoot
{
  [SerializeField] GameObject _prefabTask; // con 0 là bg , 1 là text tile ; 2 là slider , 3 ;là button
  [SerializeField] GameObject _parentTask;
  [Header("---AlLTask---")]
  [SerializeField] Slider _sliderTaskAll;
  [SerializeField] int _allTaskTarget;
  [SerializeField] int _currentValueTask;
  [SerializeField] Button[] _arrayReward;  // có 3 phần quà . nggười con index 0 = đã nhận , 1 = có thể nhận , 2 default 
  [Header("---ShowReward---")]
  [SerializeField] GameObject _objShowReward;
  [SerializeField] GameObject _objContentParent;


  void Awake()
  {
    //   
    CheckUpdateDataTaskVersion();
    CheckToday();
    // CreatPrefabTask();
    UpdateState_isDayLyTask();
  }
  void OnEnable()
  {
    CreatPrefabTask();
    SetupAnimShowReward();

  }
  void OnDisable()
  {
    UpdateState_isDayLyTask();
  }
  void UpdateState_isDayLyTask()
  {
    UpdateRewardButtonState(); // check button Reward  
    foreach (var task in DailyTaskManager.Instance._taskDataJsonBase.TasksTodayJson) // check button select
    {
      if (task._currentValue >= task._targetValue && task._isSuccess == false && task._isPick == true)
      {
        // Nếu có nhiệm vụ đạt điều kiện, đặt flag thành true
        if (DailyNoticeManager.Instance != null)
        {
          DailyNoticeManager.Instance._isDayLyTask = true;
        }
        break;
      }

    }
  }
  public void CheckUpdateDataTaskVersion() // kiểm tra dữ liệu các nhiệm vụ xem có khớp với dữ lụu trong ScriptableObject
  {
    
    if (DailyTaskManager.Instance._taskDataJsonBase.TasksTodayJson.Count != DailyTaskManager.Instance._taskData.AllTasks.Count)
    {
      // Nếu số lượng khác nhau hoặc file JSON không tồn tại, chạy lại ChangeScriptableObjectToJson
      DailyTaskManager.Instance.ChangeScriptableObjectToJson();
      UpdateDataTaskVersion();
      return;
    }

    // Debug.Log("--CheckUpdateDataTaskVersion2--");
    for (int i = 0; i < DailyTaskManager.Instance._taskData.AllTasks.Count; i++)
    {
      var scriptableTask = DailyTaskManager.Instance._taskData.AllTasks[i];
      var jsonTask = DailyTaskManager.Instance._taskDataJsonBase.TasksTodayJson.Find(t => t._idTask == scriptableTask._idTask);
      // Nếu không tìm thấy nhiệm vụ hoặc tên nhiệm vụ khác, đồng bộ lại dữ liệu
      if (jsonTask._strNameTask != scriptableTask._strNameTask)// nếu sửa đổi tên sẽ update lại
      {
        // Debug.Log("--CheckUpdateDataTaskVersion3--");
        // Debug.Log("--updatedataNameTask--");
        DailyTaskManager.Instance.ChangeScriptableObjectToJson();
        UpdateDataTaskVersion();
        return;
      }
      if (jsonTask._targetValue != scriptableTask._targetValue) //nếu sửa đổi nhiệm vụ sẽ update lại
      {
        // Debug.Log("--CheckUpdateDataTaskVersion4--");
        // Debug.Log("--updatedataValueTask--");
        DailyTaskManager.Instance.ChangeScriptableObjectToJson();
        UpdateDataTaskVersion();
        return;
      }
    }
  }
  public void UpdateDataTaskVersion() // update lại dữ liệu
  {
    ResetNextDay();// reset Dữ Liệu
    SetupSliderTaskAll();
    // Debug.Log("---UpdateData---");
  }
  void CheckToday()
  {
    string STRToday = DateTime.Now.ToString("yyyyMMdd");
    if (PlayerPrefs.GetString("todayTask") == "")
    {
      PlayerPrefs.SetString("todayTask", STRToday); // trường hợp ngày đầu tiên chưa được khởi tạo
      ResetNextDay();
    }
    string STRtodayData = PlayerPrefs.GetString("todayTask");

    if (STRtodayData != STRToday)
    {
      PlayerPrefs.SetString("todayTask", STRToday);
      ResetNextDay();// reset Dữ Liệu
    }
    SetupSliderTaskAll();
  }
  void SetupSliderTaskAll()
  {
    _currentValueTask = PlayerPrefs.GetInt("CurrentValuetodayTask", 0);

    _sliderTaskAll.maxValue = _allTaskTarget;
    _sliderTaskAll.value = _currentValueTask;
    CheckValuesliderAll();
  }
  void CheckValuesliderAll()
  {
    UpdateRewardButtonState(); // Cập nhật trạng thái nút (enable/disable)
    AssignRewardButtonEvents(); // Gán sự kiện cho các nút
  }

  void SetDefaultRewardButton()
  {
    foreach (Button rewardButton in _arrayReward)
    {
      Transform buttonTransform = rewardButton.gameObject.transform;

      // Lặp qua tất cả các `Child` của nút
      for (int i = 0; i < buttonTransform.childCount - 1; i++) // phần tử cuối cùng là text lên vẫn bật
      {
        buttonTransform.GetChild(i).gameObject.SetActive(false); // Tắt tất cả các `Child`
      }
    }
  }

  void UpdateRewardButtonState()
  {
    int rewardStep = _allTaskTarget / _arrayReward.Length;
    SetDefaultRewardButton();
    // Duyệt qua các nút phần quà và cập nhật trạng thái `interactable`
    for (int i = 0; i < _arrayReward.Length; i++)
    {
      bool isClaimed = PlayerPrefs.GetInt($"RewardClaimed_{i}", 0) == 1;
      if (isClaimed)
      {
        // Nếu đã nhận, nút không thể nhấn
        _arrayReward[i].interactable = false;
        _arrayReward[i].transform.GetChild(0).gameObject.SetActive(true); // Tắt tất cả các `Child`
      }
      else if (_currentValueTask >= rewardStep * (i + 1))
      {
        // Nếu chưa nhận và đạt đủ tiến trình, nút có thể nhấn
        _arrayReward[i].interactable = true;
        _arrayReward[i].transform.GetChild(1).gameObject.SetActive(true); // Tắt tất cả các `Child`
        if (DailyNoticeManager.Instance != null)
        {
          DailyNoticeManager.Instance._isDayLyTask = true;
        }
      }
      else
      {
        // Nếu chưa đạt tiến trình, nút không thể nhấn
        _arrayReward[i].transform.GetChild(2).gameObject.SetActive(true); // Tắt tất cả các `Child`
        _arrayReward[i].interactable = false;
      }
    }
  }

  void AssignRewardButtonEvents()
  {
    // Gán sự kiện cho các nút phần quà
    for (int i = 0; i < _arrayReward.Length; i++)
    {
      int rewardIndex = i; // Tạo bản sao để tránh lỗi closure
      _arrayReward[i].onClick.RemoveAllListeners(); // Xóa sự kiện cũ
      _arrayReward[i].onClick.AddListener(() => OnRewardClaimed(rewardIndex)); // Thêm sự kiện mới
    }
  }
  void OnRewardClaimed(int rewardIndex) // button nhận quà
  {
    // SoundSystem.Instance.PlayCollectRewardSfx();
    if (DailyNoticeManager.Instance != null)
    {
      DailyNoticeManager.Instance._isDayLyTask = false;
    }
    // Vô hiệu hóa nút sau khi nhận quà
    _arrayReward[rewardIndex].interactable = false;
    PlayerPrefs.SetInt($"RewardClaimed_{rewardIndex}", 1);
    // Xử lý logic nhận phần quà ở đây (cộng tiền, vật phẩm, v.v.)
    // Ví dụ:
    // AddCoins(100 * (rewardIndex + 1));
    _arrayReward[rewardIndex].gameObject.transform.GetChild(0).gameObject.SetActive(true);
    _arrayReward[rewardIndex].gameObject.transform.GetChild(1).gameObject.SetActive(false);
    _arrayReward[rewardIndex].gameObject.transform.GetChild(2).gameObject.SetActive(false);
    switch (rewardIndex)
    {
      case 0:
        GameSystem.Instance.CurrentCoin += 100;
        ShowAnimReward(rewardIndex);

        break;

      case 1:

        GameSystem.Instance.CurrentCoin += 150;
        ShowAnimReward(rewardIndex);

        break;

      case 2:
        GameSystem.Instance.CurrentCoin += 200;
        ShowAnimReward(rewardIndex);

        break;
    }
  }
  void ResetNextDay()
  {
    Debug.Log("---ResetNextDay");
    _currentValueTask = 0;
    PlayerPrefs.SetInt("CurrentValuetodayTask", 0);

    for (int i = 0; i < _arrayReward.Length; i++)
    {
      PlayerPrefs.SetInt($"RewardClaimed_{i}", 0);
    }
    // Reset trạng thái _isPick cho tất cả task
    foreach (var task in DailyTaskManager.Instance._taskDataJsonBase.TasksTodayJson)
    {
      task._isPick = false; // reset _ispick
      task._isSuccess = false;
      task._currentValue = 0;
    }
    // Random lại một task cho mỗi type
    RandomizeTasksPerType();

    // Lưu trạng thái sau khi reset
    DailyTaskManager.Instance.SaveDataJsonTask();
  }
  void RandomizeTasksPerType()
  {
    // Group các nhiệm vụ theo type
    Dictionary<int, List<DailyTaskManager.TaskDataJsonBase.Task>> tasksByType = new Dictionary<int, List<DailyTaskManager.TaskDataJsonBase.Task>>();

    foreach (var task in DailyTaskManager.Instance._taskDataJsonBase.TasksTodayJson)
    {
      if (!tasksByType.ContainsKey(task._isTypeTask)) // `task._idTask` có thể được thay bằng thuộc tính `Type` nếu có
      {
        tasksByType[task._isTypeTask] = new List<DailyTaskManager.TaskDataJsonBase.Task>();
      }
      tasksByType[task._isTypeTask].Add(task);
    }

    // Random chọn 1 task từ mỗi type
    foreach (var kvp in tasksByType)
    {
      var taskList = kvp.Value;
      if (taskList.Count > 0)
      {
        int randomIndex = UnityEngine.Random.Range(0, taskList.Count);
        taskList[randomIndex]._isPick = true; // Đặt task được chọn
      }
    }
  }


  void CreatPrefabTask()
  {
    // Xóa các prefab cũ (nếu cần thiết)
    foreach (Transform child in _parentTask.transform)
    {
      if (child != null)
      {
        Destroy(child.gameObject);
      }
    }

    if (DailyTaskManager.Instance == null)
    {
      return;
    }


    // Tạo các task được chọn (_isPick = true)
    foreach (var task in DailyTaskManager.Instance._taskDataJsonBase.TasksTodayJson)
    {
      if (!task._isPick) continue; // Chỉ tạo các task có _isPick = true

      GameObject newTaskPrefab = Instantiate(_prefabTask, _parentTask.transform);
      Image taskImgae = newTaskPrefab.transform.GetChild(0).GetComponent<Image>();
      TMP_Text taskNameText = newTaskPrefab.transform.GetChild(1).GetComponent<TMP_Text>();
      Slider taskProgressSlider = newTaskPrefab.transform.GetChild(2).GetComponent<Slider>();
      TMP_Text currentValueText = taskProgressSlider.transform.GetChild(taskProgressSlider.transform.childCount - 1).GetComponent<TMP_Text>();
      GameObject CheckBox = newTaskPrefab.transform.GetChild(3).gameObject;
      GameObject Tick = CheckBox.transform.GetChild(0).gameObject;
      Button collectButton = newTaskPrefab.transform.GetChild(4).GetComponent<Button>();

      // Cập nhật giao diện
      if (taskNameText != null)
      {
        taskNameText.text = task._strNameTask;
      }

      if (taskImgae != null)
      {

      }

      if (taskProgressSlider != null)
      {
        taskProgressSlider.maxValue = task._targetValue;
        taskProgressSlider.value = task._currentValue;
        currentValueText.text = $"{task._currentValue}/{task._targetValue}";
      }

      // Cập nhật trạng thái nút thu thập
      // collectButton.interactable = !task._isSuccess && task._currentValue >= task._targetValue;
      // collectButton.gameObject.SetActive(!task._isSuccess);
      UpdateCollectButtonState(collectButton, task, Tick);

      // Gán sự kiện cho nút thu thập
      collectButton.onClick.RemoveAllListeners();
      collectButton.onClick.AddListener(() =>
      {
        ActionButtonTask(collectButton.gameObject, task._idTask, Tick);
      });
    }

  }

  void UpdateCollectButtonState(Button collectButton, DailyTaskManager.TaskDataJsonBase.Task task, GameObject _Tick)
  {
    if (task._isSuccess)
    {
      // Nhiệm vụ đã hoàn thành, nút bị vô hiệu hóa và ẩn
      collectButton.interactable = false;
      collectButton.gameObject.SetActive(false);
      _Tick.gameObject.SetActive(true);
    }
    else if (task._currentValue >= task._targetValue)
    {
      // Nhiệm vụ đạt tiến trình yêu cầu, nút có thể nhấn
      _Tick.gameObject.SetActive(false);
      collectButton.interactable = true;
      collectButton.gameObject.SetActive(true);

      if (DailyNoticeManager.Instance != null)
      {
        DailyNoticeManager.Instance._isDayLyTask = true;
      }

    }
    else
    {
      // Nhiệm vụ chưa đạt tiến trình yêu cầu, nút bị vô hiệu hóa
      collectButton.interactable = false;
      collectButton.gameObject.SetActive(true); // Vẫn hiển thị nút nhưng không thể nhấn
      _Tick.gameObject.SetActive(false);
    }
  }
  void ActionButtonTask(GameObject _gameObject, int _idTask, GameObject _Tick)
  {

    if (DailyNoticeManager.Instance != null)
    {
      DailyNoticeManager.Instance._isDayLyTask = false;
    }
    _gameObject.gameObject.SetActive(false);
    _currentValueTask++;
    PlayerPrefs.SetInt("CurrentValuetodayTask", _currentValueTask);

    CheckValuesliderAll();
    UpdatSliderTaskAll(_Tick);
    DailyTaskManager.Instance._taskDataJsonBase.TasksTodayJson[_idTask]._isSuccess = true;
    DailyTaskManager.Instance.SaveDataJsonTask();
  }
  void UpdatSliderTaskAll(GameObject _tick)
  {
    _sliderTaskAll.DOKill();
    _sliderTaskAll.DOValue(_currentValueTask, 0.2f) // Tween đến giá trị mới trong 0.5 giây
    .SetEase(Ease.OutQuad) // Tăng tốc mượt mà
    .OnComplete(() =>
    {
      _tick.SetActive(true);
    });
  }

  void SetupAnimShowReward()
  {
    // Lặp qua tất cả các con (child) của _objShowReward
    for (int i = 0; i < _objContentParent.transform.childCount; i++)
    {
      Transform child = _objContentParent.transform.GetChild(i);
      child.gameObject.SetActive(false); // Tắt từng GameObject con
    }
    _objContentParent.SetActive(true);
    _objShowReward.SetActive(false);

  }
  void ShowAnimReward(int _intShow)
  {
    GameObject Target = _objContentParent.transform.GetChild(_intShow).gameObject;
    Target.transform.localScale = Vector3.zero;
    Target.SetActive(true);
    _objShowReward.SetActive(true);
    Target.transform.DOKill();
    Target.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear)
    .OnComplete(() =>
    {
      Target.GetComponent<Button>().onClick.RemoveAllListeners();
      Target.GetComponent<Button>().onClick.AddListener(() => BtnHideAnimReward(_intShow));
    });
  }
  void BtnHideAnimReward(int _intShow)
  {
    GameObject Target = _objContentParent.transform.GetChild(_intShow).gameObject;
    Target.transform.DOKill();
    Target.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.Linear)
    .OnComplete(() =>
    {
      _objShowReward.SetActive(false);
    });
  }

}