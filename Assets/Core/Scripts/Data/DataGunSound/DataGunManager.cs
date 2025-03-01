using UnityEngine;
using HoangNam;
using System.IO;
using System.Collections.Generic;
using System;


public class DataGunManager : MonoBehaviour
{
    public static DataGunManager Instance { get; private set; }
    public DataGun _dataGun;
    public GunDataJsonBase _GunDataJsonBase;
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
        if (!File.Exists(SaveSystem.GetSaveDataPathFrom("GunData")))
        {

            ChangeScriptableObjectToJson();
        }
        else
        {
            CheckUpdateDataGunVersion();
            LoadDataJsonTask();
        }
    }
    public void CheckUpdateDataGunVersion() // kiểm tra dữ liệu các nhiệm vụ xem có khớp với dữ lụu trong ScriptableObject
    {

        if (_GunDataJsonBase.ListGunsJson.Count != _dataGun.ListDataGun.Count)
        {
            // Nếu số lượng khác nhau hoặc file JSON không tồn tại, chạy lại ChangeScriptableObjectToJson
            ChangeScriptableObjectToJson();
            return;
        }

        // Duyệt từng súng trong ScriptableObject để kiểm tra sự thay đổi
        for (int i = 0; i < _dataGun.ListDataGun.Count; i++)
        {
            var scriptableGun = _dataGun.ListDataGun[i];

            // 🔥 Tìm kiếm theo cả typeGun và idGun
            var jsonGun = _GunDataJsonBase.ListGunsJson.Find(t => t._idGun == scriptableGun._idGun && t._typeGun == (int)scriptableGun._typeGun);

            if (jsonGun == null)
            {
                Debug.LogWarning($"⚠️ Không tìm thấy súng (ID: {scriptableGun._idGun}, Type: {scriptableGun._typeGun}) trong JSON. Cập nhật lại...");
                ChangeScriptableObjectToJson();
                return;
            }

            // Kiểm tra nếu tên súng, giá trị đạn hoặc chế độ bắn khác nhau -> Cập nhật lại JSON
            if (jsonGun._strGun != scriptableGun._strGun ||
                jsonGun._currentStartValue != scriptableGun._currentStartValue ||
                jsonGun._currentValue != scriptableGun._currentValue ||
                !AreFireModesEqual(jsonGun._fireModes, scriptableGun._fireModes))
            {
                Debug.LogWarning($"🔄 Dữ liệu súng {scriptableGun._strGun} đã thay đổi, cập nhật JSON...");
                ChangeScriptableObjectToJson();
                return;
            }
        }
    }
    // 🛠 Hàm hỗ trợ: Kiểm tra chế độ bắn giữa JSON và ScriptableObject
    private bool AreFireModesEqual(int[] jsonModes, DataGun.FireMode[] scriptableModes)
    {
        if (jsonModes.Length != scriptableModes.Length) return false;

        for (int i = 0; i < jsonModes.Length; i++)
        {
            if (jsonModes[i] != (int)scriptableModes[i]) return false;
        }
        return true;
    }


    public void ChangeScriptableObjectToJson()
    {
        // Tạo mới _taskDataJsonBase
        _GunDataJsonBase = new GunDataJsonBase();

        // Chuyển dữ liệu từ _taskData sang _taskDataJsonBase
        foreach (var Gun in _dataGun.ListDataGun)
        {
            _GunDataJsonBase.ListGunsJson.Add(new GunDataJsonBase.GunDataClassSave
            {
                _idGun = Gun._idGun,
                _strGun = Gun._strGun,
                _currentStartValue = Gun._currentStartValue,
                _currentValue = Gun._currentValue,
                _fireModes = Array.ConvertAll(Gun._fireModes, mode => (int)mode), // Lưu danh sách chế độ bắn
                _typeGun = (int)Gun._typeGun,
                _isOwned = Gun._isOwned,
                _isPick = Gun._isPick,
            });
        }
        // Lưu dữ liệu vào JSON
        SaveDataJsonTask();
    }

    public void SaveDataJsonTask()
    {
        SaveSystem.SaveWith(_GunDataJsonBase, "GunData");
    }
    void LoadDataJsonTask()
    {
        _GunDataJsonBase = SaveSystem.LoadWith<GunDataJsonBase>("GunData");
    }

    public void UpdateGunCurrenValue(int IdTypeGun, int IdGun, int minusCurrent)
    {
        var gunData = GetGunDataClass(IdTypeGun, IdGun);
        if (gunData != null)
        {
            // Trừ đạn nhưng không cho xuống dưới 0
            gunData._currentValue -= minusCurrent;
            if (gunData._currentValue < 0)
            {
                gunData._currentValue = 0;
            }
            // Lưu lại dữ liệu
            SaveDataJsonTask();
        }
        else
        {
            Debug.LogError($"Không tìm thấy súng với IdGun: {IdGun} và IdTypeGun: {IdTypeGun}");
        }

    }
    public void ResetDataTest(int IdTypeGun, int IdGun)
    {
        var GunData = GetGunDataClass(IdTypeGun, IdGun);
        GunData._currentValue = GunData._currentStartValue;
        SaveDataJsonTask();
    }
    public GunDataJsonBase.GunDataClassSave GetGunDataClass(int IdTypeGun, int IdGun) // lấy ra data của gun đó (chiều vào dạng sung , chiều tiếp id súng)
    {
        var GunData = _GunDataJsonBase.ListGunsJson.Find(t => t._typeGun == IdTypeGun && t._idGun == IdGun);
        return GunData;
    }
    public GunDataJsonBase.GunDataClassSave GetGunDataClassPick() // lấy ra data của gun đó (chiều vào dạng sung , chiều tiếp id súng)
    {
        var pickedGun = _GunDataJsonBase.ListGunsJson.Find(gun => gun._isPick == true);

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
        foreach (var pickedGun in _GunDataJsonBase.ListGunsJson)
        {
            pickedGun._isPick = false;
        }
        SaveDataJsonTask();
    }
    public GunDataJsonBase.GunDataClassSave SelectGun(int typeGun, int idGun)
    {
        // Reset tất cả súng trước khi chọn mới
        ResetIsPickData();
        // Tìm súng theo type và id
        var selectedGun = _GunDataJsonBase.ListGunsJson.Find(gun => gun._typeGun == typeGun && gun._idGun == idGun);

        selectedGun._isPick = true;
        SaveDataJsonTask();
        Debug.Log($"✅ Súng {selectedGun._strGun} (ID: {idGun}, Type: {typeGun}) đã được chọn.");
        return selectedGun;
    }

    public void BuyGun(int typeGun, int idGun)
    {
        if (_GunDataJsonBase == null || _GunDataJsonBase.ListGunsJson == null)
        {
            Debug.LogError("❌ _GunDataJsonBase hoặc ListGunsJson chưa được khởi tạo!");
            return;
        }

        var gunToBuy = _GunDataJsonBase.ListGunsJson.Find(gun => gun._typeGun == typeGun && gun._idGun == idGun);

        if (gunToBuy != null)
        {
            if (gunToBuy._isOwned)
            {
                Debug.LogWarning($"⚠️ Súng {gunToBuy._strGun} đã được mua trước đó.");
                return;
            }

            gunToBuy._isOwned = true;
            SaveDataJsonTask();
            Debug.Log($"✅ Đã mua súng {gunToBuy._strGun} (ID: {idGun}, Type: {typeGun}).");
        }
        else
        {
            Debug.LogWarning($"⚠️ Không tìm thấy súng với ID {idGun} và Type {typeGun}.");
        }
    }


    [System.Serializable]
    public class GunDataJsonBase
    {
        public List<GunDataClassSave> ListGunsJson = new List<GunDataClassSave>();

        [Serializable]
        public class GunDataClassSave
        {
            public int _idGun;// ID của Item
            public string _strGun; // Tên Item
            public int _currentStartValue;   // Giá trị hiện tại
            public int _currentValue;   // Giá trị hiện tại

            public int _typeGun; // dạng súng 
            public int[] _fireModes; // Chế độ bắn (danh sách kiểu số nguyên)
            public bool _isOwned; // sở hữu
            public bool _isPick; // súng đang được chọn
        }
    }



}
