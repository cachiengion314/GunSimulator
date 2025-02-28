using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour
{
  enum STATE { Active, Disable }

  [Serializable]
  struct SelectBtn
  {
    public STATE state;
    public RectTransform iconImg;
    public RectTransform tileTxt;
    public Button selectBtn;
    public Image btnImg;
    public RectTransform bgrBtn;
    public RectTransform lockImg;
  }
  [SerializeField] float time;
  [SerializeField] float distancePos;
  [SerializeField] float ratioScale;
  [SerializeField] SelectBtn[] buttonSelects;
  float posFrom;
  float posTo;
  Vector2 scaleFrom;
  Vector2 scaleTo;
  int currentId = -1;
  private void Start()
  {
    Init();
  }
  void Init()
  {
    posTo = buttonSelects[1].iconImg.anchoredPosition.y;
    posFrom = posTo + distancePos;

    scaleTo = buttonSelects[1].iconImg.transform.localScale;
    scaleFrom = scaleTo * ratioScale;

    InitSelect(1);
  }

  void InitSelect(int id)
  {
    for (int i = 0; i < buttonSelects.Length; i++)
    {
      if (buttonSelects[i].state == STATE.Disable)
      {
        Disable(i);
      }
    }
    Select(id);
  }

  public void Select(int id)
  {
    if (buttonSelects[id].state == STATE.Disable) return;

    SoundSystem.Instance.PlayButtonSfx();
    for (int i = 0; i < buttonSelects.Length; i++)
    {
      if (buttonSelects[i].state == STATE.Active)
      {
        if (i == id)
        {
          SelectAnim(i);
        }
        else
        {
          ActiveAnim(i);
        }
      }
    }
  }
  public void Disable(int id)
  {
    buttonSelects[id].state = STATE.Disable;
    buttonSelects[id].selectBtn.enabled = false;
    DisableAnim(id);
  }
  public void Active(int id)
  {
    buttonSelects[id].state = STATE.Active;
    buttonSelects[id].selectBtn.enabled = true;

    RectTransform lockImg = buttonSelects[id].lockImg;
    lockImg.gameObject.SetActive(false);

    RectTransform iconRect = buttonSelects[id].iconImg;
    iconRect.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);

    ActiveAnim(id);
  }
  private void SelectAnim(int id)
  {
    if (currentId == id) return;
    currentId = id;

    RectTransform iconRect = buttonSelects[id].iconImg;
    RectTransform tileRect = buttonSelects[id].tileTxt;
    RectTransform bgrBtn = buttonSelects[id].bgrBtn;

    tileRect.gameObject.SetActive(true);
    bgrBtn.gameObject.SetActive(true);

    iconRect.DOScale(scaleFrom, time);
    DOTween.To(() => iconRect.anchoredPosition.y, y =>
    {
      buttonSelects[id].iconImg.anchoredPosition = new Vector2(0, y);
    }, posFrom, time);
  }
  private void ActiveAnim(int id)
  {
    RectTransform iconRect = buttonSelects[id].iconImg;
    RectTransform tileRect = buttonSelects[id].tileTxt;
    RectTransform bgrBtn = buttonSelects[id].bgrBtn;

    tileRect.gameObject.SetActive(false);
    bgrBtn.gameObject.SetActive(false);

    iconRect.DOScale(scaleTo, time);
    DOTween.To(() => iconRect.anchoredPosition.y, y =>
    {
      buttonSelects[id].iconImg.anchoredPosition = new Vector2(0, y);
    }, posTo, time);
  }
  private void DisableAnim(int id)
  {
    RectTransform lockImg = buttonSelects[id].lockImg;
    RectTransform iconRect = buttonSelects[id].iconImg;

    iconRect.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
    lockImg.gameObject.SetActive(true);
  }
}
