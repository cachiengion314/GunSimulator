using UnityEngine;
using System;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "DataExplosion", menuName = "ScriptableObjects/DataExplosion", order = 1)]
public class DataExplosion : ScriptableObject
{
    [Serializable]
    public class ExplosionItem
    {
        public TypeExplosion _typExplosion; // Loại súng
        public int _idExplosion;// ID của Item
        public string _strNameExplosion; // Tên Item
        public int _currentStartValue;   // Giá trị hiện tại
        public int _currentValue;   // Giá trị hiện tại
        public float _speedExplosion; // thời gian nạp đạn
        public bool _isOwned; // súng đang được chọn
        public bool _isPick; // súng đang được chọn
    }
    public List<ExplosionItem> ListDataExplosion = new List<ExplosionItem>();
    private void OnValidate()
    {
        Dictionary<TypeExplosion, int> ExplosionTypeCount = new Dictionary<TypeExplosion, int>();
        for (int i = 0; i < ListDataExplosion.Count; i++)
        {
            TypeExplosion type = ListDataExplosion[i]._typExplosion;

            // Nếu TypeGun chưa tồn tại trong Dictionary, bắt đầu ID từ 0
            if (!ExplosionTypeCount.ContainsKey(type))
            {
                ExplosionTypeCount[type] = 0;
            }
            else
            {
                ExplosionTypeCount[type]++; // Nếu đã tồn tại, tăng ID tiếp theo
            }
            ListDataExplosion[i]._idExplosion = ExplosionTypeCount[type]; // Gán ID theo nhóm TypeGun
        }
    }
    public enum TypeExplosion
    {
        Granade,
        Bomb
    }

}
