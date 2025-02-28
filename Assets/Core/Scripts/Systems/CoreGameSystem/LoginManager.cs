using System;
using HoangNam;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
  public static LoginManager Instance { get; private set; }
  [SerializeField] int lengthData = 1;
  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      Login();
    }
  }

  public bool IsLogin()
  {
    var data = SaveSystem.LoadWith<LoginDatas>(KeyStr.NAME_LOGIN_DATA);
    if (data == null)
    {
      return false;
    }
    else
    {
      if (data.loginDatas == null) return false;
      string STRToday = DateTime.Now.ToString("yyyyMMdd");
      foreach (var day in data.loginDatas)
      {
        if (day.day.Equals(STRToday))
        {
          return true;
        }
      }
      return false;
    }
  }

  public void Login()
  {
    if (!IsLogin())
    {
      var data = SaveSystem.LoadWith<LoginDatas>(KeyStr.NAME_LOGIN_DATA);
      if (data == null)
      {
        data = new LoginDatas()
        {
          loginDatas = new LoginData[lengthData]
        };
      }
      else
      {
        if (data.loginDatas == null)
        {
          data.loginDatas = new LoginData[lengthData];
        }
      }

      var dayData = new LoginData()
      {
        day = DateTime.Now.ToString("yyyyMMdd"),
        amountRewardAds = 3
      };

      for (int i = 0; i < data.loginDatas.Length; i++)
      {
        if (data.loginDatas[i] != null) continue;
        data.loginDatas[i] = dayData;
        SaveSystem.SaveWith(data, KeyStr.NAME_LOGIN_DATA);
        return;
      }

      Array.Clear(data.loginDatas, 0, data.loginDatas.Length);
      data.loginDatas[0] = dayData;
      SaveSystem.SaveWith(data, KeyStr.NAME_LOGIN_DATA);
    }
  }

  public LoginData GetDayData(string day)
  {
    var data = SaveSystem.LoadWith<LoginDatas>(KeyStr.NAME_LOGIN_DATA);
    if (data.loginDatas == null) return null;

    foreach (var dayData in data.loginDatas)
    {
      if (dayData.day.Equals(day))
      {
        return dayData;
      }
    }
    return null;
  }

  public void ChangeDayData(LoginData dayData)
  {
    var data = SaveSystem.LoadWith<LoginDatas>(KeyStr.NAME_LOGIN_DATA);
    if (data == null) return;
    if (data.loginDatas == null) return;
    for (int i = 0; i < data.loginDatas.Length; i++)
    {
      if (data.loginDatas[i].day != dayData.day) continue;
      data.loginDatas[i] = dayData;
      SaveSystem.SaveWith(data, KeyStr.NAME_LOGIN_DATA);
      return;
    }
  }
}
[Serializable]
public class LoginData
{
  public string day;
  public int amountRewardAds;
}

[Serializable]
public class LoginDatas
{
  public LoginData[] loginDatas;
}
