using System;
using UnityEngine;

public class LightsaberControl : MonoBehaviour
{
    [SerializeField] SpriteRenderer swordHiltRdr;
    [SerializeField] SpriteRenderer bladeRdr;
    public Action OnCurrentCapacityChange;
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

            ReduceCurrentCapacity();
        }
        else
        {
            size.y = Mathf.Lerp(size.y, 0, _speed * Time.deltaTime);
            if (size.y < 0.05f) size.y = 0;
        }
        pos.y = size.y / 2;

        bladeRdr.size = size;
        bladeRdr.transform.localPosition = pos;
    }

    public void Recharge()
    {
        CurrentCapacity = _capacity;
        _isCapacityEmpty = false;
    }

    public void ReduceCurrentCapacity()
    {
        CurrentCapacity -= Time.deltaTime;
        if(CurrentCapacity <= 0) _isCapacityEmpty = true;
    }
}
