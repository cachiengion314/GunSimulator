using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSlectCtrl : MonoBehaviour
{
    [Header("Block level")]
    [SerializeField] Image blockLevelImg;
    [SerializeField] Sprite blockSelectSpr;
    [SerializeField] Sprite blockUncheckedSpr;

    [Space(10)]
    [Header("Line level")]
    [SerializeField] Image lineLevelImg;
    [SerializeField] Sprite lineSelectSpr;
    [SerializeField] Sprite lineUncheckedSpr;

    [Space(10)]
    [Header("Compoment")]
    [SerializeField] TextMeshProUGUI levelTxt;

    public void Select(int level)
    {
        levelTxt.SetText((level+1).ToString());
        blockLevelImg.sprite = blockSelectSpr;
        lineLevelImg.sprite = lineSelectSpr;
    }

    public void Unchecked(int level)
    {
        levelTxt.SetText((level+1).ToString());
        blockLevelImg.sprite = blockUncheckedSpr;
        lineLevelImg.sprite = lineUncheckedSpr;
    }
}
