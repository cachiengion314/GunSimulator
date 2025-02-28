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
  [SerializeField] ParticleSystem muzzleEfx;
  [SerializeField] Animator fireFrameAnim;
  [SerializeField] ParticleSystem bulletBurstUpEfx;

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

  public ParticleSystem SpawnMuzzleEfxAt(float3 pos)
  {
    var obj = Instantiate(muzzleEfx, pos, muzzleEfx.transform.rotation);
    return obj;
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
