using System;
using UnityEngine;

public partial class ItemSystem
{
    [SerializeField] TaserControl taserPref;
    [SerializeField] Transform _taserParent;
    public Transform TaserParent { get { return _taserParent; } }
    TaserControl _currentTaser;
    public TaserControl CurrentTaser { get { return _currentTaser; } }

    public void SpawnTaser(TaserData data)
    {
        _currentTaser = Instantiate(taserPref, _taserParent);
        _currentTaser.SetBodyRenderer(data.BodySprite);
        _currentTaser.SetColor(data.color);
        _currentTaser.SetCapacity(data.Capacity);
    }

    public void OnTaser()
    {
        _currentTaser?.OnTaser();
    }
    public void OffTaser()
    {
        _currentTaser?.OffTaser();
    }

    public void ReduceCurrentCapacity()
    {
        _currentTaser?.ReduceCurrentCapacity();
    }
}
