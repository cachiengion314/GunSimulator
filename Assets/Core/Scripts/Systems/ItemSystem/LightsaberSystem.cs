using UnityEngine;

public partial class ItemSystem
{
    [SerializeField] LightsaberControl lightsaberPref;
    [SerializeField] Transform _lightsaberParent;
    public Transform LightsaberParent { get { return _lightsaberParent; } }
    LightsaberControl _currentLightsaber;
    public LightsaberControl CurrentLightsaber { get { return _currentLightsaber; } }

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
        _currentLightsaber.BladeLengthen();
    }

    public void SetExpanding(bool isExpanding)
    {
        if(!_currentLightsaber) return;
        _currentLightsaber.isExpanding = isExpanding;
    }
}
