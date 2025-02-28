using UnityEngine;

public partial class GameSystem : MonoBehaviour
{
  [Header("---GunData---")]
  [SerializeField] public int _idTypePick;
  [SerializeField] public int _idGunPick;
  [SerializeField] public int _IdFireModes = 0; // id 0 = single , 1 = auto; 2  = Burst;
  [SerializeField] public int _idShopMode = 0; // id 0 = GunSound , 1 = Explosion; 2  = TaserGun 3 = LightSaber;

  void Start()
  {
    var gunData = DataGunManager.Instance.GetGunDataClassPick();

    if (gunData == null)
    {
      var Gunpick = DataGunManager.Instance.GetGunDataClass(0, 0);
      Gunpick._isOwned = true;
      Gunpick._isPick = true;

      _idTypePick = gunData._typeGun;
      _idGunPick = gunData._idGun;

      DataGunManager.Instance.SaveDataJsonTask();
      return;
    }
    _idTypePick = gunData._typeGun;
    _idGunPick = gunData._idGun;
  }
}