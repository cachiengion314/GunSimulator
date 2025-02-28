using System;
using HoangNam;
using UnityEngine;

public class UiShop : BaseUIRoot
{

  public void BuyRemoveAds()
  {
    SoundSystem.Instance.PlayButtonSfx();
    IAPSystem.Instance.PurchaseProduct(0, (complete, productId) =>
    {
      if (complete)
      {
        GameSystem.Instance.IsRemoveAds = true;
        UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("buy success");
      }
      else
      {
        UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("purchase failed");
      }
    });
  }

  public void BuyStarterBundle()
  {
    SoundSystem.Instance.PlayButtonSfx();
    IAPSystem.Instance.PurchaseProduct(1, (complete, productId) =>
    {
      if (complete)
      {
        GameSystem.Instance.CurrentCoin += 400;
        GameSystem.Instance.CurrentBooster1 += 3;
        GameSystem.Instance.CurrentBooster2 += 3;
        GameSystem.Instance.CurrentBooster3 += 3;
        UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("buy success");
      }
      else
      {
        UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("purchase failed");
      }
    });
  }

  public virtual void BuyMoney(int Key, int amount)
  {
    SoundSystem.Instance.PlayButtonSfx();
    IAPSystem.Instance.PurchaseProduct(Key, (complete, productId) =>
    {
      if (complete)
      {
        GameSystem.Instance.CurrentCoin += amount;
        UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("buy success");
      }
      else
      {
        UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("purchase failed");
      }
    });
  }
  public virtual void BuyMini()
  {
    BuyMoney(2, 400);
  }

  public virtual void BuySmall()
  {
    BuyMoney(3, 1700);
  }

  public virtual void BuyMedium()
  {
    BuyMoney(4, 4800);
  }

  public virtual void BuyBig()
  {
    BuyMoney(5, 10000);
  }

  public virtual void BuySuper()
  {
    BuyMoney(6, 20000);
  }

  public virtual void BuyMega()
  {
    BuyMoney(7, 40000);
  }

  public virtual void RewardAds(int money)
  {
    SoundSystem.Instance.PlayButtonSfx();
    string STRToday = DateTime.Now.ToString("yyyyMMdd");
    LoginData dayData = LoginManager.Instance.GetDayData(STRToday);
    if (dayData != null && dayData.amountRewardAds > 0)
    {
#if UNITY_EDITOR
      GameSystem.Instance.CurrentCoin += money;
      dayData.amountRewardAds--;
      LoginManager.Instance.ChangeDayData(dayData);
      GameSystem.Instance.onChangeNotiFreeCoin?.Invoke();
#else
            LevelPlayAds.Instance.ShowRewardedAd(() =>
            {
                GameSystem.Instance.CurrentCoin += money;
                dayData.amountRewardAds--;
                LoginManager.Instance.ChangeDayData(dayData);
            }, "RewardAds",
            () =>
            {
                UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("no ads available");
            });
#endif
    }
    else
    {
      UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("out of turns");
      Debug.Log("het luot");
    }
  }
}
