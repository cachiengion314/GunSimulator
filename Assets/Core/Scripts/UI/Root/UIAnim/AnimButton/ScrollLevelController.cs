using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class ScrollLevelController : MonoBehaviour
{
  public static ScrollLevelController Instance { get; private set; }
  [SerializeField] int _allLevel; //max level
  public int _active; // level hiện tại
  [SerializeField] float _timeTweening;//thời gian thực hiện 
  [SerializeField] Ease _tweenType; // dạng tween
  Vector3 target;
  [SerializeField] Vector3 pageStep;
  [SerializeField] RectTransform _levelPagesRect;
  [SerializeField] private Vector3 StartPagePos;
  Vector3 PosPage;
  [SerializeField] Button _btnNext;
  [SerializeField] Button _btnPrevious;
  [SerializeField] GameObject _parentBoxLevel;
  [SerializeField] GameObject _prefabUsedBoxLevel;
  [SerializeField] GameObject _prefabActiveBoxLevel;
  [SerializeField] GameObject _prefabBlockBoxLevel;

  void Start()
  {
    if (Instance == null)
    {
      Instance = this;
    }

    // CreatePage(_active);

  }
  public void BtnNext()
  {
    if (_active < _allLevel)
    {
      _active++;
      target += pageStep; // dịch chuyển sang vị trí tiếp theo
      PosPage = target;
      // Debug.Log("PosPage--Next--" + PosPage);
      MovePage();
    }
  }
  public void BtnPrevious()
  {
    if (_active > 1)
    {
      _active--;
      target -= pageStep;
      PosPage = target;
      // Debug.Log("PosPage--Previous--" + PosPage);
      MovePage();
    }
  }
  void MovePage()
  {
    DOTween.Kill(_levelPagesRect);
    _levelPagesRect.DOLocalMove(PosPage, _timeTweening).SetEase(_tweenType);
    UpdateButton();
  }
  void CreatePage(int _startValueLevel)// sinh ra level + set vị trí ban đầu , chuyền vào level hiện tại
  {
    for (int i = 1; i <= _allLevel; i++)
    {
      GameObject newBox = null;
      if (i < _startValueLevel)
      {
        newBox = Instantiate(_prefabUsedBoxLevel, _parentBoxLevel.transform);
      }
      if (i == _startValueLevel)
      {
        newBox = Instantiate(_prefabActiveBoxLevel, _parentBoxLevel.transform);
      }
      if (i > _startValueLevel)
      {
        newBox = Instantiate(_prefabBlockBoxLevel, _parentBoxLevel.transform);
      }
      if (newBox != null) // Đảm bảo newBox không null trước khi thao tác
      {
        SetChildText(newBox, "Level\n " + i);
      }
      else
      {
        Debug.LogError($"Không thể khởi tạo box cho level {i}");
      }
    }


    StartPagePos = _levelPagesRect.localPosition; // xác định vị trí ban đầu
    target = -StartPagePos + _active * pageStep; // vị trí hướng đến 
    _levelPagesRect.DOLocalMove(target, 0.01f).SetEase(_tweenType);
    PosPage = _levelPagesRect.localPosition;
    UpdateButton();
  }
  void SetChildText(GameObject parent, string newText)
  {
    // Kiểm tra nếu GameObject có con
    if (parent.transform.childCount > 0)
    {
      // Truy cập phần tử con đầu tiên
      Transform firstChild = parent.transform.GetChild(0);

      // Kiểm tra và gán Text
      var textComponent = firstChild.GetComponent<TextMeshProUGUI>();
      if (textComponent != null)
      {
        textComponent.text = newText; // Gán text mới
      }
      else
      {
        Debug.LogWarning("TextMeshProUGUI không được tìm thấy trong phần tử con đầu tiên của " + parent.name);
      }
    }
    else
    {
      Debug.LogWarning("GameObject " + parent.name + " không có phần tử con.");
    }
  }
  void UpdateButton()
  {
    _btnNext.gameObject.SetActive(true);
    _btnPrevious.gameObject.SetActive(true);
    if (_active == 1)
    {
      _btnPrevious.gameObject.SetActive(false);
    }
    if (_active == _allLevel)
    {
      _btnNext.gameObject.SetActive(false);
    }

  }


}
