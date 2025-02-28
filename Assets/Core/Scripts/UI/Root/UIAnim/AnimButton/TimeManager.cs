using System;
using HoangNam;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class UiIngameRoot : BaseUIRoot
{
  // [SerializeField] TMP_Text _textTime;
  // public float _timeStart; // thời gian chuyền vào mỗi level
  // private bool _isTimeOutHandled = false; // Cờ kiểm tra xem HandleTimeOut đã được gọi chưa
  // [Header("Events")]
  // public Action onTimeOut;

  // void Update()
  // {
  //   // UpdateTime();
  // }

  // void UpdateTime()
  // {
  //   if (GameSystem.Instance.GetGameState() != GameState.Gameplay) return;
  //   if (_timeStart == -1)
  //   {
  //     return;
  //   }

  //   if (_timeStart > 0)
  //   {
  //     _timeStart -= Time.deltaTime;
  //     _timeStart = Mathf.Max(_timeStart, 0); // Đảm bảo không nhỏ hơn 0
  //                                            // Định dạng thời gian thành phút:giây
  //     int minutes = Mathf.FloorToInt(_timeStart / 60);
  //     int seconds = Mathf.FloorToInt(_timeStart % 60);
  //     _textTime.text = $"{minutes:D2}:{seconds:D2}";
  //   }
  //   else
  //   {
  //     if (_isTimeOutHandled == false)
  //     {
  //       UIManager.Instance.Show(KeyStr.NAME_LOSE_1_LEVEL_MODAL);
  //       HandleTimeOut();
  //       _isTimeOutHandled = true;
  //     }
  //   }
  // }

  // void HandleTimeOut()
  // {
  //   if (_timeStart <= 0)
  //   {
  //     onTimeOut?.Invoke();
  //   }
  // }
  // public bool GetIsBestTime()
  // {
  //   // int Level = GameSystem.Instance.CurrentLevel;
  //   // string TextBestTimeLevelCurrent = KeyStr.BestTime + Level;

  //   // var levelDesignObj = LevelManager.Instance.GetCurrentLevelDesign();
  //   // float loatCurrenttimeLevel = levelDesignObj.TotalSecond;
  //   // float Currenttime = loatCurrenttimeLevel - _timeStart;
  //   // float BestTime = PlayerPrefs.GetFloat(TextBestTimeLevelCurrent);
  //   // if (BestTime == 0)
  //   // {
  //   //   PlayerPrefs.SetFloat(TextBestTimeLevelCurrent, Currenttime);
  //   // }
  //   // BestTime = PlayerPrefs.GetFloat(TextBestTimeLevelCurrent);
  //   // if (Currenttime <= BestTime)
  //   // {
  //   //   Debug.Log("Currenttime--" + Currenttime);
  //   //   PlayerPrefs.SetFloat(TextBestTimeLevelCurrent, Currenttime);
  //   //   return true;
  //   // }
   
  //   return false;
  // }
  
}
