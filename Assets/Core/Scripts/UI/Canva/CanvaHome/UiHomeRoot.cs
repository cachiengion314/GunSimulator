using DG.Tweening;
using HoangNam;
using UnityEngine;

public class UiHomeRoot : MonoBehaviour
{
  void Start()
  {

  }
  public void BtnPlayGame()
  {
    DOTween.KillAll();
    // SoundSystem.Instance.PlayButtonSfx();
    GameSystem.Instance.PlayGame();
  }


  public void BtnDailySign()
  {
    // SoundSystem.Instance.PlayButtonSfx();
    UIManager.Instance.Show(KeyStr.NAME_DailySignIn_MODAL);
  }

  public void BtnDailyTask()
  {
    // SoundSystem.Instance.PlayButtonSfx();
    UIManager.Instance.Show(KeyStr.Name_DailyTask_MODAL);
  }
  public void ShowShopGunSound()
  {
    // SoundSystem.Instance.PlayButtonSfx();
    UIManager.Instance.Show(KeyStr.NAME_SHOP_MODAL);
    GameSystem.Instance.IdShopMode = 0;
    UIManager.Instance.GetUI<UIShopIngame>(KeyStr.NAME_SHOP_MODAL).BtnShowShopType(0);
  }
  public void ShowShopExplosion()
  {
    // SoundSystem.Instance.PlayButtonSfx();
    UIManager.Instance.Show(KeyStr.NAME_SHOP_MODAL);
    GameSystem.Instance.IdShopMode = 1;
    UIManager.Instance.GetUI<UIShopIngame>(KeyStr.NAME_SHOP_MODAL).BtnShowShopType(1);
  }
  public void ShowShopTaserGun()
  {
    // SoundSystem.Instance.PlayButtonSfx();
    UIManager.Instance.Show(KeyStr.NAME_SHOP_MODAL);
    GameSystem.Instance.IdShopMode = 2;
    UIManager.Instance.GetUI<UIShopIngame>(KeyStr.NAME_SHOP_MODAL).BtnShowShopType(2);
  }
  public void ShowShopLightSaber()
  {
    // SoundSystem.Instance.PlayButtonSfx();
    UIManager.Instance.Show(KeyStr.NAME_SHOP_MODAL);
    GameSystem.Instance.IdShopMode = 3;
    UIManager.Instance.GetUI<UIShopIngame>(KeyStr.NAME_SHOP_MODAL).BtnShowShopType(3);
  }

}
