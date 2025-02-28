using TMPro;
using UnityEngine;

public class PriceTxt : MonoBehaviour
{
    [SerializeField] int productId;
    [SerializeField] TextMeshProUGUI priceTxt;
    private void Start()
    {
        SetText();
    }

    private void SetText()
    {
        string price = IAPSystem.Instance.GetLocalizedPrice(productId);
        if (price == null || price == "")
            Invoke(nameof(SetText), Time.deltaTime);
        else
            priceTxt.SetText(price);
    }
}
