using HoangNam;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HackManager : BaseUIRoot
{
  public static bool noAds = false;
  [SerializeField] TMP_InputField tMPInputField;

  public void AddCoin(int amount)
  {
    GameSystem.Instance.CurrentCoin += amount;
  }

  public void NextLvl()
  {
    if (int.TryParse(tMPInputField.text, out int level))
    {
      GameSystem.Instance.CurrentLevelIndex = level - 1;
      SceneManager.LoadScene("Gameplay");
    }
  }

  public void Winlevel()
  {
    if (SceneManager.GetActiveScene().name.Equals(KeyStr.NAME_SCENE_GAMEPLAY))
    {
      LevelSystem.Instance.OnCompletedLevel();
    }
  }

  public void Lose()
  {
    if (SceneManager.GetActiveScene().name.Equals(KeyStr.NAME_SCENE_GAMEPLAY))
    {
      LevelSystem.Instance.OnLoseLevel();
    }
  }

  bool isUnlock = false;
  public void Booster()
  {

  }
}
