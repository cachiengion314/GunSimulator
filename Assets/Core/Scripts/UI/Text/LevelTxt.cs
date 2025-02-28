using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTxt : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelTxt;
    [SerializeField] int differenceNumber = 1;
    [SerializeField] string _stringFirst;
    
    void Start()
    {
        levelTxt.SetText(_stringFirst+ (GameSystem.Instance.CurrentLevelIndex + differenceNumber));
    }
}
