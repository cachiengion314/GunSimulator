using System;
using UnityEngine;

public class TaserControl : MonoBehaviour
{
    [SerializeField] SpriteRenderer body;
    [SerializeField] LineRenderer lightning;
    public Action OnCurrentCapacityChange;
    public Action OnCapacityEmpty;
    float _capacity;
    public float Capacity { get { return _capacity; } }
    float _currentCapacity;
    public float CurrentCapacity
    {
        get { return _currentCapacity; }
        set
        {
            _currentCapacity = value;
            OnCurrentCapacityChange?.Invoke();
        }
    }
    bool _isCapacityEmpty = false;
    public bool IsCapacityEmpty { get { return _isCapacityEmpty; } }
    public void SetBodyRenderer(Sprite sprite)
    {
        body.sprite = sprite;
    }

    public void SetColor(Color color)
    {
        lightning.colorGradient = GetGradient(color);
    }

    public void SetCapacity(float capacity)
    {
        _capacity = capacity;
        CurrentCapacity = capacity;
    }

    Gradient GetGradient(Color color)
    {
        Gradient myGradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = color;
        colorKeys[0].time = 0.0f;

        colorKeys[1].color = color;
        colorKeys[1].time = 1.0f;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;

        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 1.0f;

        myGradient.SetKeys(colorKeys, alphaKeys);
        return myGradient;
    }

    public void OnTaser()
    {
        if (_isCapacityEmpty) return;
        lightning.gameObject.SetActive(true);
        SoundSystem.Instance.StartLightsaberPlaySfx();
    }

    public void OffTaser()
    {
        if (_isCapacityEmpty) return;
        lightning.gameObject.SetActive(false);
        SoundSystem.Instance.StopLightsaberPlaySfx();
    }

    public void Recharge()
    {
        CurrentCapacity = _capacity;
        _isCapacityEmpty = false;
    }

    public void ReduceCurrentCapacity()
    {
        CurrentCapacity -= Time.deltaTime;
        if (CurrentCapacity <= 0)
        {
            OnCapacityEmpty?.Invoke();
            OffTaser();
            SoundSystem.Instance.StopLightsaberPlaySfx();
            _isCapacityEmpty = true;
        }
    }
}
