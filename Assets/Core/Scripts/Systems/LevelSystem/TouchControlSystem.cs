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
        if (GameSystem.Instance.IdFireModes == 2) return; // Burst mode

        var currentGun = ItemSystem.Instance.GetCurrentGun();
        if (currentGun == null) return;
        if (currentGun.GetComponent<GunControl>().HaveAutoMode)
        {
          ItemSystem.Instance.AutoModeFireInvoke();
        }
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
        switch (GameSystem.Instance.IdFireModes)
        {
          case 0:
            ItemSystem.Instance.SingleModeFireInvoke();
            break;
          case 2:
            ItemSystem.Instance.BurstModeFireInvoke();
            break;
        }
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
}