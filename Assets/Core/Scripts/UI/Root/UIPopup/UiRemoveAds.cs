using HoangNam;

public class UiRemoveAds : BaseUIRoot
{
  public void BuyRemoveAds()
  {
    SoundSystem.Instance.PlayButtonSfx();
    IAPSystem.Instance.PurchaseProduct(0, (complete, productId) =>
    {
      if (complete)
      {
        GameSystem.Instance.IsRemoveAds = true;
        UIManager.Instance.Hide();
        UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("buy success");
      }
      else
      {
        UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("purchase failed");
      }
    });
  }
}
