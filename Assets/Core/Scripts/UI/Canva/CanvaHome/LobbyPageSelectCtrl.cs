using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPageSelectCtrl : MonoBehaviour
{
    [SerializeField] GameObject select;
    public void Selecting()
    {
        select.SetActive(true);
    }

    public void Unchecking()
    {
        select.SetActive(false);
    }
}
