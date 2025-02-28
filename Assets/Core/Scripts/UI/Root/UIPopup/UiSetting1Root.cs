using DG.Tweening;
using HoangNam;
using UnityEngine;

public class UiSetting1Root : BaseUIRoot
{
  [Header("Button")]

  [SerializeField] GameObject _stateMusicOn;
  [SerializeField] GameObject _stateMusicOff;
  [SerializeField] GameObject _stateSoundOn;
  [SerializeField] GameObject _stateSoundOff;
  [SerializeField] GameObject _stateVibrateOn;
  [SerializeField] GameObject _stateVibrateOff;

  void Awake()
  {
    // gameobjLock.SetActive(false);
    if (GameSystem.Instance.IsMusicOn == true)
    {
      IsMusicState(true);
    }
    else
    {
      IsMusicState(false);
    }

    if (GameSystem.Instance.IsSoundOn == true)
    {
      IsSoundState(true);
    }
    else
    {
      IsSoundState(false);
    }

    if (GameSystem.Instance.IsHapticOn == true)
    {

      IsVibrateState(true);
    }
    else
    {
      IsVibrateState(false);
    }
  }
  void IsMusicState(bool _State)
  {
    _stateMusicOn.SetActive(_State);
    _stateMusicOff.SetActive(!_State);
  }

  void IsSoundState(bool _State)
  {
    _stateSoundOn.SetActive(_State);
    _stateSoundOff.SetActive(!_State);
  }
  void IsVibrateState(bool _State)
  {
    _stateVibrateOn.SetActive(_State);
    _stateVibrateOff.SetActive(!_State);
  }

  public void setOnOffBtn(int id)//gan vao buton trong popupsetting
  {
    SoundSystem.Instance.PlayButtonSfx();
    switch (id)
    {
      case 0:
        if (_stateMusicOn.activeSelf == true)
        {
          IsMusicState(false);
          GameSystem.Instance.IsMusicOn = false;
        }
        else
        {
          IsMusicState(true);
          GameSystem.Instance.IsMusicOn = true;
        }
        break;
      case 1:
        if (_stateSoundOn.activeSelf == true)
        {
          IsSoundState(false);
          GameSystem.Instance.IsSoundOn = false;

        }
        else
        {
          IsSoundState(true);
          GameSystem.Instance.IsSoundOn = true;
        }

        break;
      case 2:
        if (_stateVibrateOn.activeSelf == true)
        {
          IsVibrateState(false);
          GameSystem.Instance.IsHapticOn = false;
        }
        else
        {
          IsVibrateState(true);
          GameSystem.Instance.IsHapticOn = true;
        }

        break;
    }

  }

  public void BtnBackHome()
  {
    DOTween.KillAll();
    SoundSystem.Instance.PlayButtonSfx();
    // UIManager.Instance.Show(KeyStr.NAME_QUIT_MODAL);
    GameSystem.Instance.LoadSceneByName(KeyStr.NAME_SCENE_LOBBY);
    //Load thang sang lobby
  }

  public void Restore()
  {
    SoundSystem.Instance.PlayButtonSfx();
    IAPSystem.Instance.RestorePurchases((succes, id) =>
    {
      if (succes)
      {
        if (id == IAPSystem.Instance.products[0].productId)
        {
          GameSystem.Instance.IsRemoveAds = true;
        }
        UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("restore success");
      }
      else
      {
        UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("restore failure");
      }
    });
  }

  static int count = 0;
  public void ShowHack()
  {
    if (count > 20)
    {

      UIManager.Instance.Show("HackModal(Clone)");
    }
    else
    {

      count++;
    }
  }

}


