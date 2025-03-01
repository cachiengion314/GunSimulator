using UnityEngine;
using UnityEngine.Android;
using System.Collections;

public class FlashlightController : MonoBehaviour
{
  private AndroidJavaObject cameraManager;
  private string cameraID;
  private bool isFlashlightOn = false;

  void Start()
  {
    if (Application.platform == RuntimePlatform.Android)
    {
      StartCoroutine(RequestPermissionAndInitialize());
    }
    else
    {
      Debug.LogWarning("FlashlightController is running on a non-Android platform.");
    }
  }

  IEnumerator RequestPermissionAndInitialize()
  {
    if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
    {
      Permission.RequestUserPermission(Permission.Camera);

      while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
      {
        yield return null;
      }
    }

    InitializeFlashlight();
  }

  void InitializeFlashlight()
  {
    try
    {
      // Get CameraManager instance
      AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
      AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
      cameraManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "camera");

      // Get the list of camera IDs
      string[] cameraIDList = cameraManager.Call<string[]>("getCameraIdList");

      if (cameraIDList != null && cameraIDList.Length > 0)
      {
        cameraID = cameraIDList[0]; // Use the first camera (usually rear)
        Debug.Log("Camera initialized with ID: " + cameraID);
      }
      else
      {
        Debug.LogError("No camera found on this device.");
      }
    }
    catch (System.Exception e)
    {
      Debug.LogError("Failed to initialize Camera2 API: " + e.Message);
    }
  }

  public void ToggleFlashlight()
  {
    if (Application.platform == RuntimePlatform.Android && cameraManager != null && !string.IsNullOrEmpty(cameraID))
    {
      isFlashlightOn = !isFlashlightOn; // Toggle state
      cameraManager.Call("setTorchMode", cameraID, isFlashlightOn);
      Debug.Log("Flashlight toggled: " + isFlashlightOn);
    }
  }
}
