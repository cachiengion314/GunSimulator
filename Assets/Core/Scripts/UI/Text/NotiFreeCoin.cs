using System;
using UnityEngine;

public class NotiFreeCoin : MonoBehaviour
{
    void Start()
    {
        GameSystem.Instance.onChangeNotiFreeCoin += ShowNoti;
        ShowNoti();
    }
    void OnDestroy()
    {
        GameSystem.Instance.onChangeNotiFreeCoin -= ShowNoti;
    }

    void ShowNoti()
    {
        string STRToday = DateTime.Now.ToString("yyyyMMdd");
        LoginData dayData = LoginManager.Instance.GetDayData(STRToday);
        if (dayData.amountRewardAds > 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
