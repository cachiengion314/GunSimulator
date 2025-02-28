using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinTxt : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinTxt;
    private void Start()
    {
        GameSystem.Instance.onCoinChanged += SetCoin;
        SetCoin();
    }

    private void OnDestroy()
    {
        GameSystem.Instance.onCoinChanged -= SetCoin;
    }
    private void SetCoin()
    {
        coinTxt.SetText(GameSystem.Instance.CurrentCoin.ToString());
    }
}
