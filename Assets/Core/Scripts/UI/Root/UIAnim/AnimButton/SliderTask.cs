using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public partial class UiIngameRoot : BaseUIRoot
{
    // [SerializeField] Slider _slider;
    // [SerializeField] int _valueTest;
    // [SerializeField] GameObject _prefabTask;
    // [SerializeField] private RectTransform _sliderPrarentFillArea; // Khu vực Fill của Slider
    // void GenerateTasks(int _value)//_value != 1;
    // {
    //     // Lấy RectTransform của toàn bộ Slider (bao gồm track)
    //     RectTransform sliderRect = _slider.GetComponent<RectTransform>();
    //     float sliderWidth = sliderRect.rect.width; // Chiều rộng của Slider

    //     if (_value == 1)
    //     {
    //         // Trường hợp chỉ có một task, đặt ở cuối slider (_slider.value = 1)
    //         GameObject task = Instantiate(_prefabTask, _sliderPrarentFillArea.transform);
    //         // task.transform.GetChild(0).GetComponent<Image>().sprite = RenderSystem.Instance.GetSpriteTask(0);
    //         RectTransform taskRect = task.GetComponent<RectTransform>();
    //         taskRect.anchoredPosition = new Vector2(0, 0); // Điểm cuối (bên phải)
    //         return; // Kết thúc hàm
    //     }

    //     // Tính toán khoảng cách giữa các task (nếu có nhiều hơn 1 task)
    //     float startX = -sliderWidth / 2; // Điểm bắt đầu (nằm ở trái Slider)
    //     float taskSpacing = sliderWidth / (_value - 1); // Khoảng cách giữa các task

    //     for (int i = 0; i < _value; i++)
    //     {
    //         // Tính vị trí X của mỗi task
    //         float taskXPosition = startX + (taskSpacing * i);

    //         // Tạo task từ prefab
    //         GameObject task = Instantiate(_prefabTask, _sliderPrarentFillArea.transform);
    //         // task.transform.GetChild(0).GetComponent<Image>().sprite = RenderSystem.Instance.GetSpriteTask(i);
    //         // Đặt vị trí của task
    //         RectTransform taskRect = task.GetComponent<RectTransform>();
    //         taskRect.anchoredPosition = new Vector2(taskXPosition, 0); // Căn chỉnh vị trí X
    //     }
    // }
    // public void NextPhaseSlider(int _currentPhase)// 
    // {
    //     if (_valueTest == 1)
    //     {
    //         _slider.DOValue(1f, 0.2f)
    //           .SetEase(Ease.Linear)
    //           .OnComplete(() =>
    //           {
    //               GameObject TaskCompulele = _sliderPrarentFillArea.GetChild(_currentPhase - 1).gameObject;
    //               StatusTask(TaskCompulele, false);
    //           }
    //           );
    //         return;
    //     }

    //     float moveValue = 1f / (_valueTest - 1);
    //     // Debug.Log("moveValue--" + moveValue);
    //     float targetValue = _slider.value + moveValue;
    //     // Debug.Log("targetValue--" + targetValue);

    //     // Đảm bảo giá trị nằm trong khoảng [0, 1]
    //     targetValue = Mathf.Clamp(targetValue, 0f, 1f);
    //     // Debug.Log("targetValue--zz--" + targetValue);
    //     // Sử dụng DOTween để di chuyển slider
    //     _slider.DOValue(targetValue, 0.2f)
    //            .SetEase(Ease.Linear)
    //            .OnComplete(() =>
    //            {
    //                Debug.Log("----TaskCompulele----");
    //                GameObject TaskCompulele = _sliderPrarentFillArea.GetChild(_currentPhase - 1).gameObject;
    //                StatusTask(TaskCompulele, false);
    //            }
    //            //    Debug.Log($"Slider đã di chuyển thêm {moveValue}. Giá trị hiện tại: {targetValue}")
    //            );
    // }
    // void StatusTask(GameObject parent, bool _status)
    // {
    //     parent.transform.GetChild(0).gameObject.SetActive(_status);
    //     parent.transform.GetChild(1).gameObject.SetActive(!_status);
    // }
}
