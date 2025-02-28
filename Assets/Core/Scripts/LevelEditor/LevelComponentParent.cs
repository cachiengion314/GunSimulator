using UnityEngine;

[ExecuteInEditMode]
public class LevelComponentParent : MonoBehaviour
{
  [Header("Dependencies")]
  [SerializeField] LevelEditor levelEditor;

  private void Awake()
  {

  }

  public void Clear()
  {
    for (int i = transform.childCount - 1; i >= 0; i--)
    {
      DestroyImmediate(transform.GetChild(i).gameObject);
    }
  }
}
