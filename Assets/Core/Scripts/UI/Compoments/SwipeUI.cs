using DG.Tweening;
using UnityEngine;

public class SwipeUI : MonoBehaviour
{
  [SerializeField] RectTransform canvas;
  [SerializeField] float time = 0.1f;
  RectTransform rect;
  float size;
  private void Start()
  {
    size = canvas.sizeDelta.x;

    Vector2 sizeDeltaCanvas = canvas.sizeDelta;
    sizeDeltaCanvas.x *= transform.childCount;

    rect = GetComponent<RectTransform>();
    rect.sizeDelta = sizeDeltaCanvas;

    GetComponent<MultiScreen>().SetSize();
  }
  public void Select(int id)
  {
    float to = size * (1 - id);
    DOTween.To(() => rect.anchoredPosition.x, x =>
    {
      rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);
    }, to, time);
  }
}
