using HoangNam;
using UnityEngine;

[ExecuteInEditMode]
public class LevelLoadInvoker : MonoBehaviour
{
  [SerializeField] LevelEditor levelEditor;
  bool _isLoadLevelsFromDisk;

  private void Awake()
  {
#if UNITY_EDITOR
    if (Application.isPlaying) return;
    if (levelEditor == null) return;
    if (_isLoadLevelsFromDisk) return;
    HoangNam.Utility.Print("Load levels from disk successfully!");
    _isLoadLevelsFromDisk = true;
    levelEditor.LoadLevelsFromDisk();
#endif
  }
}
