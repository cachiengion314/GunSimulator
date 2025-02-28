using HoangNam;
using TMPro;
using UnityEngine;

public class UiBuyHearts : BaseUIRoot
{
  [SerializeField] TextMeshProUGUI timeTxt;
  [SerializeField] TextMeshProUGUI numberCurrentHeartsTxt;
  int coin = 20;

  private void FixedUpdate()
  {
    // timeTxt.text = HeartSystem.Instance.countdownSystem.TimeStr;
    numberCurrentHeartsTxt.text = GameSystem.Instance.CurrentHeart.ToString();
  }

  public void BuyHearts()
  {
    if (!GameSystem.Instance.IsHeartFull)
    {
      GameSystem.Instance.CurrentHeart++;
      GameSystem.Instance.CurrentCoin -= 20;
    }
    SoundSystem.Instance.PlayButtonSfx();
    UIManager.Instance.Hide();
  }

  public void FreeHeart()
  {
    LevelPlayAds.Instance.ShowRewardedAd(
      () => BuyHearts()
    , "FreeHeart", () =>
    {
      UIManager.Instance.GetUI<UiNotify>(KeyStr.NAME_NOTIFY_MODAL).ShowNotify("no ads available");
    });
  }
}
