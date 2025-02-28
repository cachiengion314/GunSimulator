using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevel : MonoBehaviour
{
  [SerializeField] Image levelImg;
  [SerializeField] Image levelBarImg;
  [SerializeField] TextMeshProUGUI levelTxt;
  [SerializeField] Sprite levelBarOn;
  [SerializeField] Sprite levelOn;
  [SerializeField] Sprite levelBarOff;
  [SerializeField] Sprite levelOff;

  public void SetLevelOn()
  {
    levelImg.sprite = levelOn;
    levelBarImg.sprite = levelBarOn;
  }

  public void SetLevelOff()
  {
    levelImg.sprite = levelOff;
    levelBarImg.sprite = levelBarOff;
  }
  public void SetLevel(int level)
  {
    levelTxt.SetText(level.ToString());
  }
  public void SetActiveLevelBar(bool active)
  {
    levelBarImg.gameObject.SetActive(active);
  }
}
