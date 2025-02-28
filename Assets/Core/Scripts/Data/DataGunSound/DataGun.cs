using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DataGun", menuName = "ScriptableObjects/DataGun", order = 1)]
public class DataGun : ScriptableObject
{

    [Serializable]
    public class GunItem
    {
        public TypeGun _typeGun; // Loại súng
        public int _idGun;// ID của Item
        public string _strGun; // Tên Item
        public int _currentStartValue;   // Giá trị hiện tại
        public int _currentValue;   // Giá trị hiện tại
        public float _speedATK; // thời gian nạp đạn
        public FireMode[] _fireModes; // Các chế độ bắn
        public bool _isOwned; // súng đang được chọn
        public bool _isPick; // súng đang được chọn
    }
    public List<GunItem> ListDataGun = new List<GunItem>();
    private void OnValidate()
    {
        Dictionary<TypeGun, int> gunTypeCount = new Dictionary<TypeGun, int>();
        for (int i = 0; i < ListDataGun.Count; i++)
        {
            TypeGun type = ListDataGun[i]._typeGun;

            // Nếu TypeGun chưa tồn tại trong Dictionary, bắt đầu ID từ 0
            if (!gunTypeCount.ContainsKey(type))
            {
                gunTypeCount[type] = 0;
            }
            else
            {
                gunTypeCount[type]++; // Nếu đã tồn tại, tăng ID tiếp theo
            }
            ListDataGun[i]._idGun = gunTypeCount[type]; // Gán ID theo nhóm TypeGun
        }
    }

    public enum TypeGun
    {
        Pistol,
        SMGGun,
        ShotGun,
        RifleGun,
        SniperGun

    }
    public enum FireMode
    {
        Single,
        Auto,
        Burst
    }
}
