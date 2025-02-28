using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LuckyData", menuName = "ScriptableObjects/LuckyData", order = 1)]

public class LuckyData : ScriptableObject
{
    [Serializable]
    public struct RewardDataLuckyType
    {
        public int _currentValue;   // Giá trị hiện tại
        public TypeItem _typeItem;

    }

    [Serializable]
    public class LuckyItem
    {
        public int _idItem;// ID của Item
        public string _strItem; // Tên Item
        // public int _currentValue;   // Giá trị hiện tại
        public float _percentage; // tỉ lệ phần trăm
        public RewardDataLuckyType[] _typeItem;
    }
    public List<LuckyItem> AllItem = new List<LuckyItem>();
    private void OnValidate()
    {
        for (int i = 0; i < AllItem.Count; i++)
        {
            AllItem[i]._idItem = i; // Gán ID theo thứ tự
        }
    }
    public enum TypeItem
    {
        Coin,
        Booster,
        Heart , 
        RemoveAds

    }
}