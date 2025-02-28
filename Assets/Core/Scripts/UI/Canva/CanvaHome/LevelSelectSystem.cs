using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectSystem : MonoBehaviour
{
    [Header("Level setup")]
    [SerializeField] LevelSlectCtrl levelPrefab;
    [SerializeField] int limitSelect;
    [SerializeField] int amountLevel;
    [SerializeField] ScrollRect _scrollRect;
    void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        SetUp(GameSystem.Instance.CurrentLevelIndex);
        yield return null;
        SelectLevel(GameSystem.Instance.CurrentLevelIndex);
    }

    void SelectLevel(int currentLevel)
    {
        int index = currentLevel;
        if (currentLevel > limitSelect) index = limitSelect - 1;
        if (TryGetComponent(out RectTransform rect)
            && transform.GetChild(0).TryGetComponent(out RectTransform rect1))
        {
            rect.anchoredPosition = new Vector2(0, -index * rect1.rect.height);
        }
    }

    void SetUp(int currentLevel)
    {
        int level = 0;
        if (currentLevel > limitSelect)
        {
            level = currentLevel - limitSelect + 1;
        }
        for (int i = 0; i < amountLevel; i++)
        {
            var levelObj = Instantiate(levelPrefab, transform);
            levelObj.transform.SetAsFirstSibling();
            if (level <= currentLevel)
                levelObj.Select(level);
            else
                levelObj.Unchecked(level);
            level++;
        }
        SetposScrollView();
    }
    void SetposScrollView()
    {
        // Kiểm tra nếu có con
        if (transform.childCount > 0)
        {
            // Lấy đối tượng con cuối cùng
            RectTransform lastChild = transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>();

            // Tính toán vị trí cần cuộn
            float contentHeight = GetComponent<RectTransform>().rect.height;
            float lastChildPosition = lastChild.anchoredPosition.y;

            // Đặt vị trí ScrollView sao cho con cuối cùng nằm đúng vị trí
            float normalizedPosition = Mathf.Clamp01(1 - (lastChildPosition / contentHeight));
            _scrollRect.verticalNormalizedPosition = normalizedPosition;
        }
    }
}
