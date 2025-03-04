using UnityEngine;
using HoangNam;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;
using System.Collections;
using DG.Tweening;
using System;


public partial class UiIngameRoot : BaseUIRoot
{
  static public UiIngameRoot Instance { get; private set; }
  [Header("--SetupIngame--")]
  [SerializeField] GameObject parentSetupTypeGun;
  [SerializeField] GameObject parentSetupTypeLight;
  [SerializeField] GameObject parentSetupTypeExplosion;
  [Header("---ShopIngame---")]
  public Button _btnShowShopCoin;
  [Header("---Gun---")]
  [SerializeField] TMP_Text textCurrentBullet;
  public Button[] TypeFireModes;
  [Header("--Bullet--")]
  [SerializeField] GameObject parentBullet;
  [SerializeField] GameObject PrefabBullet;
  public ColorPickerControl colorPickerControl;
  [SerializeField] Slider capacity;
  [Header("---Explosion---")]
  [SerializeField] TMP_Text valueExplosion;
  [SerializeField] TMP_Dropdown _valueTimeExplosion;
  private void Awake()
  {
    Instance = this;
  }
  private void Start()
  {

    switch (GameSystem.Instance.IdShopMode)
    {
      case 0:
        SetupTypeGun();
        parentSetupTypeGun.gameObject.SetActive(true);
        parentSetupTypeExplosion.gameObject.SetActive(false);
        parentSetupTypeLight.gameObject.SetActive(false);

        break;
      case 1:
        parentSetupTypeGun.gameObject.SetActive(false);
        parentSetupTypeExplosion.gameObject.SetActive(true);
        parentSetupTypeLight.gameObject.SetActive(false);

        _valueTimeExplosion.value = 0;
        _valueTimeExplosion.onValueChanged.AddListener(UpdateExplosionTime);

        break;
      case 2:
        break;
      case 3:
        StartCoroutine(AddActionToCurrentLightsaber());
        parentSetupTypeGun.gameObject.SetActive(false);
        parentSetupTypeExplosion.gameObject.SetActive(false);
        parentSetupTypeLight.gameObject.SetActive(true);
        break;
    }
  }


  IEnumerator AddActionToCurrentLightsaber()
  {
    yield return new WaitUntil(() => ItemSystem.Instance.CurrentLightsaber);
    ItemSystem.Instance.CurrentLightsaber.OnCurrentCapacityChange += OnCurrentCapacityChange;
    ItemSystem.Instance.CurrentLightsaber.OnCapacityEmpty += LightSaberShowPoPupBuyBullet;
  }
  void SetupTypeGun()
  {
    SetupButtonsTypeFire();
    ItemSystem.Instance.OnFire += UpdateBullet;
    ItemSystem.Instance.OnIsAuto += UpdateButtonType;
    TouchDetect.Instance.onTouchEnd += SetTypeSingleButton;
    ItemSystem.Instance.OnOutOfAmmo += ShowPoPupBuyBullet;

    var gunData = DataGunManager.Instance.GetGunDataClass(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdWeaponsPick);
    int _currentValueGun = gunData._currentValue;
    textCurrentBullet.text = _currentValueGun.ToString();
    SetUpBullet();
  }

  void OnDestroy()
  {
    if (!ItemSystem.Instance.CurrentLightsaber) return;
    ItemSystem.Instance.CurrentLightsaber.OnCurrentCapacityChange -= OnCurrentCapacityChange;
    ItemSystem.Instance.CurrentLightsaber.OnCapacityEmpty -= LightSaberShowPoPupBuyBullet;
  }

  void OnCurrentCapacityChange()
  {
    var maxCapacity = ItemSystem.Instance.CurrentLightsaber.Capacity;
    var currentCapacity = ItemSystem.Instance.CurrentLightsaber.CurrentCapacity;
    var value = currentCapacity / maxCapacity;
    capacity.value = value;
  }

  void SetTypeSingleButton(float2 _test, float2 _test2)
  {
    TypeFireModes[0].gameObject.SetActive(true);
    TypeFireModes[1].gameObject.SetActive(false);
  }

  void UpdateButtonType(bool _isAuto)
  {
    TypeFireModes[0].gameObject.SetActive(!_isAuto);
    TypeFireModes[1].gameObject.SetActive(_isAuto);
  }

  public void UpdateBullet(int _value)
  {
    DataGunManager.Instance.UpdateGunCurrenValue(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdWeaponsPick, _value);
    // UpdateTextCurrentBullet();
    UpdateBullet();
  }
  void SetUpBullet()
  {
    var _gunData = DataGunManager.Instance.GetGunDataClassPick();
    int _bulletStart = _gunData._currentStartValue;
    int _bulletCurrent = _gunData._currentValue;
    for (int i = 0; i < _bulletStart; i++)
    {
      GameObject bullet = Instantiate(PrefabBullet, parentBullet.transform);
      bullet.SetActive(i < _bulletCurrent);
    }
  }
  void UpdateBullet()
  {
    //lữu giữ liệu vào GunControl
    var _dataGunTarget = DataGunManager.Instance.GetGunDataClassPick();
    int intCurrentAmmo = _dataGunTarget._currentValue;
    var _currentGun = ItemSystem.Instance.GetCurrentGun();
    _currentGun.GetComponent<GunControl>().SetCurrentAmmo(intCurrentAmmo);
    int index = _currentGun.GetComponent<GunControl>().CurrentAmmo;
    textCurrentBullet.text = "x " + index.ToString();
    Debug.Log("index : " + index);
    // cập nhật hình ảnh
    GameObject _bulletTarget = parentBullet.transform.GetChild(index).gameObject;
    if (_bulletTarget != null)
    {
      _bulletTarget.gameObject.SetActive(false);
    }

  }

