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
  [SerializeField] TMP_Text _textTatget;
  [SerializeField] string _strTextTarget;

  [Header("---IsUsingBooster---")]
  [SerializeField] GameObject _CavanIsUsingBooster;
  [SerializeField] GameObject[] ArrayButton;
  [Header("---Gun---")]
  [SerializeField] GameObject _playerController;
  [SerializeField] TMP_Text _textCurrentBullet;
  public Button[] _typeFireModes;

  private void Start()
  {
    Instance = this;
    UpdateTextCurrentBullet();
    SetupButtons();
  }
  void Update()
  {
    // UpdateStatButton(GameSystem.Instance._IdFireModes);
  }
  // void UpdateStatButton(int _index)
  // {
  //   _typeFireModes[_index].gameObject.GetComponent<Image>().color = Color.green;
  // }
  public void UpdateTextCurrentBullet()
  {
    var gunData = DataGunManager.Instance.GetGunDataClass(GameSystem.Instance._idTypePick, GameSystem.Instance._idGunPick);
    int _currentValueGun = gunData._currentValue;
    _textCurrentBullet.text = _currentValueGun.ToString();
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
    foreach (var type in _typeFireModes)
    {
      type.gameObject.SetActive(false);
    }
    var GunData = DataGunManager.Instance.GetGunDataClassPick();
    for (int i = 0; i < GunData._fireModes.Length; i++)
    {
      if (GunData._fireModes.Length == 3)
      {
        _typeFireModes[0].gameObject.SetActive(true);
        _typeFireModes[1].gameObject.SetActive(false);
        _typeFireModes[2].gameObject.SetActive(true);
      }
      else
      {
        _typeFireModes[i].gameObject.SetActive(true);
      }
    }
  }

  public void BtnSetTypeGun(int _idType) // gắn ở button từ 0 - 2 0= single 1= auto. 2  = bủrt  
  {
    GameSystem.Instance._IdFireModes = _idType;
  }



}
