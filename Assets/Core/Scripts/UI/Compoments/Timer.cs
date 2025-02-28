using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
  [SerializeField] int maxTimeInSeconds = 1200;
  private DateTime startTime;  // Thời điểm bắt đầu đếm thời gian
  bool isGetStartTime = false;
  public static Action<string> onUpdateTimer;

  private void Start()
  {

  }

  void Update()
  {

  }

  // Hàm cập nhật giao diện hiển thị thời gian
  void UpdateTimerDisplay(float timeInSeconds)
  {

  }

  public void StartTimer(int extraTime)
  {

  }
}
