using UnityEngine;
using Lofelt.NiceVibrations;
using UnityEngine.SceneManagement;
using HoangNam;
using System;

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
  [SerializeField] AudioClip gunReload;
  [System.Serializable]
  public struct GunAudioClips
  {
    public int idTypeGun;         // ID loại súng

    public AudioClip[] audioClips;// Danh sách âm thanh của loại súng đó
  }
  [Header("---ListPistolFireSfx---")] //Type theo id type DatdGunManager
  public GunAudioClips[] gunAudioClipsArray;
  private void OnValidate()
  {
    for (int i = 0; i < gunAudioClipsArray.Length; i++)
    {
      gunAudioClipsArray[i].idTypeGun = i; // Gán ID theo index tự động
    }
  }


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
  public void PlayGunSoundSfx(int idType, int idGun)
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(GetTypeHaptic(idType));
    }
    if (!GameSystem.Instance.IsSoundOn) return;
    AudioClip _audioClipTarget = GetGunSound(idType, idGun);
    if (_audioClipTarget == null) return;
    // auidoFireTypeSingle.PlayOneShot(_audioClipTarget);
    AudioSource.PlayClipAtPoint(_audioClipTarget, Vector3.forward * -9, 1);
  }
  public AudioClip GetGunSound(int idType, int idGun)
  {
    foreach (var gunAudio in gunAudioClipsArray)
    {
      if (gunAudio.idTypeGun == idType)
      {
        if (idGun >= 0 && idGun < gunAudio.audioClips.Length)
        {
          return gunAudio.audioClips[idGun];
        }
        else
        {
          Debug.LogError($"❌ idGun {idGun} vượt quá danh sách âm thanh loại {idType}");
          return null;
        }
      }
    }
    Debug.LogError($"❌ Không tìm thấy âm thanh cho loại súng {idType}");
    return null;
  }

  HapticPatterns.PresetType GetTypeHaptic(int idType)
  {
    switch (idType)
    {
      case 0: return HapticPatterns.PresetType.LightImpact;  // 🔹 Nhẹ, rung ngắn, phù hợp với bắn lẻ, chạm nhẹ UI
      case 1: return HapticPatterns.PresetType.MediumImpact; // 🔸 Trung bình, rung vừa, phù hợp với bắn liên thanh nhẹ
      case 2: return HapticPatterns.PresetType.HeavyImpact;  // 🔴 Mạnh, rung dài hơn, phù hợp với súng hạng nặng (Shotgun, Sniper)
      case 3: return HapticPatterns.PresetType.MediumImpact;      // ✅ Nhẹ nhàng, dành cho thông báo thành công (reload xong, headshot)
      case 4: return HapticPatterns.PresetType.HeavyImpact;      // ⚠️ Cảnh báo, rung dài hơn bình thường, phù hợp khi sắp hết đạn
      case 5: return HapticPatterns.PresetType.HeavyImpact;      // ❌ Cảnh báo mạnh, dành cho lỗi (hết đạn, súng kẹt)
      default:
        Debug.LogWarning($"⚠️ idType {idType} không hợp lệ, mặc định sử dụng LightImpact");
        return HapticPatterns.PresetType.LightImpact;
    }
  }
  public void GunReloadSfx()
  {
    if (GameSystem.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }
    if (!GameSystem.Instance.IsSoundOn) return;
    AudioSource.PlayClipAtPoint(gunReload, Vector3.forward * -9, 1f);
  }


}
