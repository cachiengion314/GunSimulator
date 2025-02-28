using UnityEngine;
using UnityEngine.UI;

public class MultiScreen : MonoBehaviour
{
  [SerializeField] GridLayoutGroup gridLayoutGroup;
  void Start()
  {
    Canvas.ForceUpdateCanvases();
    Invoke(nameof(SetSize), 0);
  }
  public void SetSize()
  {
    RectTransform rectTransform = GetComponent<RectTransform>();
    float width = rectTransform.rect.width;
    float height = rectTransform.rect.height;
    float sizeX = width / transform.childCount;
    gridLayoutGroup.cellSize = new Vector2(sizeX, height);
  }
}
