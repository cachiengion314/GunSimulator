using UnityEngine;

public partial class GameSystem : MonoBehaviour
{
  [Header("---GunData---")]
  public int IdTypePick; // dạng vũ khí (dùng chung cho shop id 0 , 1 ,2 ) , 3 thì logic của nguyên
  public int IdWeaponsPick; // id vũ khí ()
  public int IdFireModes = 0; // id 0 = single , 1 = auto; 2  = Burst;
  public int IdShopMode = 0; // id 0 = GunSound , 1 = Explosion; 2  = TaserGun 3 = LightSaber;

  void Start()
  {
    var gunData = DataGunManager.Instance.GetGunDataClassPick();

    if (gunData == null)
    {
      var Gunpick = DataGunManager.Instance.GetGunDataClass(0, 0);
      Gunpick._isOwned = true;
      Gunpick._isPick = true;

      IdTypePick = gunData._typeGun;
      IdWeaponsPick = gunData._idGun;

      DataGunManager.Instance.SaveDataJsonTask();
      return;
    }
    IdTypePick = gunData._typeGun;
    IdWeaponsPick = gunData._idGun;
  }
}