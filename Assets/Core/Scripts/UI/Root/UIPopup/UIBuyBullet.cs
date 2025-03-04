using UnityEngine;

public class UIBuyBullet : BaseUIRoot
{

    public void BtnADSBuy30Bullet()
    {
        switch (GameSystem.Instance.IdShopMode)
        {
            case 0:
                BuyBulletGun();
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                BuyBulletLight();
                break;
        }

    }
    void BuyBulletGun()
    {
        // LevelPlayAds.Instance.ShowRewardedAd(() =>
        // {
        DataGunManager.Instance.ResetDataTest(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdWeaponsPick);
        var Test = DataGunManager.Instance.GetGunDataClassPick();
        int valueTest = Test._currentValue;
        Debug.Log("Test.valueTest" + valueTest);

        var currentGun = ItemSystem.Instance.GetCurrentGun();
        int startcurrent = currentGun.GetComponent<GunControl>().InitAmmo;
        Debug.Log("Test.startcurrent" + startcurrent);
        currentGun.GetComponent<GunControl>().SetCurrentAmmo(startcurrent);
        UiIngameRoot.Instance.UpdateCurrentBulletBuyFull();
        this.Exit();
        SoundSystem.Instance.GunReloadSfx();

        // }, " BuyBullet");
    }
    void BuyBulletLight()
    {
        ItemSystem.Instance.CurrentLightsaber.Recharge();
        var Test = ItemSystem.Instance.CurrentLightsaber;// Gameobjcet kiếm Tắt
        Test.gameObject.transform.GetChild(1).gameObject.SetActive(true);// Gameobjcet kiếm Tắt
        this.Exit();
    }
    protected override void OnHideCompleted()
    {
        base.OnHideCompleted();
        GameSystem.Instance.SetGameState(GameState.Gameplay);
    }
}
