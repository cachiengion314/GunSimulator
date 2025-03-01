using UnityEngine;
using HoangNam;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;


public partial class UiIngameRoot : BaseUIRoot
{
  static public UiIngameRoot Instance { get; private set; }
  [Header("---ShopIngame---")]
  public Button _btnShowShopCoin;

  [Header("---TextTarget---")]
  [SerializeField] TMP_Text textTatget;
  [SerializeField] string strTextTarget;

  [Header("---IsUsingBooster---")]
  [SerializeField] GameObject cavanIsUsingBooster;
  [SerializeField] GameObject[] arrayButton;
  [Header("---Gun---")]
  [SerializeField] TMP_Text textCurrentBullet;
  public Button[] TypeFireModes;

  public ColorPickerControl colorPickerControl;
  private void Awake()
  {
    Instance = this;
  }
  private void Start()
  {
    SetupButtons();
    ItemSystem.Instance.OnFire += UpdateBullet;
    ItemSystem.Instance.OnIsAuto += UpdateButtonType;
    TouchDetect.Instance.onTouchEnd += SetTypeSingleButton;
    ItemSystem.Instance.OnOutOfAmmo += ShowPoPupBuyBullet;
    ItemSystem.Instance.OnHaveBurst += GunHaveBurt;
    var gunData = DataGunManager.Instance.GetGunDataClass(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick);
    int _currentValueGun = gunData._currentValue;
    textCurrentBullet.text = _currentValueGun.ToString();

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
    DataGunManager.Instance.UpdateGunCurrenValue(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick, _value);
    UpdateTextCurrentBullet();
  }

  public void UpdateTextCurrentBullet()
  {
    var _dataGunTarget = DataGunManager.Instance.GetGunDataClassPick();

    int intCurrentAmmo = _dataGunTarget._currentValue;

    var _currentGun = ItemSystem.Instance.GetCurrentGun();
    _currentGun.GetComponent<GunControl>().SetCurrentAmmo(intCurrentAmmo);

    int _currentValueGun = _currentGun.GetComponent<GunControl>().CurrentAmmo;

    textCurrentBullet.text = _currentValueGun.ToString();
  }
  void ShowPoPupBuyBullet()
  {
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

  public void BtnBackLobbyIngame()
  {
    GameSystem.Instance.LoadSceneByName(KeyStr.NAME_SCENE_LOBBY);

  }

  void SetupButtons()
  {
    foreach (var type in TypeFireModes)
    {
      type.gameObject.SetActive(false);
    }
    var GunData = DataGunManager.Instance.GetGunDataClassPick();
    // var _gunControl = ItemSystem.Instance.GetCurrentGun();
    // if (_gunControl == null)
    // {
    //   Debug.Log("_gunControl == null");
    //   return;
    // }
    // // for (int i = 0; i < GunData._fireModes.Length; i++)
    // // {
    // //   if (GunData._fireModes.Length == 3)
    // //   {
    // //     TypeFireModes[0].gameObject.SetActive(true);
    // //     TypeFireModes[1].gameObject.SetActive(false);
    // //     TypeFireModes[2].gameObject.SetActive(true);
    // //   }
    // //   else
    // //   {
    // //     TypeFireModes[i].gameObject.SetActive(true);
    // //   }
    // // }
    // if (_gunControl.GetComponent<GunControl>().HaveBurstMode == true)
    // {
    //   TypeFireModes[2].gameObject.SetActive(true);
    // }
    // else
    // {
    //   TypeFireModes[2].gameObject.SetActive(false);
    // }
    // TypeFireModes[0].gameObject.SetActive(true);
    // TypeFireModes[1].gameObject.SetActive(true);

  }

  public void BtnSetTypeGun(int _idType) // gắn ở button từ 0 - 2 0= single 1= auto. 2  = bủrt  
  {
    GameSystem.Instance.IdFireModes = _idType;
  }
  void GunHaveBurt()
  {
    TypeFireModes[2].gameObject.SetActive(true);
  }



}
