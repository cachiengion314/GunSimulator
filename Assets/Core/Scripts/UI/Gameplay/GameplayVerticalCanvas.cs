using HoangNam;
using TMPro;
using UnityEngine;

public class GameplayVerticalCanvas : MonoBehaviour
{
  public static GameplayVerticalCanvas Instance { get; private set; }

  [Header("Dependencies")]
  [SerializeField] TMP_Text lvlText;
  public TMP_Text LvlText { get { return lvlText; } }
  [SerializeField] QuestLayout questLayout;

  private void Start()
  {
    Instance = this;
  }

  public void ShowSetting()
  {
    // SoundSystem.Instance.PlayButtonSfx();
    UIManager.Instance.Show(KeyStr.NAME_SETTING_1_MODAL);
  }
}
