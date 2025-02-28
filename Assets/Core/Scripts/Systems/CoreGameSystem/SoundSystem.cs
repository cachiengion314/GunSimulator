using UnityEngine;
using Lofelt.NiceVibrations;
using UnityEngine.SceneManagement;
using HoangNam;

/// <summary> Fix haptic issue when build in IOS
/// https://github.com/asmadsen/react-native-unity-view/issues/35
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundSystem : MonoBehaviour
{
  public static SoundSystem Instance { get; private set; }

  [Header("Injected Dependencies")]
  [SerializeField] AudioClip mainThemeHome;
  [SerializeField] AudioClip mainThemeIngame;
  // 1.0
  [Header("---SFX---")]
  [SerializeField] AudioClip cannotTouchBoxSfx;
  [SerializeField] AudioClip _clickButtom;
  [SerializeField] AudioClip _hidePopup;
  [SerializeField] AudioClip _colectionTray;
  [SerializeField] AudioClip _dropTray;
  [SerializeField] AudioClip _trayComplete;
  [SerializeField] AudioClip _trayMovement;
  [SerializeField] AudioClip _dropCup;
  [SerializeField] AudioClip _levelComplete;
  [SerializeField] AudioClip _levelFail;
  [SerializeField] AudioClip pistolFireSfx;

  [Header("Components")]
  [SerializeField] AudioSource audioSource;

  private void Awake()
  {
    if (Instance == null)
      Instance = this;
    else Destroy(gameObject);
  }

  private void Start()
  {
    SceneManager.sceneLoaded += ScreenLoaded;
  }

  private void OnDestroy()
  {
    SceneManager.sceneLoaded -= ScreenLoaded;
  }

  void ScreenLoaded(Scene scene, LoadSceneMode mode)
  {
    if (GameSystem.Instance.IsMusicOn)
    {
      PlayMainThemeSfx();
    }
    else
    {
      StopMainThemeSfx();
    }
  }
  public void PlayHapticWith(HapticPatterns.PresetType presetType)
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(presetType);
    }
  }

  public void PlayButtonSfx()
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
    }
    if (!GameSystem.Instance.IsSoundOn) return;
    AudioSource.PlayClipAtPoint(_clickButtom, Vector3.forward * -9, 1);
  }

  public void PlayHidePopupSfx()
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }
    if (!GameSystem.Instance.IsSoundOn) return;
    AudioSource.PlayClipAtPoint(_hidePopup, Vector3.forward * -9, 1);
  }

  public void PlayColectionTraySfx()
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
    }
    if (!GameSystem.Instance.IsSoundOn) return;
    AudioSource.PlayClipAtPoint(_colectionTray, Vector3.forward * -9, 1);
  }

  public void PlayDropTraySfx()
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }
    if (!GameSystem.Instance.IsSoundOn) return;
    AudioSource.PlayClipAtPoint(_dropTray, Vector3.forward * -9, 1);
  }

  public void PlayBoxCompleteSfx()
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
    }
    if (!GameSystem.Instance.IsSoundOn) return;
    AudioSource.PlayClipAtPoint(_trayComplete, Vector3.forward * -9, 1);
  }

  public void PlayBoxMovementSfx()
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
    }
    if (!GameSystem.Instance.IsSoundOn) return;
    AudioSource.PlayClipAtPoint(_trayMovement, Vector3.forward * -9, 1f);
  }

  public void PlayLevelCompleteSfx()
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
    }
    if (!GameSystem.Instance.IsSoundOn) return;
    AudioSource.PlayClipAtPoint(_levelComplete, Vector3.forward * -9, 1);
  }

  // 1.0
  public void PlayLevelFailSfx()
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
    }
    if (!GameSystem.Instance.IsSoundOn) return;
    AudioSource.PlayClipAtPoint(_levelFail, Vector3.forward * -9, 1);
  }

  public void PlayDropCupSfx()
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }
    if (!GameSystem.Instance.IsSoundOn) return;
    AudioSource.PlayClipAtPoint(_dropCup, Vector3.forward * -9, 1f);
  }

  public void PlayCannotTouchCupSfx()
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
    }
    if (!GameSystem.Instance.IsSoundOn) return;
    AudioSource.PlayClipAtPoint(cannotTouchBoxSfx, Vector3.forward * -9, 1f);
  }


  public void PlayMainThemeSfx()
  {
    if (SceneManager.GetActiveScene().name.Equals(KeyStr.NAME_SCENE_LOBBY))
    {
      PlayMainThemeSfx(mainThemeHome);
    }
    else
    {
      PlayMainThemeSfx(mainThemeIngame);
    }
  }

  public void PlayMainThemeSfx(AudioClip audioClip)
  {
    audioSource.volume = 0.3f;
    audioSource.clip = audioClip;
    audioSource.loop = true;
    audioSource.Play();
  }

  public void StopMainThemeSfx()
  {
    audioSource.Stop();
  }

  public void PlayPistolSfx()
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }
    if (!GameSystem.Instance.IsSoundOn) return;
    AudioSource.PlayClipAtPoint(pistolFireSfx, Vector3.forward * -9, 1);
  }

}
