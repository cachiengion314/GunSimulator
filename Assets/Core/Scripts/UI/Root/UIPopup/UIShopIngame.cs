using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
public class UIShopIngame : UiShop
{
  static public UIShopIngame Instance { get; private set; }
  [Header("---AnimShop---")]
  [SerializeField] RectTransform _bgShop;
  [SerializeField] RectTransform _contentsShop;
  Vector2 SavePositonBGShop;
  Vector2 SavePositonContentShop;
  [SerializeField] float _timeAnimeShop;
  [SerializeField] Button _btnExit;
  private float _startvalue;

  private RectTransform _currentScrollParent; // Lưu ScrollView hiện tại

  [Header("---ShopGun---")]
  [SerializeField] GameObject[] _arrayParentItem; // 0 = GunSound . 1 = Explosion . 2 = TaserGun . 3 = lightSaber
  [SerializeField] GameObject[] _arrayScollParentItem;// 0 = GunSound . 1 = Explosion . 2 = TaserGun . 3 = lightSaber

  [Header("---ItemGun---")]
  [SerializeField] GameObject[] _arrayPrefabItemGunSound;

  [Header("---ItemExplosion---")]
  [SerializeField] GameObject[] _arrayPrefabItemExplosion;
  [Header("---ItemTaserGun---")]
  [SerializeField] GameObject[] _arrayPrefabItemTaserGun;
  [Header("---ItemTLightSaber---")] //kích cỡ kiếm , dạng kiếm dài ngắn KHÁC NHAU thì dùng cái này 
  [SerializeField] GameObject[] _arrayPrefabItemLightSaber;
  [Header("---ItemLighTest---")]
  [SerializeField] GameObject ItemLightSaber;//kích cỡ kiếm , dạng kiếm và độ dài GIỐNG NHAU thì dùng cái này 

  void Awake()
  {
    SetDefault();
  }
  void OnEnable()
  {

    SetupAnimShop();
    SetupItemShop();
  }
  void SetupItemShop()
  {
    ClearContentAllShop();
    CreatItemShopNew();
    SetupShowShopType();
  }
  void CreatItemShopNew() // chuyền IdTypeGun  ,chuyền _prefabItem , chuyền parent
  {
    CreatShopGunSound();
    CreatShopLightSaber();
  }
  void ClearContentAllShop()
  {
    foreach (var parent in _arrayParentItem)
    {
      if (parent != null)
      {
        foreach (Transform child in parent.transform)
        {
          if (child != null)
          {

            Destroy(child.gameObject);
          }
        }
      }
    }

  }
  void CreatShopGunSound()
  {
    foreach (var Item in DataGunManager.Instance._GunDataJsonBase.ListGunsJson)
    {
      int idGun = Item._idGun; // lấy ra IdGun
      int IdTypeGun = Item._typeGun;
      GameObject ItemGunShop = Instantiate(_arrayPrefabItemGunSound[IdTypeGun], _arrayParentItem[0].transform); // khởi tạo Item
      Image _ImageIcon = ItemGunShop.transform.GetChild(1).GetComponent<Image>();
      TMP_Text _TextInfo = ItemGunShop.transform.GetChild(2).GetComponent<TMP_Text>();
      Button ButtonBuy = ItemGunShop.transform.GetChild(3).GetComponent<Button>();
      Button ButtonUse = ItemGunShop.transform.GetChild(4).GetComponent<Button>();
      GameObject ObjPick = ItemGunShop.transform.GetChild(5).gameObject;
      ObjPick.gameObject.SetActive(false);

      if (Item._isOwned == true) // đã sở hữu
      {
        ButtonUse.gameObject.SetActive(true);
        ButtonBuy.gameObject.SetActive(false);
        ButtonUse.onClick.RemoveAllListeners();
        ButtonUse.onClick.AddListener(() =>
        {
          BTNClickUse(IdTypeGun, idGun);
        });
      }
      else // đã chưa sổ hữu
      {
        ButtonUse.gameObject.SetActive(false);
        ButtonBuy.gameObject.SetActive(true);
        ButtonBuy.onClick.RemoveAllListeners();
        ButtonBuy.onClick.AddListener(() =>
        {
          BTNClickBuy(ButtonBuy, ButtonUse, IdTypeGun, idGun);

        });
      }
      switch (IdTypeGun) // có 5 dạng súng . 0 = lục .1 = SMG . 2 = Shotgun .3 = AK . 4 = Sniper
      {
        case 0:
          _ImageIcon.sprite = RenderSystem.Instance.GetSpriteIconPistol(idGun);
          break;
        case 1:
          _ImageIcon.sprite = RenderSystem.Instance.GetSpriteIconSMGgun(idGun);
          break;
        case 2:
          _ImageIcon.sprite = RenderSystem.Instance.GetSpriteIconShotGun(idGun);
          break;
        case 3:
          _ImageIcon.sprite = RenderSystem.Instance.GetSpriteIconRiffle(idGun);
          break;
        case 4:
          _ImageIcon.sprite = RenderSystem.Instance.GetSpriteIconSniper(idGun);
          break;
      }
    }
  }

