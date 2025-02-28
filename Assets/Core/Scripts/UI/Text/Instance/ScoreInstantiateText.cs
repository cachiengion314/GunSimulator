using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TMPro;

public class ScoreInstantiateText : MonoBehaviour
{
    [SerializeField] float _valueScaleTexScore = 1.2f;
    float _valueMove = 200f;
    float floatDefault;
    void Start()
    {
        SetUPAnim();
        PlayAnim();
    }
    void SetUPAnim()
    {
        this.transform.localScale = Vector3.zero;

        floatDefault = this.transform.GetComponent<RectTransform>().anchoredPosition.y;
    }
    void PlayAnim()
    {

        float Target = floatDefault + _valueMove;

        TMP_Text textScore = this.transform.GetComponent<TMP_Text>();
        Sequence sequence = DOTween.Sequence();

        sequence.Append(this.transform.DOLocalMoveY(Target, 0.8f).SetEase(Ease.OutBack));
        sequence.Join(this.transform.DOScale(Vector3.one * _valueScaleTexScore, 0.8f).SetEase(Ease.OutBack));
        sequence.Append(textScore.DOFade(0, 0.8f))
        .OnComplete(() =>
        {
            Destroy(this.gameObject);
        });

    }
}


