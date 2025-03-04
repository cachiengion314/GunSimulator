using UnityEngine;

public partial class ItemSystem
{
    [SerializeField] LightsaberControl lightsaberPref;
    [SerializeField] Transform _lightsaberParent;
    public Transform LightsaberParent { get { return _lightsaberParent; } }
    LightsaberControl _currentLightsaber;
    public LightsaberControl CurrentLightsaber { get { return _currentLightsaber; } }
    public bool isExpanding = false;

    public void SpawnLightsaber(LightsaberData data)
    {
        _currentLightsaber = Instantiate(lightsaberPref, _lightsaberParent);
        _currentLightsaber.SetSpriteSwordHilt(data.SwordHilt);
        _currentLightsaber.SetColorBlade(data.BladeColor);
        _currentLightsaber.SetCapacity(data.Capacity);
        _currentLightsaber.SetSpeed(data.Speed);
        _currentLightsaber.SetBladeLength(data.BladeLength);
    }

    public void BladeLengthen()
    {
        if (!_currentLightsaber) return;
        if (isExpanding && !_currentLightsaber.IsCapacityEmpty)
        {
            _currentLightsaber.OnBladeLengthen();
            _currentLightsaber.ReduceCurrentCapacity();
            SoundSystem.Instance.StartLightsaberPlaySfx();
        }
        else
        {
            _currentLightsaber.OffBladeLengthen();
            SoundSystem.Instance.StopLightsaberPlaySfx();
        }
        _currentLightsaber.SetPositionLightningBolt();
    }

    public void SetExpanding(bool isExpanding)
    {
        if (!_currentLightsaber) return;
        this.isExpanding = isExpanding;
    }

    public void OnColorChange(Color color)
    {
        if (!_currentLightsaber) return;
        _currentLightsaber.SetColorBlade(color);
    }
}
