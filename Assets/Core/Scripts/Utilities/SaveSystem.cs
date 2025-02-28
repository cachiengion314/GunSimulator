using UnityEngine;
using System.IO;
using System.Linq;

namespace HoangNam
{
  public class SaveSystem
  {
    public static string GetDataPathFrom(string dataName)
    {
      string dataPath;

#if UNITY_IOS
      dataPath = Application.persistentDataPath + "/" + dataName + ".txt";
#endif
#if UNITY_ANDROID
    dataPath = Application.persistentDataPath + "/" + dataName + ".txt";
#endif
#if UNITY_EDITOR
      dataPath = Application.dataPath + "/" + dataName + ".txt";
#endif

      return dataPath;
    }

    public static string GetSaveDataPathFrom(string dataName)
    {
      string dataPath;

#if UNITY_IOS
      dataPath = Application.persistentDataPath + "/" + dataName + ".save.txt";
#endif
#if UNITY_ANDROID
    dataPath = Application.persistentDataPath + "/" + dataName + ".save.txt";
#endif
#if UNITY_EDITOR
      dataPath = Application.dataPath + "/" + dataName + ".save.txt";
#endif

      return dataPath;
    }

    public static void RemoveFile(string name)
    {
      var _name = GetSaveDataPathFrom(name);
      if (File.Exists(_name))
        File.Delete(_name);
    }

    public static void WriteAllText(string name, string file)
    {
      var _name = GetSaveDataPathFrom(name);
      File.WriteAllText(_name, file);
    }

    public static string ReadAllText(string name)
    {
      var _name = GetSaveDataPathFrom(name);
      if (!File.Exists(_name)) return "";
      var _file = File.ReadAllText(_name);
      return _file;
    }

    public static string ParseIntArrToStr(int[] arr)
    {
      var _str_arr = arr.Aggregate("", (current, num) => current + (num + ","));
      return _str_arr;
    }

    public static int[] ParseStrArrToIntArr(string[] _file_arr)
    {
      int[] arr = new int[_file_arr.Length];
      for (int i = 0; i < arr.Length; ++i)
      {
        var result = int.TryParse(_file_arr[i], out int val);
        if (!result) continue;
        arr[i] = val;
      }
      return arr;
    }

    public static void SaveWith<T>(T data, string dataName)
    {
      var dataPath = GetSaveDataPathFrom(dataName);
      string _data = JsonUtility.ToJson(data, true);
      File.WriteAllText(dataPath, _data);
    }

    public static T LoadWith<T>(string dataName)
    {
      T temp = default;
      var dataPath = GetSaveDataPathFrom(dataName);

      if (File.Exists(dataPath))
      {
        string data = File.ReadAllText(dataPath);
        var tData = JsonUtility.FromJson<T>(data);

        return tData;
      }
      return temp;
    }

    public static void Save<T>(T data, string dataName)
    {

      var dataPath = GetDataPathFrom(dataName);
      string _data = JsonUtility.ToJson(data, true);
      File.WriteAllText(dataPath, _data);
    }

    public static T Load<T>(string dataName)
    {
      T temp = default;
      var dataPath = GetDataPathFrom(dataName);

      if (File.Exists(dataPath))
      {
        string data = File.ReadAllText(dataPath);
        var tData = JsonUtility.FromJson<T>(data);

        return tData;
      }
      return temp;
    }
  }
}