using System.Collections;
using TMPro;
using UnityEngine;

public class UiNotify : BaseUIRoot
{
  float timeLife = 2f;
  [SerializeField] TextMeshProUGUI messTxt;
  Coroutine hideNotify;
  public void ShowNotify(string message)
  {
    messTxt.SetText(message);
    Show();
    if (hideNotify != null)
    {
      StopCoroutine(hideNotify);
    }
    hideNotify = StartCoroutine(Delay());
  }

  IEnumerator Delay()
  {
    yield return new WaitForSeconds(timeLife);
    Hide();
  }
}
