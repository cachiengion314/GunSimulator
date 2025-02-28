using HoangNam;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LightsaberUiControl : MonoBehaviour
{
    int _id;
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] Image lightsaberImg;
    public void SetName(string name)
    {
        nameTxt.text = name;
    }
    public void SetLightsaberImg(Sprite sprite)
    {
        lightsaberImg.sprite = sprite;
    }
    public void SetId(int id)
    {
        _id = id;
    }

    public void SelectLightsaberat()
    {
        GameSystem.Instance.CurrentLightsaberIndex = _id;
        GameSystem.Instance.LoadSceneByName(KeyStr.NAME_SCENE_GAMEPLAY);
    }
}
