using UnityEngine;
using HoangNam;
using System.IO;
using System.Collections.Generic;
using System;

public class DataExplosionManager : MonoBehaviour
{
    public static DataExplosionManager Instance { get; private set; }
    public DataExplosion _dataExplosion;
    public ExplosionDataJsonBase _ExplosionDataJsonBase;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        SetupDataTask();
    }
    public void SetupDataTask()
    {
        if (!File.Exists(SaveSystem.GetSaveDataPathFrom("ExplosionData")))
        {

            ChangeScriptableObjectToJson();
        }
        else
        {
            LoadDataJsonTask();
        }
    }

    public void ChangeScriptableObjectToJson()
    {
        // Tạo mới _taskDataJsonBase
        _ExplosionDataJsonBase = new ExplosionDataJsonBase();

        // Chuyển dữ liệu từ _taskData sang _taskDataJsonBase
        foreach (var Explosion in _dataExplosion.ListDataExplosion)
        {
            _ExplosionDataJsonBase.ListExplosionsJson.Add(new ExplosionDataJsonBase.ExplosionDataClassSave
            {
                _idExplosion = Explosion._idExplosion,
                _strNameExplosion = Explosion._strNameExplosion,
                _currentStartValue = Explosion._currentStartValue,
                _currentValue = Explosion._currentValue,
                _speedExplosion = Explosion._speedExplosion,
                _typExplosion = (int)Explosion._typExplosion,
                _isOwned = Explosion._isOwned,
                _isPick = Explosion._isPick,
            });
        }
        // Lưu dữ liệu vào JSON
        SaveDataJsonTask();
    }

    public void SaveDataJsonTask()
    {
        SaveSystem.SaveWith(_ExplosionDataJsonBase, "ExplosionData");
    }
    void LoadDataJsonTask()
    {
        _ExplosionDataJsonBase = SaveSystem.LoadWith<ExplosionDataJsonBase>("ExplosionData");
    }

    public void UpdateGunCurrenValue(int IdTypeExplosion, int IdExplosion, int minusCurrent)
    {
        var explosionData = GetGunDataClass(IdTypeExplosion, IdExplosion);
        if (explosionData != null)
        {
            // Trừ đạn nhưng không cho xuống dưới 0
            explosionData._currentValue -= minusCurrent;
            if (explosionData._currentValue < 0)
            {
                explosionData._currentValue = 0;
            }
            // Lưu lại dữ liệu
            SaveDataJsonTask();
        }
        else
        {
            Debug.LogError($"Không tìm thấy súng với IdGun: {IdExplosion} và IdTypeGun: {IdTypeExplosion}");
        }

    }
    public void ResetDataTest(int IdTypeGun, int IdGun)
    {
        var GunData = GetGunDataClass(IdTypeGun, IdGun);
        GunData._currentValue = GunData._currentStartValue;
        SaveDataJsonTask();
    }
    public ExplosionDataJsonBase.ExplosionDataClassSave GetGunDataClass(int IdTypeGun, int IdGun) // lấy ra data của gun đó (chiều vào dạng sung , chiều tiếp id súng)
    {
        var GunData = _ExplosionDataJsonBase.ListExplosionsJson.Find(t => t._typExplosion == IdTypeGun && t._idExplosion == IdGun);
        return GunData;
    }
    public ExplosionDataJsonBase.ExplosionDataClassSave GetGunDataClassPick() // lấy ra data của gun đó (chiều vào dạng sung , chiều tiếp id súng)
    {
        var pickedGun = _ExplosionDataJsonBase.ListExplosionsJson.Find(gun => gun._isPick == true);

        if (pickedGun != null)
        {
            return pickedGun;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy súng nào đang được chọn!");
            return null; // Nếu không có súng nào được chọn
        }
    }
    public void ResetIsPickData()
    {
        foreach (var pickedGun in _ExplosionDataJsonBase.ListExplosionsJson)
        {
            pickedGun._isPick = false;
        }
        SaveDataJsonTask();
    }
    public ExplosionDataJsonBase.ExplosionDataClassSave SelectGun(int typeGun, int idGun)
    {
        // Reset tất cả súng trước khi chọn mới
        ResetIsPickData();
        // Tìm súng theo type và id
        var selectedGun = _ExplosionDataJsonBase.ListExplosionsJson.Find(gun => gun._typExplosion == typeGun && gun._idExplosion == idGun);

        selectedGun._isPick = true;
        SaveDataJsonTask();
        Debug.Log($"✅ Súng {selectedGun._strNameExplosion} (ID: {idGun}, Type: {typeGun}) đã được chọn.");
        return selectedGun;
    }

    public void BuyGun(int typeGun, int idGun)
    {
        if (_ExplosionDataJsonBase == null || _ExplosionDataJsonBase.ListExplosionsJson == null)
        {
            Debug.LogError("❌ _GunDataJsonBase hoặc ListGunsJson chưa được khởi tạo!");
            return;
        }

        var gunToBuy = _ExplosionDataJsonBase.ListExplosionsJson.Find(gun => gun._typExplosion == typeGun && gun._idExplosion == idGun);

        if (gunToBuy != null)
        {
            if (gunToBuy._isOwned)
            {
                Debug.LogWarning($"⚠️ Súng {gunToBuy._strNameExplosion} đã được mua trước đó.");
                return;
            }

            gunToBuy._isOwned = true;
            SaveDataJsonTask();
            Debug.Log($"✅ Đã mua súng {gunToBuy._strNameExplosion} (ID: {idGun}, Type: {typeGun}).");
        }
        else
        {
            Debug.LogWarning($"⚠️ Không tìm thấy súng với ID {idGun} và Type {typeGun}.");
        }
    }
    [System.Serializable]
    public class ExplosionDataJsonBase
    {
        public List<ExplosionDataClassSave> ListExplosionsJson = new List<ExplosionDataClassSave>();

        [Serializable]
        public class ExplosionDataClassSave
        {
            public int _typExplosion; // Loại súng
            public int _idExplosion;// ID của Item
            public string _strNameExplosion; // Tên Item
            public int _currentStartValue;   // Giá trị hiện tại
            public int _currentValue;   // Giá trị hiện tại
            public float _speedExplosion; // thời gian nạp đạn
            public bool _isOwned; // súng đang được chọn
            public bool _isPick; // súng đang được chọn
        }
    }
}
