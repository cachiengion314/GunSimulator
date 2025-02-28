using UnityEngine;

public class SetActiveRemove : MonoBehaviour
{
  private void Start()
  {
    SetRemoveAds();
    GameSystem.Instance.onChangeRemoveads += SetRemoveAds;
  }

  private void OnDestroy()
  {
    GameSystem.Instance.onChangeRemoveads -= SetRemoveAds;
  }

  void SetRemoveAds()
  {
    gameObject.SetActive(!GameSystem.Instance.IsRemoveAds);
  }
}
