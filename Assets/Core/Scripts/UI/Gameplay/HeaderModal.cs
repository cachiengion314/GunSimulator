using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeaderModal : MonoBehaviour
{
  public static HeaderModal Instance { get; private set; }

  [Header("Internal dependencies")]
  [SerializeField] RectTransform contents;
  [SerializeField] TextMeshProUGUI moveTxt;
  [SerializeField] TextMeshProUGUI levelTxt;
  [SerializeField] GameObject questObj;
  [SerializeField] GameObject questSpecialObj;
  [SerializeField] TextMeshProUGUI showText;

  private void OnEnable()
  {
    LevelSystem.onInitedLevel += OnInitedLevel;
  }

  private void OnDisable()
  {
    LevelSystem.onInitedLevel -= OnInitedLevel;
  }

  private void Start()
  {
    Instance = this;
  }

  private void OnInitedLevel()
  {
    InitLevel();
  }

  private void InitLevel()
  {
    InitLevelUI();
    InitSetupLevelUI();

    questObj.SetActive(false);
    questSpecialObj.SetActive(false);
  }

  private void InitLevelUI()
  {
    var levelCurrent = LevelSystem.Instance.GetCurrentLevelDesign();
  }

  private void Spawn()
  {

  }

  private void InitSetupLevelUI()
  {

  }

  public void InitQuestUIs()
  {

  }

  public void UpdateQuestUIs()
  {

  }

  private void UpdateQuestAt(int id, int amount)
  {
    if (amount <= 0)
    {
      contents.GetChild(id + 2).GetChild(2).gameObject.SetActive(true);
      contents.GetChild(id + 2).GetChild(1).gameObject.SetActive(false);
      return;
    }

    contents.GetChild(id + 2).GetComponentInChildren<TextMeshProUGUI>().text = "" + amount;
    contents.GetChild(id + 2).GetChild(2).gameObject.SetActive(false);
  }

  public void UpdateSpecialQuest(int amount)
  {

  }

  private void UpdateAmountMove()
  {

  }

  public GameObject GetQuest(int idQuest)
  {
    return contents.GetChild(idQuest + 2).gameObject;
  }

  public void ShowTextAt(Vector2 screenPoint, string textValue, Color color, float _scale = 1f, float _slowFactor = 1f)
  {
    var _showText = Instantiate(showText, transform);

    _showText.transform.position = screenPoint;
    _showText.color = color;
    _showText.rectTransform.position = screenPoint;
    _showText.text = textValue;

    var targetScale = Vector3.one * _scale;
    var targetMove = screenPoint + Vector2.up * 1.5f;

    _showText.transform.DOScale(targetScale, _slowFactor).SetEase(Ease.OutBack);
    _showText.transform.DOMove(targetMove, _slowFactor * 1.1f).SetEase(Ease.OutBack)
      .OnComplete(() =>
      {
        Destroy(_showText.gameObject);
      });
  }
}
