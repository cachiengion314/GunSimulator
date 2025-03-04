using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// TouchControlSystem
/// </summary>
public partial class LevelSystem : MonoBehaviour
{
  void OnTouchEnd(float2 touchPos, float2 touchingDir)
  {
    switch (GameSystem.Instance.IdShopMode)
    {
      case 0:
        break;
      case 1:
        break;
      case 2:
        ItemSystem.Instance.OffTaser();
        break;
      case 3:
        ItemSystem.Instance.SetExpanding(false);
        break;

    }

  }

  void OnTouchMoved(float2 touchPos, float2 touchingDir)
  {
    switch (GameSystem.Instance.IdShopMode)
    {
      case 0:

        if (touchPos.x > 2.7f || touchPos.x < -2.7f) return;
        GunHold();

        break;
      case 1:
        break;
      case 2:
        break;
      case 3:
        ItemSystem.Instance.SetExpanding(true);
        break;

    }

  }

  void OnTouchBegan(float2 touchPos, Collider2D[] touchedColliders)
  {
    switch (GameSystem.Instance.IdShopMode)
    {
      case 0:
        if (touchPos.x > 2.7f || touchPos.x < -2.7f) return;
        GunTouch();
        break;
      case 1:
        if (touchPos.x > 2f || touchPos.x < -2f) return;
        ExplosionTouch();
        break;
      case 2:
        ItemSystem.Instance.OnTaser();
        break;
      case 3:
        ItemSystem.Instance.SetExpanding(true);
        break;

    }

  }

  void GunHold()
  {
    if (GameSystem.Instance.IdFireModes == 2) return; // Burst mode

    var currentGun = ItemSystem.Instance.GetCurrentGun();
    if (currentGun == null) return;
    if (currentGun.GetComponent<GunControl>().HaveAutoMode)
    {
      ItemSystem.Instance.AutoModeFireInvoke();
    }
  }
  void GunTouch()
  {
    switch (GameSystem.Instance.IdFireModes)
    {
      case 0:
        ItemSystem.Instance.SingleModeFireInvoke();
        break;
      case 2:
        ItemSystem.Instance.BurstModeFireInvoke();
        break;
    }
  }


  void ExplosionTouch()
  {
    Debug.Log("ExplosionTouch 1");
    switch (GameSystem.Instance.IdTypePick)
    {
      case 0:
        ItemSystem.Instance.GranadeModeInvoke();
        Debug.Log("ExplosionTouch 2.1");
        break;
      case 1:
        ItemSystem.Instance.BombModeInvoke();
        Debug.Log("ExplosionTouch 2.2");
        break;
    }

  }
}