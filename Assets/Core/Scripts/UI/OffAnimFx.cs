using UnityEngine;

public class OffAnimFx : MonoBehaviour
{
    public void SetOffGameObjAnimFx()
    {
        if (GameSystem.Instance.IdFireModes == 2)
        {
            PlayerController.Instance.countBurst += 1;
            if (PlayerController.Instance.countBurst >= 3)
            {
                PlayerController.Instance._isbursting = false;
                PlayerController.Instance.countBurst = 0;
                gameObject.SetActive(false);
                Debug.Log("tắtFX");
            }
        }
        else
        {
            // if (GameSystem.Instance._IdFireModes != 2)
            // {
            GameSystem.Instance.IdFireModes = 0;
            PlayerController.Instance._isbursting = false;
         
            // }
            gameObject.SetActive(false);
            Debug.Log("tắtFX");
        }
    }
}