  void CreatShopLightSaber()
  {
    LightsaberData[] lightsaberDatas = RenderSystem.Instance.GetLightsaberDatas();

    for (int i = 0; i < lightsaberDatas.Length; i++)
    {
      GameObject ItemLightSaberShop = Instantiate(ItemLightSaber, _arrayParentItem[3].transform); // khởi tạo Item
      Image _ImageIcon = ItemLightSaberShop.transform.GetChild(1).GetComponent<Image>();
      TMP_Text _TextInfo = ItemLightSaberShop.transform.GetChild(2).GetComponent<TMP_Text>();
      Button ButtonBuy = ItemLightSaberShop.transform.GetChild(3).GetComponent<Button>();
      Button ButtonUse = ItemLightSaberShop.transform.GetChild(4).GetComponent<Button>();
      GameObject ObjPick = ItemLightSaberShop.transform.GetChild(5).gameObject;

      LightsaberData data = lightsaberDatas[i];
      
      _ImageIcon.sprite = data.SwordHilt;
      _TextInfo.text = data.Name;

      ObjPick.SetActive(false);
      ButtonBuy.gameObject.SetActive(false);
      ButtonUse.gameObject.SetActive(true);
      int index = i;
      ButtonUse.onClick.AddListener(() =>
      {
        GameSystem.Instance.CurrentLightsaberIndex = index;
        PlayGame();
      });


    }
  }
  void BTNClickUse(int _idTypeGun, int _IdGun)
  {
    var Gunpick = DataGunManager.Instance.SelectGun(_idTypeGun, _IdGun);
    GameSystem.Instance.IdTypePick = Gunpick._typeGun;
    GameSystem.Instance.IdGunPick = Gunpick._idGun;
    PlayGame();

  }
  void BTNClickBuy(Button _ButtonBuy, Button _ButtonSelect, int _idTypeGun, int _IdGun) // button buy
  {
    DataGunManager.Instance.BuyGun(_idTypeGun, _IdGun);

    _ButtonSelect.gameObject.SetActive(true);
    _ButtonBuy.gameObject.SetActive(false);
    // _ButtonSelect.onClick.RemoveAllListeners();
    // _ButtonSelect.onClick.AddListener(() =>
    // {
    //   BTNClickUse(_idTypeGun, _IdGun, _objPick);
    // }); 
    // xem quảng cáo xong thì vào Gameplay
    // LevelPlayAds.Instance.ShowRewardedAd(() =>
    // {
    //   var Gunpick = DataGunManager.Instance.SelectGun(_idTypeGun, _IdGun);
    //   GameSystem.Instance._idTypePick = Gunpick._typeGun;
    //   GameSystem.Instance._idGunPick = Gunpick._idGun;
    //   PlayGame();
    // }, "BuyGun",
    // () =>
    // {
    var Gunpick = DataGunManager.Instance.SelectGun(_idTypeGun, _IdGun);
    GameSystem.Instance.IdTypePick = Gunpick._typeGun;
    GameSystem.Instance.IdGunPick = Gunpick._idGun;
    PlayGame();
  }
  void SetDefault()
  {
    _startvalue = Screen.height;
    SavePositonBGShop = _bgShop.anchoredPosition;
    SavePositonContentShop = _contentsShop.anchoredPosition;
    _btnExit.interactable = false;

  }
  void SetupAnimShop()
  {
    _bgShop.anchoredPosition = new Vector2(SavePositonBGShop.x, SavePositonBGShop.y - _startvalue);
    _contentsShop.anchoredPosition = new Vector2(SavePositonContentShop.x, SavePositonContentShop.y - _startvalue);
    if (_currentScrollParent != null)
    {
      _currentScrollParent.anchoredPosition = new Vector2(SavePositonContentShop.x, SavePositonContentShop.y - _startvalue);
    }
    _btnExit.interactable = false;
  }
  void StartAnimShop(int _idShop)
  {
    _bgShop.DOKill();
    _contentsShop.DOKill();

    float timebg = _timeAnimeShop * 0.5f;
    float timeContent = _timeAnimeShop * 0.35f;
    float delayContent = _timeAnimeShop * 0.15f;
    Sequence sequence = DOTween.Sequence();
    sequence.Join(_bgShop.DOAnchorPos(SavePositonBGShop, timebg)
                      .SetEase(Ease.OutBack));// Hiệu ứng nảy

    RectTransform ObjTarget = _arrayScollParentItem[_idShop].gameObject.GetComponent<RectTransform>();
    if (ObjTarget != null)
    {
      ObjTarget.gameObject.transform.DOKill();
    }
    sequence.Insert(delayContent, ObjTarget.DOAnchorPos(SavePositonContentShop, timeContent)
                  .SetEase(Ease.OutBack))
                  .OnComplete(() =>
                  {
                    _btnExit.interactable = true;
                  });
  }

  public void PlayAnimShop()
  {
    SetupAnimShop();
    StartAnimShop(GameSystem.Instance.IdShopMode);
  }
  protected override void OnShowCompleted()
  {
    base.OnShowCompleted(); // Gọi base để đảm bảo logic cơ bản vẫn chạy
    PlayAnimShop(); // Thực hiện anim win sau khi OnShow hoàn tất

  }
  public void BTNExitShop()
  {
    this.Hide();
    // UiIngameRoot.Instance._btnShowShopCoin.interactable = true;
  }
  public void BtnShowShopType(int _IdType) // từ 0 - 4 loại súng gắn vào button để hiện thị shop
  {
    SetupShowShopType();
    ShowShopType(_IdType);
    _currentScrollParent = _arrayScollParentItem[_IdType].GetComponent<RectTransform>();
  }
  void SetupShowShopType()
  {
    for (int idTypeGun = 0; idTypeGun < _arrayScollParentItem.Length; idTypeGun++)
    {
      _arrayScollParentItem[idTypeGun].gameObject.SetActive(false);
    }
  }
  void ShowShopType(int Id)
  {
    _arrayScollParentItem[Id].gameObject.SetActive(true);
  }
  void PlayGame()
  {
    DOTween.KillAll();
    // SoundSystem.Instance.PlayButtonSfx();

    GameSystem.Instance.PlayGame();
  }

}
