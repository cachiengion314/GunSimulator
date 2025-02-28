using System;

public class HeartsSystem
{
  public static Action onLoadHeartsEvent;
  int heartsMax;
  public bool infinteHeartsState;

  public HeartsSystem(int heartsMax = 5, bool infinteHeartsState = false)
  {
    this.heartsMax = heartsMax;
    this.infinteHeartsState = infinteHeartsState;
  }

  public int GetHeartsMax { get => heartsMax; }
  public bool GetInfinteHeartsState { get => infinteHeartsState; }

  public void InfinteHeartsState(bool infinteHeartsState)
  {
    this.infinteHeartsState = infinteHeartsState;
  }

  public void AddHearts(int hearts)
  {
    HeartsLimit();
    onLoadHeartsEvent?.Invoke();
  }

  public void LostHearts(int hearts)
  {
    if (infinteHeartsState) return;
    HeartsLimit();
    onLoadHeartsEvent?.Invoke();
  }

  private void HeartsLimit()
  {

  }
}
