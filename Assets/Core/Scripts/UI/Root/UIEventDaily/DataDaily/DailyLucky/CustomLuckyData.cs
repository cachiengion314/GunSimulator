using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CustomLuckyData", menuName = "ScriptableObjects/CustomLuckyData", order = 1)]

public class CustomLuckyData : ScriptableObject
{
    [Serializable]
    public class LuckyItemCustom
    {
        public int _currentValue;   // Giá trị hiện tại
        public TypeCostum _typeItem;

    }

    public List<LuckyItemCustom> AllCustomItem = new List<LuckyItemCustom>();
    public enum TypeCostum
    {
        Coin,
        Booster,
        Heart,
        RemovAds
    
    }
}
