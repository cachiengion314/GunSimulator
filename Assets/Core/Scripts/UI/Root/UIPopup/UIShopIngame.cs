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
  [SerializeField] GameObject _arrayPrefabItemExplosion;
  [Header("---ItemTaserGun---")]
  [SerializeField] GameObject[] _arrayPrefabItemTaserGun;
  [Header("---ItemTLightSaber---")] //kích cỡ kiếm , dạng kiếm dài ngắn KHÁC NHAU thì dùng cái này 
  [SerializeField] GameObject[] _arrayPrefabItemLightSaber;
  [Header("---ItemLigh---")]
  [SerializeField] GameObject ItemLightSaber;//kích cỡ kiếm , dạng kiếm và độ dài GIỐNG NHAU thì dùng cái này 
  [SerializeField] GameObject ItemGunTaser;//kích cỡ kiếm , dạng kiếm và độ dài GIỐNG NHAU thì dùng cái này 

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
    CreatShopExploSion();
    CreatShopLightSaber();
    CreatShopGunTaser();
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
          BTNClickUse(GameSystem.Instance.IdShopMode, IdTypeGun, idGun);
        });
      }
      else // đã chưa sổ hữu
      {
        ButtonUse.gameObject.SetActive(false);
        ButtonBuy.gameObject.SetActive(true);
        ButtonBuy.onClick.RemoveAllListeners();
        ButtonBuy.onClick.AddListener(() =>
        {
          BTNClickBuy(ButtonBuy, ButtonUse, GameSystem.Instance.IdShopMode, IdTypeGun, idGun);

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
  void CreatShopExploSion()
  {
    foreach (var Item in DataExplosionManager.Instance._ExplosionDataJsonBase.ListExplosionsJson)
    {
      int IdTypeExplosion = Item._idTypeExplosion;
      int idExplosion = Item._idExplosion; // lấy ra IdGun
      GameObject ItemGunShop = Instantiate(_arrayPrefabItemExplosion, _arrayParentItem[1].transform); // khởi tạo Item
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
          BTNClickUse(GameSystem.Instance.IdShopMode, IdTypeExplosion, idExplosion);
        });
      }
      else // đã chưa sổ hữu
      {
        ButtonUse.gameObject.SetActive(false);
        ButtonBuy.gameObject.SetActive(true);
        ButtonBuy.onClick.RemoveAllListeners();
        ButtonBuy.onClick.AddListener(() =>
        {
          BTNClickBuy(ButtonBuy, ButtonUse, GameSystem.Instance.IdShopMode, IdTypeExplosion, idExplosion);

        });
      }
      switch (IdTypeExplosion) // có 2 dạng bomb . 0 = Granade .1 = Bomb .
      {
        case 0:
          _ImageIcon.sprite = RenderSystem.Instance.GetSpriteIconGranade(idExplosion);
          break;
        case 1:
          _ImageIcon.sprite = RenderSystem.Instance.GetSpriteIconBomb(idExplosion);
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
      int index = i;

      bool isVideoAds = PlayerPrefs.GetInt("Lightsaber" + index, data.isVideoAds) == 1;

      ObjPick.SetActive(false);
      if (isVideoAds)
      {
        ButtonBuy.gameObject.SetActive(true);
        ButtonUse.gameObject.SetActive(false);
      }
      else
      {
        ButtonBuy.gameObject.SetActive(false);
        ButtonUse.gameObject.SetActive(true);
      }
      ButtonUse.onClick.AddListener(() =>
      {
        GameSystem.Instance.CurrentLightsaberIndex = index;
        PlayGame();
      });

      ButtonBuy.onClick.AddListener(() =>
      {
        PlayerPrefs.SetInt("Lightsaber" + index, 0);
        GameSystem.Instance.CurrentLightsaberIndex = index;
        PlayGame();
      });

    }
  }
  void CreatShopGunTaser()
  {
    TaserData[] TaserDatas = RenderSystem.Instance.GetTaserDatas();

    for (int i = 0; i < TaserDatas.Length; i++)
    {
      GameObject ItemTaserShop = Instantiate(ItemGunTaser, _arrayParentItem[2].transform); // khởi tạo Item
      Image _ImageIcon = ItemTaserShop.transform.GetChild(1).GetComponent<Image>();
      TMP_Text _TextInfo = ItemTaserShop.transform.GetChild(2).GetComponent<TMP_Text>();
      Button ButtonBuy = ItemTaserShop.transform.GetChild(3).GetComponent<Button>();
      Button ButtonUse = ItemTaserShop.transform.GetChild(4).GetComponent<Button>();
      GameObject ObjPick = ItemTaserShop.transform.GetChild(5).gameObject;

      TaserData data = TaserDatas[i];

      _ImageIcon.sprite = data.BodySprite;
      _TextInfo.text = data.Name;
      int index = i;

      bool isVideoAds = PlayerPrefs.GetInt("Lightsaber" + index, data.isVideoAds) == 1;

      ObjPick.SetActive(false);
      if (isVideoAds)
      {
        ButtonBuy.gameObject.SetActive(true);
        ButtonUse.gameObject.SetActive(false);
      }
      else
      {
        ButtonBuy.gameObject.SetActive(false);
        ButtonUse.gameObject.SetActive(true);
      }
      ButtonUse.onClick.AddListener(() =>
      {
        GameSystem.Instance.CurrentTaserIndex = index;
        PlayGame();
      });

      ButtonBuy.onClick.AddListener(() =>
      {
        PlayerPrefs.SetInt("Tasber" + index, 0);
        GameSystem.Instance.CurrentTaserIndex = index;
        PlayGame();
      });

    }
  }
  void BTNClickUse(int idShop, int _idTypeGun, int _IdGun)
  {
    switch (idShop)
    {
      case 0:// shop gun
        var Gunpick = DataGunManager.Instance.SelectGun(_idTypeGun, _IdGun);
        GameSystem.Instance.IdTypePick = Gunpick._typeGun;
        GameSystem.Instance.IdWeaponsPick = Gunpick._idGun;
        PlayGame();
        break;

      case 1:// shop =Explosion
        var Explosionpick = DataExplosionManager.Instance.SelectExplosion(_idTypeGun, _IdGun);
        GameSystem.Instance.IdTypePick = Explosionpick._idTypeExplosion;
        GameSystem.Instance.IdWeaponsPick = Explosionpick._idExplosion;
        PlayGame();
        break;
    }
  }
  void BTNClickBuy(Button _ButtonBuy, Button _ButtonSelect, int IdShopMode, int _idTypeGun, int _IdGun) // button buy
  {
    // DataGunManager.Instance.BuyGun(_idTypeGun, _IdGun);

    // _ButtonSelect.gameObject.SetActive(true);
    // _ButtonBuy.gameObject.SetActive(false);
    // // _ButtonSelect.onClick.RemoveAllListeners();
    // // _ButtonSelect.onClick.AddListener(() =>
    // // {
    // //   BTNClickUse(_idTypeGun, _IdGun, _objPick);
    // // }); 
    // // xem quảng cáo xong thì vào Gameplay
    // // LevelPlayAds.Instance.ShowRewardedAd(() =>
    // // {
    // //   var Gunpick = DataGunManager.Instance.SelectGun(_idTypeGun, _IdGun);
    // //   GameSystem.Instance._idTypePick = Gunpick._typeGun;
    // //   GameSystem.Instance._idGunPick = Gunpick._idGun;
    // //   PlayGame();
    // // }, "BuyGun",
    // // () =>
    // // {
    // var Gunpick = DataGunManager.Instance.SelectGun(_idTypeGun, _IdGun);
    // GameSystem.Instance.IdTypePick = Gunpick._typeGun;
    // GameSystem.Instance.IdWeaponsPick = Gunpick._idGun;
    // PlayGame();
    switch (IdShopMode)
    {
      case 0:
        DataGunManager.Instance.BuyGun(_idTypeGun, _IdGun);

        _ButtonSelect.gameObject.SetActive(true);
        _ButtonBuy.gameObject.SetActive(false);
        var Gunpick = DataGunManager.Instance.SelectGun(_idTypeGun, _IdGun);
        GameSystem.Instance.IdTypePick = Gunpick._typeGun;
        GameSystem.Instance.IdWeaponsPick = Gunpick._idGun;
        PlayGame();
        break;
      case 1:
        DataExplosionManager.Instance.BuyExplosion(_idTypeGun, _IdGun);

        _ButtonSelect.gameObject.SetActive(true);
        _ButtonBuy.gameObject.SetActive(false);
        var Explosion = DataExplosionManager.Instance.SelectExplosion(_idTypeGun, _IdGun);
        GameSystem.Instance.IdTypePick = Explosion._idTypeExplosion;
        GameSystem.Instance.IdWeaponsPick = Explosion._idExplosion;
        PlayGame();
        break;

    }
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
