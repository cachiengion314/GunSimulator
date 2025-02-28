using UnityEngine;

public class GridBackground : MonoBehaviour
{
  public Transform gridParent; // GameObject chứa các ô vuông
  public GameObject background; // GameObject làm BG

  void Start()
  {
    // Tính toán kích thước của grid dựa trên các ô vuông

  }
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.K))
    {
      Bounds bounds = CalculateBounds(gridParent);

      // Cập nhật vị trí và scale cho BG
      background.transform.position = bounds.center;
      background.transform.localScale = new Vector3(bounds.size.x, bounds.size.y, 1);
    }
  }

  Bounds CalculateBounds(Transform parent)
  {
    Bounds bounds = new Bounds(parent.GetChild(0).position, Vector3.zero);
    foreach (Transform child in parent)
    {
      bounds.Encapsulate(child.position);
    }
    return bounds;
  }
}
