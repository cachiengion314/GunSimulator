using HoangNam;
using UnityEngine;

public class UILose1LevelRoot : BaseUIRoot
{
  [SerializeField] int coin = 200;

  private void Start()
  {
    InitUIQuests();
  }

  private void InitUIQuests()
  {

  }

  private void InitUIQuest()
  {



  }

  public void BTNExit()
  {
    SoundSystem.Instance.PlayLevelFailSfx();
    UIManager.Instance.Show(KeyStr.NAME_LOSE_2_LEVEL_MODAL);
  }

  public void BTNReviveByCoin()
  {
    if (GameSystem.Instance.CurrentCoin < coin)
    {
      UIManager.Instance.Show(KeyStr.NAME_SHOP_MODAL);
      return;
    }
    GameSystem.Instance.CurrentCoin -= coin;
    UIManager.Instance.Hide();

    Revive();
  }

  public void BTNReviveByAds()
  {
    SoundSystem.Instance.PlayButtonSfx();
    LevelPlayAds.Instance.ShowRewardedAd(() =>
    {
      UIManager.Instance.Hide();
      Revive();
    }, "GetX2", () =>
    {
      UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("no ads available");
    });

  }
  public void Revive()
  {
    ReviveDailyTask();
  }
  void ReviveDailyTask()
  {
    DailyTaskManager.Instance.UpdateTaskProgress(3, 1);
  }

  // public void BuySpecialOffer01()
  // {
  //   SoundSystem.Instance.PlayButtonSfx();
  //   IAPSystem.Instance.PurchaseProduct(KeyStr.KEY_IAP_SPECIAL_OFFER, (complete, productId) =>
  //   {
  //     if (complete)
  //     {
  //       GameSystem.Instance.CurrentCoin += 2400;
  //       GameSystem.Instance.CurrentBooster1 += 3;
  //       GameSystem.Instance.CurrentBooster2 += 3;
  //       GameSystem.Instance.CurrentBooster3 += 3;
  //       UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("buy success");
  //     }
  //     else
  //     {
  //       UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("purchase failed");
  //     }
  //   });
  // }
}
