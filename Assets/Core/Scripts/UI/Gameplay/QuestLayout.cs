using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class QuestLayout : MonoBehaviour
{
  public static QuestLayout Instance { get; private set; }

  [Header("Dependences")]
  [SerializeField] Transform[] questBlocks;
  [SerializeField] TMP_Text[] questAmountTxts;
  [SerializeField] Image[] questIconImgs;
  [SerializeField] Image[] tickImgs;

  private void Start()
  {
    Instance = this;
  }

  void ShowAmountTextAt(int index)
  {
    questAmountTxts[index].gameObject.SetActive(true);
    tickImgs[index].gameObject.SetActive(false);
  }

  void ShowTickImgAt(int index)
  {
    tickImgs[index].gameObject.SetActive(true);
    questAmountTxts[index].gameObject.SetActive(false);
  }

  public void VisualizeQuestBy(LevelDesignObj levelDesignObj)
  {

  }

  public Image FindQuestImgFor(int colorIdx)
  {
    return null;
  }

  public int FindAmountFor(int colorIdx)
  {
    return -1;
  }

  public void ChangeAmountFor(int colorIdx, int amount)
  {

  }

  public bool HasQuestFullilledAt(int colorIdx)
  {

    return false;
  }

  public bool HasQuestsFullilled()
  {

    return true;
  }
}
