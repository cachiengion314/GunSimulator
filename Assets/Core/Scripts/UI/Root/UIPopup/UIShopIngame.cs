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
  [Header("---ItemTLightSaber---")]
  [SerializeField] GameObject[] _arrayPrefabItemLightSaber;

  void Awake()
  {

  }
  void OnEnable()
  {
    SetupItemShop();
    // SetDefault();
    // SetupAnimShop();
  }
  void SetupItemShop()
  {
    // for (int idTypeGun = 0; idTypeGun < _arrayParentItem.Length; idTypeGun++)
    // {
    //   CreatItemShop(idTypeGun);
    // }
    CreatItemShopNew();
    SetupShowShopType();
  }
  void CreatItemShop(int IdTypeGun) // chuyền IdTypeGun  ,chuyền _prefabItem , chuyền parent
  {
    if (_arrayParentItem[IdTypeGun] == null || _arrayPrefabItemGunSound[IdTypeGun] == null)
    {
      Debug.LogError($"❌ Lỗi: Parent hoặc Prefab cho loại {IdTypeGun} chưa được gán!");
      return;
    }

    foreach (var Item in DataGunManager.Instance._GunDataJsonBase.ListGunsJson)
    {
      if (Item._typeGun == IdTypeGun)
      {
        int idGun = Item._idGun; // lấy ra IdGun
        GameObject ItemPistol = Instantiate(_arrayPrefabItemGunSound[IdTypeGun], _arrayParentItem[IdTypeGun].transform); // khởi tạo Item
        Image _ImageIcon = ItemPistol.transform.GetChild(1).GetComponent<Image>();
        TMP_Text _TextInfo = ItemPistol.transform.GetChild(2).GetComponent<TMP_Text>();
        Button ButtonBuy = ItemPistol.transform.GetChild(3).GetComponent<Button>();
        Button ButtonUse = ItemPistol.transform.GetChild(4).GetComponent<Button>();
        GameObject ObjPick = ItemPistol.transform.GetChild(5).gameObject;
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
        switch (IdTypeGun)
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
  }
  void CreatItemShopNew() // chuyền IdTypeGun  ,chuyền _prefabItem , chuyền parent
  {
    CreatShopGunSound();
    CreatShopExplosion();
  }
  void CreatShopGunSound()
  {
    foreach (var Item in DataGunManager.Instance._GunDataJsonBase.ListGunsJson)
    {
      int idGun = Item._idGun; // lấy ra IdGun
      int IdTypeGun = Item._typeGun;
      GameObject ItemPistol = Instantiate(_arrayPrefabItemGunSound[IdTypeGun], _arrayParentItem[0].transform); // khởi tạo Item
      Image _ImageIcon = ItemPistol.transform.GetChild(1).GetComponent<Image>();
      TMP_Text _TextInfo = ItemPistol.transform.GetChild(2).GetComponent<TMP_Text>();
      Button ButtonBuy = ItemPistol.transform.GetChild(3).GetComponent<Button>();
      Button ButtonUse = ItemPistol.transform.GetChild(4).GetComponent<Button>();
      GameObject ObjPick = ItemPistol.transform.GetChild(5).gameObject;
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

  void CreatShopExplosion()
  {
    foreach (var Item in DataExplosionManager.Instance._ExplosionDataJsonBase.ListExplosionsJson)
    {
      int IdTypeExplosion = Item._typExplosion;
      int idExplosoin = Item._idExplosion; // lấy ra IdGun
      GameObject ItemPistol = Instantiate(_arrayPrefabItemGunSound[IdTypeExplosion], _arrayParentItem[1].transform); // khởi tạo Item
      Image _ImageIcon = ItemPistol.transform.GetChild(1).GetComponent<Image>();
      TMP_Text _TextInfo = ItemPistol.transform.GetChild(2).GetComponent<TMP_Text>();
      Button ButtonBuy = ItemPistol.transform.GetChild(3).GetComponent<Button>();
      Button ButtonUse = ItemPistol.transform.GetChild(4).GetComponent<Button>();
      GameObject ObjPick = ItemPistol.transform.GetChild(5).gameObject;
      ObjPick.gameObject.SetActive(false);

      if (Item._isOwned == true) // đã sở hữu
      {
        ButtonUse.gameObject.SetActive(true);
        ButtonBuy.gameObject.SetActive(false);
        ButtonUse.onClick.RemoveAllListeners();
        ButtonUse.onClick.AddListener(() =>
        {
          BTNClickUse(IdTypeExplosion, idExplosoin); //
        });
      }
      else // đã chưa sổ hữu
      {
        ButtonUse.gameObject.SetActive(false);
        ButtonBuy.gameObject.SetActive(true);
        ButtonBuy.onClick.RemoveAllListeners();
        ButtonBuy.onClick.AddListener(() =>
        {
          BTNClickBuy(ButtonBuy, ButtonUse, IdTypeExplosion, idExplosoin);

        });
      }
      switch (IdTypeExplosion)
      {
        case 0:
          _ImageIcon.sprite = RenderSystem.Instance.GetSpriteIconPistol(idExplosoin);
          break;
        case 1:
          _ImageIcon.sprite = RenderSystem.Instance.GetSpriteIconSMGgun(idExplosoin);
          break;
        case 2:
          _ImageIcon.sprite = RenderSystem.Instance.GetSpriteIconShotGun(idExplosoin);
          break;
        case 3:
          _ImageIcon.sprite = RenderSystem.Instance.GetSpriteIconRiffle(idExplosoin);
          break;
        case 4:
          _ImageIcon.sprite = RenderSystem.Instance.GetSpriteIconSniper(idExplosoin);
          break;
      }
    }
  }
  void BTNClickUse(int _idTypeGun, int _IdGun)
  {
    var Gunpick = DataGunManager.Instance.SelectGun(_idTypeGun, _IdGun);
    GameSystem.Instance._idTypePick = Gunpick._typeGun;
    GameSystem.Instance._idGunPick = Gunpick._idGun;
    PlayGame();
    // _objPick.SetActive(true);
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
    GameSystem.Instance._idTypePick = Gunpick._typeGun;
    GameSystem.Instance._idGunPick = Gunpick._idGun;
    PlayGame();

    // });

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
    // _contentsShop.anchoredPosition = new Vector2(SavePositonContentShop.x, SavePositonContentShop.y - _startvalue);
    if (_currentScrollParent != null)
    {
      _currentScrollParent.anchoredPosition = new Vector2(SavePositonContentShop.x, SavePositonContentShop.y - _startvalue);
    }
    _btnExit.interactable = false;
  }
  void StartAnimShop()
  {
    _bgShop.DOKill();
    // _contentsShop.DOKill();

    // float timebg = _timeAnimeShop * 0.5f;
    // float timeContent = _timeAnimeShop * 0.35f;
    // float delayContent = _timeAnimeShop * 0.15f;
    // Sequence sequence = DOTween.Sequence();
    // sequence.Join(_bgShop.DOAnchorPos(SavePositonBGShop, timebg)
    //                   .SetEase(Ease.OutBack));// Hiệu ứng nảy

    // sequence.Insert(delayContent, _contentsShop.DOAnchorPos(SavePositonContentShop, timeContent)
    //               .SetEase(Ease.OutBack))
    //               .OnComplete(() =>
    //               {
    //                 _btnExit.interactable = true;
    //               });
    // ; // Hiệu ứng mượt

    if (_currentScrollParent != null)
    {
      _currentScrollParent.DOKill();
    }

    float timebg = _timeAnimeShop * 0.5f;
    float timeContent = _timeAnimeShop * 0.35f;
    float delayContent = _timeAnimeShop * 0.15f;

    Sequence sequence = DOTween.Sequence();

    sequence.Join(_bgShop.DOAnchorPos(SavePositonBGShop, timebg)
                      .SetEase(Ease.OutBack)); // Hiệu ứng nảy

    if (_currentScrollParent != null)
    {
      sequence.Insert(delayContent, _currentScrollParent.DOAnchorPos(SavePositonContentShop, timeContent)
                    .SetEase(Ease.OutBack))
                    .OnComplete(() =>
                    {
                      _btnExit.interactable = true;
                    });
    }
  }

  public void PlayAnimShop()
  {
    SetupAnimShop();
    StartAnimShop();
  }
  protected override void OnShowCompleted()
  {
    base.OnShowCompleted(); // Gọi base để đảm bảo logic cơ bản vẫn chạy
    // PlayAnimShop(); // Thực hiện anim win sau khi OnShow hoàn tất

  }
  public void BTNExitShop()
  {
    this.Exit();
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
