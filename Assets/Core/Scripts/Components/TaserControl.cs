using UnityEngine;

public class TaserControl : MonoBehaviour
{
    [SerializeField] SpriteRenderer body;
    [SerializeField] LineRenderer lightning;
    public void SetBodyRenderer(Sprite sprite)
    {
        body.sprite = sprite;
    }

    public void SetColor(Color color)
    {
        lightning.colorGradient = GetGradient(color);
    }

    Gradient GetGradient(Color color)
    {
        Gradient myGradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = color;
        colorKeys[0].time = 0.0f;

        colorKeys[1].color = color;
        colorKeys[1].time = 1.0f;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;

        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 1.0f;

        myGradient.SetKeys(colorKeys, alphaKeys);
        return myGradient;
    }

    public void OnTaser()
    {
        lightning.gameObject.SetActive(true);
    }

    public void OffTaser()
    {
        lightning.gameObject.SetActive(false);
    }
}
