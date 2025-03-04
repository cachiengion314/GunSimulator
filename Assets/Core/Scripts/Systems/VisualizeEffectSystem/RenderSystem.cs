using System;
using UnityEngine;

public class RenderSystem : MonoBehaviour
{
  public static RenderSystem Instance { get; private set; }

  [Header("Themes")]
  [SerializeField] ThemeObj[] themes;
  [Range(0, 2)]
  public int CurrentThemeIndex;

  private void Start()
  {
    Instance = this;
  }

  public Color GetColorFrom(int colorValue)
  {
    return themes[CurrentThemeIndex].LiquidColors[colorValue];
  }

  public int GetLengthColor()
  {
    return themes[CurrentThemeIndex].LiquidColors.Length;
  }

  public Color GetSurfaceColorFrom(int colorValue)
  {
    return themes[CurrentThemeIndex].LiquidSurfaceColors[colorValue];
  }

  public Sprite GetTraySpriteColorFrom(int colorValue)
  {
    return themes[CurrentThemeIndex].TrayRenderers[colorValue];
  }

  public Sprite GetBoxSpriteColorFrom(int health)
  {
    if (health <= 0) return themes[CurrentThemeIndex].BoxRenderers[0];
    return themes[CurrentThemeIndex].BoxRenderers[health - 1];
  }
  public Sprite GetSpriteIconPistol(int _id)
  {
    if (_id <= themes[CurrentThemeIndex].SpriteIconPistol.Length)
    {
      return themes[CurrentThemeIndex].SpriteIconPistol[_id];
    }
    return null;

  }
  public Sprite GetSpriteIconSMGgun(int _id)
  {
    if (_id <= themes[CurrentThemeIndex].SpriteIconSMGGun.Length)
    {
      return themes[CurrentThemeIndex].SpriteIconSMGGun[_id];
    }
    return null;

  }
  public Sprite GetSpriteIconShotGun(int _id)
  {
    if (_id <= themes[CurrentThemeIndex].SpriteIconShotGun.Length)
    {
      return themes[CurrentThemeIndex].SpriteIconShotGun[_id];
    }
    return null;
  }
  public Sprite GetSpriteIconRiffle(int _id)
  {
    if (_id <= themes[CurrentThemeIndex].SpriteIconRiffleGun.Length)
    {
      return themes[CurrentThemeIndex].SpriteIconRiffleGun[_id];
    }
    return null;
  }
  public Sprite GetSpriteIconSniper(int _id)
  {
    if (_id <= themes[CurrentThemeIndex].SpriteIconSniperGun.Length)
    {
      return themes[CurrentThemeIndex].SpriteIconSniperGun[_id];
    }
    return null;
  }

  public LightsaberData GetLightsaberDataAt(int _id)
  {
    if (_id <= themes[CurrentThemeIndex].lightsaberDatas.Length)
    {
      return themes[CurrentThemeIndex].lightsaberDatas[_id];
    }
    return null;
  }
  public LightsaberData[] GetLightsaberDatas()
  {
    return themes[CurrentThemeIndex].lightsaberDatas;
  }
  public Sprite GetSpriteIconGranade(int _id)
  {
    if (_id <= themes[CurrentThemeIndex].SpriteIconGranade.Length)
    {
      return themes[CurrentThemeIndex].SpriteIconGranade[_id];
    }
    return null;

  }
  public Sprite GetSpriteIconBomb(int _id)
  {
    if (_id <= themes[CurrentThemeIndex].SpriteIconBomb.Length)
    {
      return themes[CurrentThemeIndex].SpriteIconBomb[_id];
    }
    return null;

  }
}

