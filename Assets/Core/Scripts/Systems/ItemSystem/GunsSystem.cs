
using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

// GunsSystem
public partial class ItemSystem : MonoBehaviour
{
  [Header("Gun dependencies")]
  [SerializeField] GunControl pistolPref;
  [SerializeField] GunControl SMGPref;
  [SerializeField] GunControl shotgunPref;
  [SerializeField] GunControl assaultRiflePref;
  [SerializeField] GunControl sniperRiflePref;
  [SerializeField] GunControl machineGunPref;
  [SerializeField] Transform gunParent;
  public Transform GunParent { get { return gunParent; } }

  [Header("Datas")]
  float3 defaultLocalScale = new(1, 1, 0);
  public Action<int> OnFire;
  public Action<bool> OnIsAuto;
  public Action OnSingle;
  public Action OnOutOfAmmo;

 void Update()
  {
    if (GunParent.childCount == 0) return;

    if (Input.GetKeyDown(KeyCode.A))
    {
      SingleModeFireInvoke();
    }
    if (Input.GetKey(KeyCode.S))
    {
      AutoModeFireInvoke();
    }
    if (Input.GetKeyDown(KeyCode.D))
    {
      BurstModeFireInvoke();
    }
  }

  public GameObject GetCurrentGun()
  {
    var gun = GunParent.GetChild(0);
    return gun.gameObject;
  }

  public void SpawnAssaultRifle(int _idGun = 0)
  {
    var obj = Instantiate(assaultRiflePref, gunParent);

    obj.GetComponent<GunControl>().ChangeBodyRendererBaseOn(_idGun);

    obj.transform.localScale = defaultLocalScale;
    obj.transform.position = new Vector3(0, 0, 0);
  }

  public void SpawnPistol(int _idGun = 0)
  {
    var obj = Instantiate(pistolPref, gunParent);

    obj.GetComponent<GunControl>().ChangeBodyRendererBaseOn(_idGun);
    if (_idGun == 1)
      obj.GetComponent<GunControl>().ChangeBodyRendererBaseOn(2);

    obj.transform.localScale = defaultLocalScale;
    obj.transform.position = new Vector3(0, 0, 0);
  }

  public void SpawnSMG(int _idGun = 0)
  {
    var obj = Instantiate(SMGPref, gunParent);

    obj.GetComponent<GunControl>().ChangeBodyRendererBaseOn(_idGun);

    obj.transform.localScale = defaultLocalScale;
    obj.transform.position = new Vector3(0, 0, 0);
  }

  public void SpawnShotgun(int _idGun = 0)
  {
    var obj = Instantiate(shotgunPref, gunParent);

    obj.GetComponent<GunControl>().ChangeBodyRendererBaseOn(_idGun);

    obj.transform.localScale = defaultLocalScale;
    obj.transform.position = new Vector3(0, 0, 0);
  }

  public void SpawnSniperRifles(int _idGun = 0)
  {
    var obj = Instantiate(sniperRiflePref, gunParent);

    obj.GetComponent<GunControl>().ChangeBodyRendererBaseOn(_idGun);

    obj.transform.localScale = defaultLocalScale;
    obj.transform.position = new Vector3(0, 0, 0);
  }

  public void SpawnMachineGun(int _idGun = 0)
  {
    var obj = Instantiate(machineGunPref, gunParent);

    obj.GetComponent<GunControl>().ChangeBodyRendererBaseOn(_idGun);

    obj.transform.localScale = defaultLocalScale;
    obj.transform.position = new Vector3(0, 0, 0);
  }

  public void SpawnGunBy(int gunType, int idGun)
  {
    switch (gunType)
    {
      case 0:
        SpawnPistol(idGun);
        break;
      case 1:
        SpawnSMG(idGun);
        break;
      case 2:
        SpawnShotgun(idGun);
        break;
      case 3:
        SpawnAssaultRifle(idGun);
        break;
      case 4:
        SpawnSniperRifles(idGun);
        break;
      case 5:
        SpawnMachineGun(idGun);
        break;
    }

    var _dataGunTarget = DataGunManager.Instance.GetGunDataClassPick();
    int intInitAmmo = _dataGunTarget._currentStartValue;

    int intCurrentAmmo = _dataGunTarget._currentValue;

    var _currentGun = GetCurrentGun();
    _currentGun.GetComponent<GunControl>().SetInitAmmo(intInitAmmo);
    _currentGun.GetComponent<GunControl>().SetCurrentAmmo(intCurrentAmmo);
  }

  bool _isFireInvoke;
  public void BurstModeFireInvoke()
  {
    var currentGun = GetCurrentGun();

    if (currentGun.GetComponent<IDoTweenControl>().IsTweening() || _isFireInvoke)
    {
      print("Please standby");
      return;
    }

    _isFireInvoke = true;

    var seq = DOTween.Sequence();
    var currentAnimDuration = 0f;
    var oneshotDuration = currentGun.GetComponent<GunControl>().BurstModeFireRate / 3f;
    for (int i = 0; i < 3; ++i)
    {
      var currDur = currentAnimDuration + oneshotDuration * i;
      seq.InsertCallback(currDur,
        () =>
        {
          InvokeFireAnim(
            currentGun,
            oneshotDuration,
            currentGun.GetComponent<GunControl>().ReduceAmmoPerShot,
            currentGun.GetComponent<GunControl>().BurstModeRecoilPosition,
            currentGun.GetComponent<GunControl>().BurstModeRecoilAngles
          );
          OnFire?.Invoke(currentGun.GetComponent<GunControl>().ReduceAmmoPerShot);
        });
    }

    seq.InsertCallback(
      currentAnimDuration + currentGun.GetComponent<GunControl>().BurstModeFireRate,
      () =>
      {
        _isFireInvoke = false;
      });
  }