  public void UpdateCurrentBulletBuyFull()
  {
    var _dataGunTarget = DataGunManager.Instance.GetGunDataClassPick();
    int intCurrentAmmo = _dataGunTarget._currentValue;
    var _currentGun = ItemSystem.Instance.GetCurrentGun();
    _currentGun.GetComponent<GunControl>().SetCurrentAmmo(intCurrentAmmo);
    // update Text + imgae
    int index = _currentGun.GetComponent<GunControl>().CurrentAmmo;
    textCurrentBullet.text = "x " + index.ToString();
    foreach (Transform child in parentBullet.transform)
    {
      child.gameObject.SetActive(true);
    }
  }
  void ShowPoPupBuyBullet()
  {
    UIManager.Instance.Show(KeyStr.NAME_BuyBullet_MODAL);
  }
  void LightSaberShowPoPupBuyBullet()
  {
    var Test = ItemSystem.Instance.CurrentLightsaber;// Gameobjcet kiếm Tắt
    Test.gameObject.transform.GetChild(1).gameObject.SetActive(false);// Gameobjcet kiếm Tắt
    UIManager.Instance.Show(KeyStr.NAME_BuyBullet_MODAL);
  }

  public void BtnSettingIngame()
  {
    UIManager.Instance.Show(KeyStr.NAME_SETTING_2_MODAL);
  }

  public void BtnShopIngame()
  {
    UIManager.Instance.Show(KeyStr.NAME_SHOP_MODAL);
    _btnShowShopCoin.interactable = false;

  }

  public void BtnBackLobbyIngame() // gắn vào button back home
  {
    DOTween.KillAll();
    GameSystem.Instance.isBombing = false;
    GameSystem.Instance.explosionTime = 5f;
    GameSystem.Instance.LoadSceneByName(KeyStr.NAME_SCENE_LOBBY);

  }

  void SetupButtonsTypeFire()
  {
    foreach (var type in TypeFireModes)
    {
      type.gameObject.SetActive(false);
    }
    var GunData = DataGunManager.Instance.GetGunDataClassPick();

    for (int i = 0; i < GunData._fireModes.Length; i++)
    {
      if (GunData._fireModes.Length == 3)
      {
        TypeFireModes[0].gameObject.SetActive(true);
        TypeFireModes[1].gameObject.SetActive(false);
        TypeFireModes[2].gameObject.SetActive(true);
      }
      else
      {
        TypeFireModes[i].gameObject.SetActive(true);
      }
    }

  }

  public void BtnSetTypeGun(int _idType) // gắn ở button từ 0 - 2 0= single 1= auto. 2  = bủrt  
  {
    GameSystem.Instance.IdFireModes = _idType;
  }
  // void GunHaveBurt()
  // {
  //   TypeFireModes[2].gameObject.SetActive(true);
  // }

  private void UpdateExplosionTime(int index)
  {
    var currenExplosion = ItemSystem.Instance.GetCurrentExplosion();
    switch (index)
    {
      case 0:
        DOTween.KillAll();
        GameSystem.Instance.explosionTime = 5f;
        GameSystem.Instance.isBombing = false;
        currenExplosion.GetComponent<ExplosionControl>().textTime.text = "00:05";
        break;
      case 1:
        DOTween.KillAll();
        GameSystem.Instance.explosionTime = 10f;
        GameSystem.Instance.isBombing = false;
        currenExplosion.GetComponent<ExplosionControl>().textTime.text = "00:10";
        break;
      case 2:
        DOTween.KillAll();
        GameSystem.Instance.explosionTime = 15f;
        GameSystem.Instance.isBombing = false;
        currenExplosion.GetComponent<ExplosionControl>().textTime.text = "00:15";
        break;
      case 3:
        DOTween.KillAll();
        GameSystem.Instance.explosionTime = 30f;
        GameSystem.Instance.isBombing = false;
        currenExplosion.GetComponent<ExplosionControl>().textTime.text = "00:30";
        break;
      case 4:
        DOTween.KillAll();
        GameSystem.Instance.explosionTime = 45f;
        GameSystem.Instance.isBombing = false;
        currenExplosion.GetComponent<ExplosionControl>().textTime.text = "00:45";
        break;
      case 5:
        DOTween.KillAll();
        GameSystem.Instance.explosionTime = 60f;
        GameSystem.Instance.isBombing = false;
        currenExplosion.GetComponent<ExplosionControl>().textTime.text = "00:60";
        break;
    }

    Debug.Log($"⏳ Thời gian phát nổ đã cập nhật: {GameSystem.Instance.explosionTime}s");
  }


}
