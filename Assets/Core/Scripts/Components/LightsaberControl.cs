using System;
using DigitalRuby.LightningBolt;
using UnityEngine;

public class LightsaberControl : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] LightningBoltScript lightningBoltScript;
    [SerializeField] SpriteRenderer swordHiltRdr;
    [SerializeField] SpriteRenderer bladeRdr;
    public Action OnCurrentCapacityChange;
    public Action OnCapacityEmpty;
    float _capacity;
    public float Capacity { get { return _capacity; } }
    float _currentCapacity;
    bool _isCapacityEmpty = false;
    public float CurrentCapacity
    {
        get { return _currentCapacity; }
        set
        {
            _currentCapacity = value;
            OnCurrentCapacityChange?.Invoke();
        }
    }
    float _speed;
    public float Speed { get { return _speed; } }
    float _bladeLength;
    public float BladeLength { get { return _bladeLength; } }
    public bool isExpanding = false;

    public void SetSpriteSwordHilt(Sprite sprite)
    {
        swordHiltRdr.sprite = sprite;
    }

    public Sprite GetSpriteSwordHilt()
    {
        return swordHiltRdr.sprite;
    }

    public void SetColorBlade(Color color)
    {
        bladeRdr.material.SetColor("_EmissionColor", color * 5);
        lineRenderer.colorGradient = GetGradient(color);
    }

    Gradient GetGradient(Color color)
    {
        Gradient myGradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = color;
        colorKeys[0].time = 0.0f;

        colorKeys[1].color = Color.white;
        colorKeys[1].time = 1.0f;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;

        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 1.0f;

        myGradient.SetKeys(colorKeys, alphaKeys);
        return myGradient;
    }

    public Color GetColorBlade()
    {
        return bladeRdr.material.GetColor("_EmissionColor");
    }

    public void SetCapacity(float capacity)
    {
        _capacity = capacity;
        CurrentCapacity = capacity;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetBladeLength(float bladeLength)
    {
        _bladeLength = bladeLength;
    }

    public void BladeLengthen()
    {
        var size = bladeRdr.size;
        var pos = bladeRdr.transform.localPosition;

        if (isExpanding && !_isCapacityEmpty)
        {

            size.y = Mathf.Lerp(size.y, _bladeLength, _speed * Time.deltaTime);
            if (_bladeLength - size.y < 0.05f) size.y = _bladeLength;
            SoundSystem.Instance.StartLightsaberPlaySfx();
            ReduceCurrentCapacity();
        }
        else
        {
            SoundSystem.Instance.StopLightsaberPlaySfx();
            size.y = Mathf.Lerp(size.y, 0, _speed * 2f * Time.deltaTime);
            if (size.y < 0.05f) size.y = 0;
        }
        pos.y = size.y / 2;

        bladeRdr.size = size;
        bladeRdr.transform.localPosition = pos;

        SetPositionLightningBolt();
    }



    public void Recharge()
    {
        CurrentCapacity = _capacity;
        _isCapacityEmpty = false;
    }

    void ReduceCurrentCapacity()
    {
        CurrentCapacity -= Time.deltaTime;
        if (CurrentCapacity <= 0)
        {
            OnCapacityEmpty?.Invoke();
            SoundSystem.Instance.StopLightsaberPlaySfx();
            _isCapacityEmpty = true;
        }
    }

    void SetPositionLightningBolt()
    {
        var startPos = bladeRdr.transform.position;
        var endPos = bladeRdr.transform.position;
        startPos.y += bladeRdr.size.y / 2;
        endPos.y -= bladeRdr.size.y / 2;
        lightningBoltScript.StartPosition = startPos;
        lightningBoltScript.EndPosition = endPos;
    }
}
