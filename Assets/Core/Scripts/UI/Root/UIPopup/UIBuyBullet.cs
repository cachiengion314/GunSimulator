using UnityEngine;

public class UIBuyBullet : BaseUIRoot
{

    public void BtnADSBuy30Bullet()
    {
        // LevelPlayAds.Instance.ShowRewardedAd(() =>
        // {
        DataGunManager.Instance.ResetDataTest(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick);
        var Test = DataGunManager.Instance.GetGunDataClassPick();
        int valueTest = Test._currentValue;
        Debug.Log("Test.valueTest" + valueTest);

        var currentGun = ItemSystem.Instance.GetCurrentGun();
        int startcurrent = currentGun.GetComponent<GunControl>().InitAmmo;
        Debug.Log("Test.startcurrent" + startcurrent);
        currentGun.GetComponent<GunControl>().SetCurrentAmmo(startcurrent);
        UiIngameRoot.Instance.UpdateTextCurrentBullet();
        this.Exit();

        // }, " BuyBullet");
    }
    protected override void OnHideCompleted()
    {
        base.OnHideCompleted();
        GameSystem.Instance.SetGameState(GameState.Gameplay);
    }
}
