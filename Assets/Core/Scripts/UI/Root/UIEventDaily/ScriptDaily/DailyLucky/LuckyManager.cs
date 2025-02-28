using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using HoangNam;
public class LuckyManager : MonoBehaviour
{
    // public static LuckyManager Instance { get; private set; }
    // [SerializeField] public LuckyData _luckyData;
    // public LuckyDataJsonBase _luckyJsonBase;
   
    // void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //     }
    //     SetupDataTask();
    // }

    // public void SetupDataTask()
    // {
    //     if (!File.Exists(SaveSystem.GetSaveDataPathFrom("TaskData")))
    //     {

    //         ChangeScriptableObjectToJson();
    //     }
    //     else
    //     {

    //         LoadDataJsonTask();
    //     }
    // }

    // public void ChangeScriptableObjectToJson()
    // {
    //     // Tạo mới _taskDataJsonBase
    //     _luckyJsonBase = new LuckyDataJsonBase();

    //     // Chuyển dữ liệu từ _taskData sang _taskDataJsonBase
    //     foreach (var task in _luckyData.AllItem)
    //     {
    //         _taskDataJsonBase.TasksTodayJson.Add(new TaskDataJsonBase.Task
    //         {
    //             _idTask = task._idTask,
    //             _strNameTask = task._strNameTask,
    //             _currentValue = task._currentValue,
    //             _targetValue = task._targetValue,
    //             _isSuccess = task._isSuccess,
    //             _isPick = task._isPick,
    //             _isTypeTask = (int)task._isTypeTask,
    //         });
    //     }
    //     // Lưu dữ liệu vào JSON
    //     SaveDataJsonTask();
    // }

    // public void SaveDataJsonTask()
    // {
    //     SaveSystem.SaveWith(_taskDataJsonBase, "TaskData");
    // }

    // void LoadDataJsonTask()
    // {
    //     _taskDataJsonBase = SaveSystem.LoadWith<TaskDataJsonBase>("TaskData");
    // }

    // public void UpdateTaskProgress(int taskId, int pointsToAdd)
    // {
    //     var task = _taskDataJsonBase.TasksTodayJson.Find(t => t._idTask == taskId);
    //     if (task != null)
    //     {
    //         task._currentValue += pointsToAdd;

    //         if (task._currentValue >= task._targetValue)
    //         {
    //             task._currentValue = task._targetValue;
    //         }

    //         SaveDataJsonTask();
    //     }
    // }

    // [System.Serializable]
    // public class LuckyDataJsonBase
    // {
    //     public List<luckyDataJson> LuckysAllDataJson = new List<luckyDataJson>();

    //     [Serializable]
    //     public class luckyDataJson
    //     {
    //         public int _idItem;// ID của Item
    //         public string _strItem; // Tên Item
    //         public float _percentage; // tỉ lệ phần trăm
    //         public RewardDataLuckyTypeJson[] _typeItem;
    //     }
    //     [Serializable]
    //     public struct RewardDataLuckyTypeJson
    //     {
    //         public int _currentValue;   // Giá trị hiện tại
    //         public int _typeItem;

    //     }

    // }
}
