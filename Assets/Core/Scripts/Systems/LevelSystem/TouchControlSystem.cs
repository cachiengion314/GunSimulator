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
  void OnTouchEnd(float2 touchPos, float2 touchingDir) { }

  void OnTouchMoved(float2 touchPos, float2 touchingDir) { }

  void OnTouchBegan(float2 touchPos, Collider2D[] touchedColliders)
  {
    // // gắn ở button từ 0 - 2, 0 = single, 1 = auto, 2 = burst  
    switch (GameSystem.Instance.IdFireModes)
    {
      case 0:
        break;
      case 1:
        break;
      case 2:
        break;
    }

  }
}