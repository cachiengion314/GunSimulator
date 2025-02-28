using UnityEngine;

public class ColorTrail : MonoBehaviour
{
  [SerializeField] ParticleSystem _particleSystem;
  [SerializeField] ParticleSystem _smokeParticleSystem;
  [SerializeField] ParticleSystem _starParticleSystem;

  public void ChangeDuration(float duration)
  {
    _particleSystem.Stop();
    var main = _particleSystem.main;
    main.duration = duration;
  }

  public void ChangeColorTo(Color color)
  {
    var rend = _particleSystem.GetComponent<ParticleSystemRenderer>();
    for (int i = 0; i < rend.materials.Length; ++i)
    {
      rend.materials[i].SetColor("_Color", color);
    }

    var smokeRend = _smokeParticleSystem.GetComponent<ParticleSystemRenderer>();
    for (int i = 0; i < smokeRend.materials.Length; ++i)
    {
      smokeRend.materials[i].SetColor("_Color", color);
    }

    var starRend = _starParticleSystem.GetComponent<ParticleSystemRenderer>();
    for (int i = 0; i < starRend.materials.Length; ++i)
    {
      starRend.materials[i].SetColor("_Color", color);
    }
  }

  public void ChangeSortingOrderTo(int sortingOrder)
  {
    var rend = _particleSystem.GetComponent<ParticleSystemRenderer>();
    for (int i = 0; i < rend.materials.Length; ++i)
    {
      rend.sortingOrder = sortingOrder;
    }

    var smokeRend = _smokeParticleSystem.GetComponent<ParticleSystemRenderer>();
    for (int i = 0; i < smokeRend.materials.Length; ++i)
    {
      smokeRend.sortingOrder = sortingOrder;
    }

    var starRend = _starParticleSystem.GetComponent<ParticleSystemRenderer>();
    for (int i = 0; i < starRend.materials.Length; ++i)
    {
      starRend.sortingOrder = sortingOrder;
    }
  }

  public void Stop()
  {
    _particleSystem.Stop();
  }

  public void Play()
  {
    _particleSystem.Play();
  }
}
