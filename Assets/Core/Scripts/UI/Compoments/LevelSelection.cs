using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
  [SerializeField] UILevel[] levels;
  [SerializeField] float[] scrollLevelValues;
  [SerializeField] Scrollbar scrollbar;
  [SerializeField] int levelLimt;
  int minlv = 0;
  private void Awake()
  {
    levels = GetComponentsInChildren<UILevel>();
  }
  private void Start()
  {
    scrollLevelValues = new float[transform.childCount];
    float valueDistance = 1.15f / (scrollLevelValues.Length - 1f);

    for (int i = 0; i < scrollLevelValues.Length; ++i)
    {
      scrollLevelValues[i] = valueDistance * i;
    }

    LevelManager(GameSystem.Instance.CurrentLevelIndex);
    SelectLevel(GameSystem.Instance.CurrentLevelIndex - minlv);
  }
  void SelectLevel(int level)
  {
    if (level < 0 || level > transform.childCount - 1) Debug.LogError($"Level {level} is not selected");
    scrollbar.value = scrollLevelValues[level];
  }
  private void LevelManager(int level)
  {
    if (level > levelLimt)
    {
      minlv = level - levelLimt;
      levels[transform.childCount - 1].SetActiveLevelBar(true);
    }
    else
    {
      levels[transform.childCount - 1].SetActiveLevelBar(false);
    }
    for (int i = 0; i < transform.childCount; i++)
    {
      levels[i].SetLevel(minlv + transform.childCount - i);

      if (transform.childCount - 1 - i <= level - minlv) levels[i].SetLevelOn();
      else levels[i].SetLevelOff();
    }
  }
}
