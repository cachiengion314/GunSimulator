using UnityEngine;
[CreateAssetMenu(fileName = "TaserData", menuName = "TaserData")]
public class TaserData : ScriptableObject
{
    public string Name;
    public Sprite BodySprite;
    public Color color;
    public float Capacity;
    public int isVideoAds;
}
