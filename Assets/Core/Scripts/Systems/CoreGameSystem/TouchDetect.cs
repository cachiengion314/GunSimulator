using System;
using System.Collections.Generic;
using Lean.Touch;
using Unity.Mathematics;
using UnityEngine;

public class TouchDetect : MonoBehaviour
{
  public static TouchDetect Instance { get; private set; }

  [Header("Properties")]
  Vector2 _touchedPosition;
  public float2 TouchingDirection;

  [Header("Events")]
  public Action<float2, Collider2D[]> onTouchBegan;
  public Action<float2, float2> onTouchMoved;
  public Action<float2, float2> onTouchEnd;

  [Header("Delay touch")]
  float _delayTouchTimer = 0;
  float _DELAYTOUCH_LENGTH = .5f;
  bool _isDelayTouch = false;

  void Awake()
  {
    Instance = this;
  }

  void Start()
  {
    LeanTouch.OnFingerOld += LeanTouch_OnFingerOld;
    LeanTouch.OnFingerDown += LeanTouch_OnDetect;
    LeanTouch.OnGesture += LeanTouch_OnGesture;
    LeanTouch.OnFingerUp += LeanTouch_OnFingerUp;
    LeanTouch.OnFingerExpired += LeanTouch_OnFingerExpired;
  }

  void OnDestroy()
  {
    LeanTouch.OnFingerOld -= LeanTouch_OnFingerOld;
    LeanTouch.OnFingerDown -= LeanTouch_OnDetect;
    LeanTouch.OnGesture -= LeanTouch_OnGesture;
    LeanTouch.OnFingerUp -= LeanTouch_OnFingerUp;
    LeanTouch.OnFingerExpired -= LeanTouch_OnFingerExpired;
  }

  private void Update()
  {
    if (!SpawnSystem.Instance.IsCompleted) return;

    if (LeanTouch.Fingers.Count > 1)
    {
      if (!_isDelayTouch)
      {
        onTouchEnd?.Invoke(0, 0);
      }
      _isDelayTouch = true;
      _delayTouchTimer += Time.deltaTime;
      if (_delayTouchTimer > _DELAYTOUCH_LENGTH)
      {
        _delayTouchTimer = 0;
        _isDelayTouch = false;
      }
    }
    else
    {
      _delayTouchTimer = 0;
      _isDelayTouch = false;
    }
  }

  private void LeanTouch_OnFingerOld(LeanFinger finger)
  {

  }

  void LeanTouch_OnDetect(LeanFinger finger)
  {
    if (!SpawnSystem.Instance.IsCompleted) return;
    if (GameSystem.Instance.GetGameState() != GameState.Gameplay) return;
    if (_isDelayTouch) return;

    Vector2 startTouchPos = Camera.main.ScreenToWorldPoint(finger.ScreenPosition);

    Collider2D[] colliders = Physics2D.OverlapPointAll(startTouchPos);

    onTouchBegan?.Invoke(startTouchPos, colliders);

    ///
    if (startTouchPos.x > 2.7f || startTouchPos.x < -2.7f) return;
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

  void LeanTouch_OnGesture(List<LeanFinger> leanFingers)
  {
    if (GameSystem.Instance.GetGameState() != GameState.Gameplay) return;
    if (leanFingers.Count == 0 || leanFingers.Count > 1) return;

    var selectedFinger = leanFingers[0];
    TouchingDirection = selectedFinger.ScreenPosition - selectedFinger.LastScreenPosition;
    _touchedPosition = Camera.main.ScreenToWorldPoint(selectedFinger.ScreenPosition);

    onTouchMoved?.Invoke(_touchedPosition, TouchingDirection);

    ///
    if (_touchedPosition.x > 2.7f || _touchedPosition.x < -2.7f) return;
    switch (GameSystem.Instance.IdFireModes)
    {
      case 1:
        ItemSystem.Instance.AutoModeFireInvoke();
        break;
    }
  }

  private void LeanTouch_OnFingerUp(LeanFinger finger)
  {
    onTouchEnd?.Invoke(_touchedPosition, TouchingDirection);
    TouchingDirection = Vector2.zero;
  }

  private void LeanTouch_OnFingerExpired(LeanFinger finger)
  {
    onTouchEnd?.Invoke(_touchedPosition, TouchingDirection);
  }
}