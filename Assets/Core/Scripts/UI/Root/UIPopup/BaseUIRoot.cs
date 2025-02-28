using DG.Tweening;
using UnityEngine;

public class BaseUIRoot : MonoBehaviour
{
  [SerializeField] protected RectTransform modal;
  public float time = 0.3f;
  [SerializeField] protected Ease easeTypeShow = Ease.OutBack;
  [SerializeField] protected Ease easeTypeHide = Ease.InBack;
  public virtual void DoOnCreate() { }
  public virtual void DoBeforeShow() { }
  public virtual void DoAfterHide() { }

  public virtual void Show()
  {

    DoBeforeShow();
    OnShow();
  }
  public virtual void Hide()
  {
    OnHide();
    DoAfterHide();
  }

  public virtual void DelayShow()
  {
    DOVirtual.DelayedCall(time, () => Show());
  }

  public virtual void DelayHide()
  {
    DOVirtual.DelayedCall(time, () => Hide());
  }

  protected virtual void OnShow()
  {
    gameObject.SetActive(true);
    modal.transform.localScale = Vector2.zero;
    modal.DOScale(1f, time).SetEase(easeTypeShow)
    .OnComplete(() =>
    {
      OnShowCompleted(); // Gọi callback khi hoàn tất OnShow
    });
  }
  protected virtual void OnHide()
  {
    modal.transform.localScale = Vector2.one;
    modal.DOScale(0, time).SetEase(easeTypeHide).OnComplete(
        () =>
        {
          gameObject.SetActive(false);
          OnHideCompleted();
        }
    );

  }

  public virtual void Exit()
  {
    UIManager.Instance.Hide();
  }

  public virtual void ExitAll()
  {
    UIManager.Instance.HideAll();
  }

  protected virtual void OnShowCompleted()
  {
    // Phương thức này sẽ được override trong lớp con
  }
  protected virtual void OnHideCompleted()
  {
    // 🔥 Phương thức mới để xử lý sau khi UI đã ẩn hoàn toàn
  }
  public void SetGameStateUnPause()
  {
    // GameSystem.Instance.SetGameState(GameState.Gameplay);
  }

}
