using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SignInData", menuName = "ScriptableObjects/SignInData", order = 1)]
public class SignInData : ScriptableObject
{
  [Serializable]
  public class SignInDay
  {
    public int IdDayName;    // Tên ngày (e.g., "Day 1", "Day 2")
    public bool isSuccess; // trạng thái đã nhận

  }
  public List<SignInDay> days = new List<SignInDay>();


}
