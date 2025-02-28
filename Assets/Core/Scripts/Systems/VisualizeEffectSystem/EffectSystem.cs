using System;
using Unity.Mathematics;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
  public static EffectSystem Instance { get; private set; }

  [Header("Prefabs")]
  [SerializeField] CoinsParticle coinPsPrefab;
  [SerializeField] ParticleSystem birthdayConfenttiEfx;
  [SerializeField] ParticleSystem arcaneSparkEfx;
  [SerializeField] Animator fireFrameAnim;
  [SerializeField] ParticleSystem bulletBurstUpEfx;
  [SerializeField] ParticleSystem assaultRifleMuzzleEfx;
  [SerializeField] ParticleSystem machineGunMuzzleEfx;
  [SerializeField] ParticleSystem pistolMuzzleEfx;
  [SerializeField] ParticleSystem shotGunMuzzleEfx;
  [SerializeField] ParticleSystem SMGMuzzleEfx;
  [SerializeField] ParticleSystem sniperRiflesMuzzleEfx;

  void Start()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else Destroy(gameObject);
  }

  public void EmissingCoinsTo(
    Transform targetPos,
    int amount, Transform fromPos = null,
    Action onCompleted = null, Action onCollided = null
  )
  {
    var coin = Instantiate(coinPsPrefab, fromPos.position, Quaternion.identity);
    coin.EmissingTo(targetPos, amount, fromPos, () =>
    {
      onCompleted?.Invoke();
      Destroy(coin.gameObject);
    }, onCollided);
  }

  public ParticleSystem SpawnBirthdayConfenttiAt(float3 pos)
  {
    var birthdayConfentti = Instantiate(birthdayConfenttiEfx, pos, Quaternion.identity);
    return birthdayConfentti;
  }

  public ParticleSystem SpawnMuzzleEfxAt(float3 pos, int gunType)
  {
    switch (gunType)
    {
      case 0:
        return Instantiate(pistolMuzzleEfx, pos, pistolMuzzleEfx.transform.rotation);
      case 1:
        return Instantiate(SMGMuzzleEfx, pos, SMGMuzzleEfx.transform.rotation);
      case 2:
        return Instantiate(shotGunMuzzleEfx, pos, shotGunMuzzleEfx.transform.rotation);
      case 3:
        return Instantiate(assaultRifleMuzzleEfx, pos, assaultRifleMuzzleEfx.transform.rotation);
      case 4:
        return Instantiate(sniperRiflesMuzzleEfx, pos, sniperRiflesMuzzleEfx.transform.rotation);
      case 5:
        return Instantiate(machineGunMuzzleEfx, pos, machineGunMuzzleEfx.transform.rotation);
    }
    return Instantiate(pistolMuzzleEfx, pos, pistolMuzzleEfx.transform.rotation);
  }

  public Animator SpawnFireFrameAnimAt(float3 pos)
  {
    var obj = Instantiate(fireFrameAnim, pos, fireFrameAnim.transform.rotation);
    return obj;
  }

  public ParticleSystem SpawnBulletBurstUpEfxAt(float3 pos)
  {
    var obj = Instantiate(bulletBurstUpEfx, pos, bulletBurstUpEfx.transform.rotation);
    return obj;
  }
}
