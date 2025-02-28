using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class UiLoadingPurchase : BaseUIRoot
{
  [SerializeField] TextMeshProUGUI loading;
  [SerializeField] float delay = 1f;
  private void OnEnable()
  {
    StartCoroutine(AnimLoading());
  }
  IEnumerator AnimLoading()
  {
    while (true)
    {
      loading.SetText("Loading");
      yield return new WaitForSeconds(delay);
      loading.SetText("Loading.");
      yield return new WaitForSeconds(delay);
      loading.SetText("Loading..");
      yield return new WaitForSeconds(delay);
      loading.SetText("Loading...");
      yield return new WaitForSeconds(delay);
    }
  }
}
