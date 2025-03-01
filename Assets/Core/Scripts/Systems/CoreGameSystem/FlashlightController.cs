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
      try
      {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
          Permission.RequestUserPermission(Permission.Camera);
        }
        // Get Camera instance
        AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");
        camera = cameraClass.CallStatic<AndroidJavaObject>("open", 0);
        cameraParameters = camera.Call<AndroidJavaObject>("getParameters");
      }
      catch (System.Exception e)
      {
        Debug.LogError("Failed to access flashlight: " + e.Message);
      }
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

  void OnDestroy()
  {
    if (camera != null)
    {
      camera.Call("release");
    }
  }
}
