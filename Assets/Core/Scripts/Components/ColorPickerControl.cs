using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerControl : MonoBehaviour
{
    [SerializeField] Image colorBarImg;
    [SerializeField] Slider slider;
    public Action<Color> OnColorChange;

    public void OnValueChange()
    {
        var texture2D = colorBarImg.sprite.texture;
        Color selectedColor = texture2D.GetPixelBilinear(0, 1 -slider.value);
        OnColorChange?.Invoke(selectedColor);
    }
}
