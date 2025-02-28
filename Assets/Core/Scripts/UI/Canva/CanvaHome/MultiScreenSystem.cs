using UnityEngine;
using UnityEngine.UI;

public class MultiScreenSystem : MonoBehaviour
{
    void Start()
    {
        Init();
    }
    void OnEnable(){
        Init();
    }
    void Init()
    {
        Canvas.ForceUpdateCanvases();
        SetSize();
    }
    void SetSize()
    {
        if (TryGetComponent(out RectTransform rect) && TryGetComponent(out GridLayoutGroup gridLayout))
        {
            float width = rect.rect.width;
            float height = rect.rect.height;
            float sizeX = width / transform.childCount;
            gridLayout.cellSize = new Vector2(sizeX, height);
        }
    }
}
