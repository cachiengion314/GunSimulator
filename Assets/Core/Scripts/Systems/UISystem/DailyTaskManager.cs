using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using HoangNam;

public class DailyTaskManager : MonoBehaviour
{
  public static DailyTaskManager Instance { get; private set; }
  [SerializeField] public TaskData _taskData;
  public TaskDataJsonBase _taskDataJsonBase;
  [SerializeField] int _idTestTask;

  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    SetupDataTask();
  }

  public void SetupDataTask()
  {
    if (!File.Exists(SaveSystem.GetSaveDataPathFrom("TaskData")))
    {

      ChangeScriptableObjectToJson();
    }
    else
    {

      LoadDataJsonTask();
    }
  }

  public void ChangeScriptableObjectToJson()
  {
    // Tạo mới _taskDataJsonBase
    _taskDataJsonBase = new TaskDataJsonBase();

    // Chuyển dữ liệu từ _taskData sang _taskDataJsonBase
    foreach (var task in _taskData.AllTasks)
    {
      _taskDataJsonBase.TasksTodayJson.Add(new TaskDataJsonBase.Task
      {
        _idTask = task._idTask,
        _strNameTask = task._strNameTask,
        _currentValue = task._currentValue,
        _targetValue = task._targetValue,
        _isSuccess = task._isSuccess,
        _isPick = task._isPick,
        _isTypeTask = (int)task._isTypeTask,
      });
    }
    // Lưu dữ liệu vào JSON
    SaveDataJsonTask();
  }

  public void SaveDataJsonTask()
  {
    SaveSystem.SaveWith(_taskDataJsonBase, "TaskData");
  }

  void LoadDataJsonTask()
  {
    _taskDataJsonBase = SaveSystem.LoadWith<TaskDataJsonBase>("TaskData");
  }

  public void UpdateTaskProgress(int taskId, int pointsToAdd)
  {
    var task = _taskDataJsonBase.TasksTodayJson.Find(t => t._idTask == taskId);
    if (task != null)
    {
      task._currentValue += pointsToAdd;

      if (task._currentValue >= task._targetValue)
      {
        task._currentValue = task._targetValue;
      }

      SaveDataJsonTask();
    }
  }

  [System.Serializable]
  public class TaskDataJsonBase
  {
    public List<Task> TasksTodayJson = new List<Task>();

    [Serializable]
    public class Task
    {
      public int _idTask;         // ID của task
      public string _strNameTask; // Tên task
      public int _currentValue;   // Giá trị hiện tại
      public int _targetValue;    // Giá trị mục tiêu
      public bool _isSuccess;     // Trạng thái đã nhận
      public bool _isPick;        // Trạng thái chọn
      public int _isTypeTask;   // ID nhóm nhiệm vụ
    }
  }

}
