using MoreMountains.Feedbacks;
using Unity.Mathematics;
using UnityEngine;

public class GunControl : MonoBehaviour,
  ISpriteRenderer,
  IGunFire,
  IFeedbackControl,
  IDoTweenControl
{
  [Header("Dependencies")]
  [SerializeField] SpriteRenderer[] bodyRenderers;
  public SpriteRenderer BodyRenderer
  {
    get
    {
      _bodyRendererIndex = math.max(0, _bodyRendererIndex);
      _bodyRendererIndex = math.min(bodyRenderers.Length, _bodyRendererIndex);
      return bodyRenderers[_bodyRendererIndex];
    }
  }

  [Header("Datas")]
  Quaternion _initRotation;
  int _bodyRendererIndex;
  bool _isTweening;
  int _initAmmo;
  int _currentAmmo;
  [SerializeField] int gunType;
  public int GunType { get { return gunType; } }
  [Header("Recoil position")]
  [SerializeField] float singleModeRecoilPosition;
  public float SingleModeRecoilPosition { get { return singleModeRecoilPosition; } }
  [SerializeField] float autoModeRecoilPosition;
  public float AutoModeRecoilPosition { get { return autoModeRecoilPosition; } }
  [SerializeField] float burstModeRecoilPosition;
  public float BurstModeRecoilPosition { get { return burstModeRecoilPosition; } }
  [Header("Recoil angles")]
  [SerializeField] float singleModeRecoilAngles;
  public float SingleModeRecoilAngles { get { return singleModeRecoilAngles; } }
  [SerializeField] float autoModeRecoilAngles;
  public float AutoModeRecoilAngles { get { return autoModeRecoilAngles; } }
  [SerializeField] float burstModeRecoilAngles;
  public float BurstModeRecoilAngles { get { return burstModeRecoilAngles; } }
  [Header("Fire rate")]
  [SerializeField] float singleModeFireRate;
  public float SingleModeFireRate { get { return singleModeFireRate; } }
  [SerializeField] float autoModeFireRate;
  public float AutoModeFireRate { get { return autoModeFireRate; } }
  [SerializeField] float burstModeFireRate;
  public float BurstModeFireRate { get { return burstModeFireRate; } }
  [Header("ReduceAmmoPerShot")]
  [SerializeField] int reduceAmmoPerShot;
  public int ReduceAmmoPerShot { get { return reduceAmmoPerShot; } }
  [Header("Mode")]
  [SerializeField] bool haveAutoMode;
  public bool HaveAutoMode { get { return haveAutoMode; } }
  [SerializeField] bool haveBurstMode;
  public bool HaveBurstMode { get { return haveBurstMode; } }

  public void ChangeBodyRendererBaseOn(int idx)
  {
    _bodyRendererIndex = idx;
    for (int i = 0; i < bodyRenderers.Length; ++i)
    {
      bodyRenderers[i].gameObject.SetActive(false);
      if (i == idx) bodyRenderers[i].gameObject.SetActive(true);
    }
    _initRotation = transform.rotation;
  }

  /// <summary>
  /// lượng đạn ban đầu
  /// </summary>
  /// <param name="ammo"></param>
  public void SetInitAmmo(int ammo)
  {
    _initAmmo = ammo;
    SetCurrentAmmo(ammo);
  }

  /// <summary>
  /// hàm set lượng đạn hiện tại
  /// </summary>
  /// <param name="ammo"></param>
  public void SetCurrentAmmo(int ammo)
  {
    _currentAmmo = ammo;
  }

  /// <summary>
  /// lấy số lượng đạn hiện có
  /// </summary>
  /// <param name="ammo"></param>
  public int GetCurrentAmmo()
  {
    return _currentAmmo;
  }

  public float3 GetMuzzlePosition()
  {
    return BodyRenderer.transform.GetChild(0).position;
  }

  public int GetSortingOrder()
  {
    throw new System.NotImplementedException();
  }

  public void ResetSortingOrder()
  {
    throw new System.NotImplementedException();
  }

  public void SetInitSortingOrder(int sortingOrder)
  {
    throw new System.NotImplementedException();
  }

  public void SetSortingOrder(int sortingOrder)
  {
    throw new System.NotImplementedException();
  }

  public void InjectChannel(int channelId)
  {
    BodyRenderer.GetComponent<MMPositionShaker>().Channel = channelId;
  }

  public bool IsTweening()
  {
    return _isTweening;
  }

  public void ChangeTweeningTo(bool onOffValue)
  {
    _isTweening = onOffValue;
  }

  public float3 GetBulletBurstUpPosition()
  {
    return BodyRenderer.transform.GetChild(1).position;
  }

  public Quaternion GetInitRotation()
  {
    return _initRotation;
  }
}
