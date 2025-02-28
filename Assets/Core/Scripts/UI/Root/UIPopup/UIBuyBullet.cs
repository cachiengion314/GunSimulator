using UnityEngine;

public class UIBuyBullet : BaseUIRoot
{

    public void BtnADSBuy30Bullet()
    {
        // LevelPlayAds.Instance.ShowRewardedAd(() =>
        // {
            DataGunManager.Instance.ResetDataTest(GameSystem.Instance._idGunPick, GameSystem.Instance._idTypePick);
            UiIngameRoot.Instance.UpdateTextCurrentBullet();
            this.Hide();
            GameSystem.Instance.SetGameState(GameState.Gameplay);
        // }, " BuyBullet");
    }
}
