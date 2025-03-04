using UnityEngine;

[CreateAssetMenu(fileName = "LightsaberData", menuName = "LightsaberData")]
public class LightsaberData : ScriptableObject
{
    public string Name;
    public Sprite SwordHilt;
    public Color BladeColor;
    public float Capacity;
    public float Speed;
    public float BladeLength;
    public int isVideoAds;
}
