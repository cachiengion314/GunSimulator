using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TaskData", menuName = "ScriptableObjects/TaskData", order = 1)]
public class TaskData : ScriptableObject
{
  [Serializable]
  public class Taskday
  {
    public int _idTask;         // ID của task
    public string _strNameTask; // Tên task
    public int _currentValue;   // Giá trị hiện tại
    public int _targetValue;    // Giá trị mục tiêu
    public bool _isSuccess;     // Trạng thái đã nhận
    public bool _isPick;     // Trạng thái ngày hôm này được nhiệm vụ được chọn để sinh ra
    public TypeTask _isTypeTask;     // Trạng thái ngày hôm này được nhiệm vụ được chọn để sinh ra


  }
  public List<Taskday> AllTasks = new List<Taskday>();

  private void OnValidate()
  {
    for (int i = 0; i < AllTasks.Count; i++)
    {
      AllTasks[i]._idTask = i; // Gán ID theo thứ tự
    }
  }
  public enum TypeTask
  {
    Type1,
    Type2,
    Type3,
  }

}
