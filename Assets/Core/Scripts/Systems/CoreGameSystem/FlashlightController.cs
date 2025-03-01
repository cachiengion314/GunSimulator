using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class FlashlightController : MonoBehaviour
{
  private new AndroidJavaObject camera;
  private AndroidJavaObject cameraParameters;

  void Start()
  {
    if (Application.platform == RuntimePlatform.Android)
    {
      StartCoroutine(RequestPermissionAndInitialize());
    }
  }

  IEnumerator RequestPermissionAndInitialize()
  {
    // Request permission if not granted
    if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
    {
      Permission.RequestUserPermission(Permission.Camera);

      // Wait until the user grants permission
      while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
      {
        yield return null; // Wait until the next frame
      }
    }

    // Now that we have permission, initialize the camera
    InitializeCamera();
  }

  public void InitializeCamera()
  {
    try
    {
      AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");
      camera = cameraClass.CallStatic<AndroidJavaObject>("open", 0);
      cameraParameters = camera.Call<AndroidJavaObject>("getParameters");

      Debug.Log("Camera initialized successfully.");
    }
    catch (System.Exception e)
    {
      Debug.LogError("Failed to access camera: " + e.Message);
    }
  }

  public void TurnOnFlashlight()
  {
    if (Application.platform == RuntimePlatform.Android && camera != null)
    {
      cameraParameters.Call("setFlashMode", "torch");
      camera.Call("setParameters", cameraParameters);
      camera.Call("startPreview");
    }
  }

  public void TurnOffFlashlight()
  {
    if (Application.platform == RuntimePlatform.Android && camera != null)
    {
      cameraParameters.Call("setFlashMode", "off");
      camera.Call("setParameters", cameraParameters);
      camera.Call("stopPreview");
    }
  }

  void Release()
  {
    if (camera != null)
    {
      camera.Call("release");
    }
  }

  void OnApplicationQuit()
  {
    Release();
  }

  void OnDestroy()
  {
    Release();
  }
}
