using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;

public class CanvaButtonUI : MonoBehaviour
{
  int oldInDex = -1;
  int index = 1; // intdex = 1 là home  , 0 là shop , 2 là leaderboard
  [SerializeField] GameObject[] objSelectTargets; // Các phần tử: 0 = Shop, 1 = Home  --// người con 1 là icon , người con 2 là text
  float valueHide = 0.4f;
  float valueYPosDefault;
  float _valueStartButton = -200;
  [SerializeField] RectTransform _rctContents;


  void Awake()
  {
    SetUpDefault();
  }
  void SetUpDefault()
  {
    valueYPosDefault = objSelectTargets[0].transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y;
    for (int i = 0; i < objSelectTargets.Length; i++)
    {
      if (i == 1) // Mặc định chỉ mục 1 (Home) được hiện
      {
        // objSelectTargets[i].transform.GetChild(0).localScale = Vector3.one;
        objSelectTargets[i].transform.GetChild(1).localScale = Vector3.one;
      }
      else
      {
        // objSelectTargets[i].transform.GetChild(0).localScale = Vector3.zero;
        objSelectTargets[i].transform.GetChild(1).localScale = Vector3.zero;

        Vector2 setup = objSelectTargets[i].transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition;
        setup.y += _valueStartButton;
        objSelectTargets[i].transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = setup;

        Color _Color = objSelectTargets[i].transform.GetChild(0).GetComponent<Image>().color;
        _Color.a = 0;
        objSelectTargets[i].transform.GetChild(0).GetComponent<Image>().color = _Color;
      }
    }
  }
  public void AnimLoadObjPick(int newIndex, int OldIndex)
  {
    objSelectTargets[newIndex].transform.gameObject.SetActive(true);
    AnimHide(objSelectTargets[OldIndex]);
    AnimShow(objSelectTargets[newIndex]);

  }
  public void ChangeIndexAndAnimate(int newIndex) // ga
  {

    if (newIndex == index)
    {
      return;
    }
    if (newIndex == 0) // nếu bấm vào shop
    {
      string STRToday = DateTime.Now.ToString("yyyyMMdd");
      LoginData dayData = LoginManager.Instance.GetDayData(STRToday);
      // if (dayData.amountRewardAds > 0)
      // {
      //   _rctContents.anchoredPosition = new Vector2(0f, 4000f);
      // }
      // else
      // {
        _rctContents.anchoredPosition = new Vector2(0f, -1500f);
      // }
    }
    oldInDex = index;
    index = newIndex;
    AnimLoadObjPick(index, oldInDex); // Gọi animation
  }
  void AnimHide(GameObject _objTarget)
  {
    GameObject BGButton = _objTarget.transform.GetChild(0).gameObject;
    GameObject parentChild = _objTarget.transform.GetChild(1).gameObject;
    GameObject childOne = parentChild.transform.GetChild(0).gameObject;
    GameObject childTwo = parentChild.transform.GetChild(1).gameObject;

    //SetupAnim

    _objTarget.transform.DOKill(); // false để ngăn oncompelete chạy 
    childOne.transform.DOKill();
    childTwo.transform.DOKill();
    BGButton.transform.DOKill();


    //play anim
    parentChild.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.Linear);

    BGButton.GetComponent<Image>().DOFade(0, 0.1f);
    // BGButton.transform.DOScale(Vector3.one * valueHide, 0.2f).SetEase(Ease.InOutQuad);
    BGButton.transform.DOLocalMoveY(valueYPosDefault + _valueStartButton, 0.3f).SetEase(Ease.InBack);

    childOne.transform.GetComponent<Image>().DOFade(0, 0.3f);
    childTwo.transform.GetComponent<TMP_Text>().DOFade(0, 0.3f);


  }
  void AnimShow(GameObject _objTarget)
  {
    GameObject BGButton = _objTarget.transform.GetChild(0).gameObject;
    GameObject parentChild = _objTarget.transform.GetChild(1).gameObject;
    GameObject childOne = parentChild.transform.GetChild(0).gameObject;
    GameObject childTwo = parentChild.transform.GetChild(1).gameObject;

    //SetupAnim


    //play anim

    _objTarget.transform.DOKill(); // false để ngăn oncompelete chạy 
    childOne.transform.DOKill();
    childTwo.transform.DOKill();
    BGButton.transform.DOKill();

    parentChild.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear);

    BGButton.GetComponent<Image>().DOFade(1, 0.1f);
    // BGButton.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutQuad);
    BGButton.transform.DOLocalMoveY(valueYPosDefault, 0.3f).SetEase(Ease.OutBack);

    childOne.transform.GetComponent<Image>().DOFade(1, 0.3f);
    childTwo.transform.GetComponent<TMP_Text>().DOFade(1, 0.3f);

  }




}
