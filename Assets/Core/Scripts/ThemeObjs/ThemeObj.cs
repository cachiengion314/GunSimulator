using UnityEngine;

[CreateAssetMenu(fileName = "ThemeObj", menuName = "ScriptableObjects/ThemeObj", order = 0)]
public class ThemeObj : ScriptableObject
{
  public Color[] LiquidColors;
  public Color[] LiquidSurfaceColors;
  public Sprite[] TrayRenderers;
  public Sprite[] BoxRenderers;
  public Sprite[] SpriteIconPistol;
  public Sprite[] SpriteIconSMGGun;
  public Sprite[] SpriteIconShotGun;
  public Sprite[] SpriteIconRiffleGun;
  public Sprite[] SpriteIconSniperGun;
  public LightsaberData[] lightsaberDatas;
  public TaserData[] taserDatas;

  public Sprite[] SpriteIconGranade;
  public Sprite[] SpriteIconBomb;
}
