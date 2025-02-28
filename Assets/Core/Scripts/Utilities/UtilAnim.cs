using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public enum AnimType
{
  Spin,
  Rotate,
  PopUp,
  Scale
}

public class UtilAnim : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] AnimType type;
  [Range(0, 50)]
  [SerializeField] float speed;

  [Header("Rotate setting")]
  [Range(-180, 180)]
  [SerializeField] float toRotateAngles;

  [Header("PopUp setting")]
  [Range(0, 10)]
  [SerializeField] float toScale;

  [Header("Scale setting")]
  [Tooltip("Value limit scale when animation zoomout")]
  [SerializeField] float3 minScale;
  [Tooltip("Value limit scale when animation zoomin")]
  [SerializeField] float3 maxScale;

  float _speedFactor = 20;

  private void Start()
  {
    if (type == AnimType.Spin)
    {
      Spin();
    }
    else if (type == AnimType.Rotate)
    {
      Rotate();
    }
    else if (type == AnimType.PopUp)
    {
      PopUp();
    }
    else if (type == AnimType.Scale)
    {
      Scale();
    }
  }

  void Spin()
  {
    transform.DORotate(-Vector3.forward * 360, _speedFactor * 1f / speed, RotateMode.WorldAxisAdd)
      .SetEase(Ease.Linear)
      .SetLoops(-1);
  }

  void Rotate()
  {
    Sequence mySeq = DOTween.Sequence();

    mySeq.Insert(0,
      transform.DORotate(-Vector3.forward * toRotateAngles, _speedFactor * 1f / speed, RotateMode.WorldAxisAdd)
      .SetEase(Ease.InOutElastic)
      .SetLoops(-1, LoopType.Yoyo)
    );

    mySeq.Insert(0,
      transform.DOLocalMoveY(20, _speedFactor * 1f / speed)
      .SetEase(Ease.InOutElastic)
      .SetLoops(-1, LoopType.Yoyo)
    );
  }

  void PopUp()
  {
    Sequence mySeq = DOTween.Sequence();

    mySeq.Insert(0,
      transform.DOScaleX(toScale, _speedFactor * 1f / speed)
        .SetEase(Ease.InOutElastic)
        .SetLoops(-1, LoopType.Yoyo)
    );
    mySeq.Insert(0,
      transform.DOScaleY(toScale, _speedFactor * 1f / speed)
        .SetEase(Ease.InOutElastic)
        .SetLoops(-1, LoopType.Yoyo)
    );
  }

  void Scale()
  {
    transform.DOScale(maxScale, _speedFactor * 1f / speed)
      .SetLoops(-1, LoopType.Yoyo);
  }
}
