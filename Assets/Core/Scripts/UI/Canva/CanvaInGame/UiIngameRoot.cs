using UnityEngine;
using HoangNam;
using UnityEngine.UI;
using TMPro;


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
  [SerializeField] GameObject playerController;
  [SerializeField] TMP_Text textCurrentBullet;
  public Button[] TypeFireModes;
  [SerializeField] GunControl gunControl;
  private void Start()
  {
    Instance = this;
    // UpdateTextCurrentBullet();
    // SetupButtons();
    // SetUpTextCurrentBullet();
    ItemSystem.Instance.OnFire += UpdateBullet;
    var gunData = DataGunManager.Instance.GetGunDataClass(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick);
    int _currentValueGun = gunData._currentValue;
    textCurrentBullet.text = _currentValueGun.ToString();
  }
  void UpdateBullet(int _value)
  {
    DataGunManager.Instance.UpdateGunCurrenValue(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick, _value);
    var gunData = DataGunManager.Instance.GetGunDataClass(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick);
    int _currentValueGun = gunData._currentValue;
    textCurrentBullet.text = _currentValueGun.ToString();
  }

  void SetUpTextCurrentBullet()
  {
    textCurrentBullet.text = gunControl.GetCurrentAmmo().ToString();
  }

  public void UpdateTextCurrentBullet()
  {
    var gunData = DataGunManager.Instance.GetGunDataClass(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick);
    int _currentValueGun = gunData._currentValue;
    textCurrentBullet.text = _currentValueGun.ToString();
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



}