  public void AutoModeFireInvoke()
  {
    var currentGun = GetCurrentGun();

    if (currentGun.GetComponent<IDoTweenControl>().IsTweening() || _isFireInvoke)
    {
      print("Please standby");
      return;
    }
    _isFireInvoke = true;

    InvokeFireAnim(
      currentGun.gameObject,
      currentGun.GetComponent<GunControl>().AutoModeFireRate,
      currentGun.GetComponent<GunControl>().ReduceAmmoPerShot,
      currentGun.GetComponent<GunControl>().AutoModeRecoilPosition,
      currentGun.GetComponent<GunControl>().AutoModeRecoilAngles
    );

    OnFire?.Invoke(currentGun.GetComponent<GunControl>().ReduceAmmoPerShot);
    OnIsAuto?.Invoke(true);

    var seq = DOTween.Sequence();
    var currentAnimDuration = 0f;
    seq.InsertCallback(
      currentAnimDuration + currentGun.GetComponent<GunControl>().AutoModeFireRate,
      () =>
      {

        _isFireInvoke = false;
      });
  }

  public void SingleModeFireInvoke()
  {
    var currentGun = GetCurrentGun();

    if (currentGun.GetComponent<IDoTweenControl>().IsTweening() || _isFireInvoke)
    {
      print("Please standby");
      return;
    }
    if (currentGun.GetComponent<GunControl>().CurrentAmmo <= 0)
    {
      OnOutOfAmmo?.Invoke();
      return;
    }
    _isFireInvoke = true;

    InvokeFireAnim(
      currentGun,
      currentGun.GetComponent<GunControl>().SingleModeFireRate,
      currentGun.GetComponent<GunControl>().ReduceAmmoPerShot,
      currentGun.GetComponent<GunControl>().SingleModeRecoilPosition,
      currentGun.GetComponent<GunControl>().SingleModeRecoilAngles
    );
    OnFire?.Invoke(currentGun.GetComponent<GunControl>().ReduceAmmoPerShot);

    var seq = DOTween.Sequence();
    var currentAnimDuration = 0f;
    seq.InsertCallback(
      currentAnimDuration + currentGun.GetComponent<GunControl>().SingleModeFireRate,
      () =>
      {

        _isFireInvoke = false;
      });
  }

  void InvokeFireAnim(
    GameObject gunObj,
    float fireRate,
    int _reduceAmmoPerShot = 1,
    float _recoilPosition = .5f,
    float _recoilAngles = 40
  )
  {
    if (gunObj == null) return;

    SoundSystem.Instance.PlayGunSoundSfx(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick);

    var muzzlePosition = gunObj.GetComponent<IGunFire>().GetMuzzlePosition();
    EffectSystem.Instance.SpawnMuzzleEfxAt(
      muzzlePosition,
      gunObj.GetComponent<GunControl>().GunType
    );

    var bulletPos = gunObj.GetComponent<IGunFire>().GetBulletBurstUpPosition();
    for (int i = 0; i < _reduceAmmoPerShot; ++i)
      EffectSystem.Instance.SpawnBulletBurstUpEfxAt(bulletPos);

    var _duration = fireRate * .5f;
    if (_duration < .2f) _duration = fireRate;
    if (_duration > .6f) _duration = .6f;

    gunObj.GetComponent<GunControl>().BodyRenderer.transform
      .DOLocalRotate(
          new Vector3(0, 0, -1 * _recoilAngles),
          _duration / 2f
        )
        .SetEase(Ease.Linear)
        .SetLoops(2, LoopType.Yoyo)
        .OnComplete(() =>
          {
            var rot = gunObj.GetComponent<GunControl>().GetInitRotation();
            gunObj.transform.localRotation = rot;
          });

    gunObj.GetComponent<IDoTweenControl>().ChangeTweeningTo(true);
    var backPos = gunObj.transform.position + Vector3.down * _recoilPosition;
    DOTween.To(
      () => { return gunObj.transform.position; },
      (val) => { gunObj.transform.position = val; },
      backPos,
      _duration / 2f
    )
      .SetLoops(2, LoopType.Yoyo)
      .SetEase(Ease.InQuad)
      .OnComplete(() =>
        {
          gunObj.GetComponent<IDoTweenControl>().ChangeTweeningTo(false);

        });

    gunObj.GetComponent<IFeedbackControl>().InjectChannel(gunObj.GetInstanceID());
    FeedbackSystem.Instance.PlayRandomShakesAt(
      gunObj.GetInstanceID(),
      _recoilPosition,
      _duration
    );
  }


}