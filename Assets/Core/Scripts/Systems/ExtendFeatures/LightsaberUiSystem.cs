using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LightsaberUiSystem : MonoBehaviour
{
    [SerializeField] Image currentLightsaberImg;
    [SerializeField] LightsaberUiControl lightsaberUiPref;
    [SerializeField] Transform _lightsaberUiParent;

    void Start()
    {
        var LightsaberDatas = RenderSystem.Instance.GetLightsaberDatas();
        SpawnLightsaberUi(LightsaberDatas);
        SetCurrentLightsaber();
    }

    public void SpawnLightsaberUi(LightsaberData[] datas)
    {
        for (int i = 0; i < datas.Length; i++)
        {
            var lightsaber = Instantiate(lightsaberUiPref, _lightsaberUiParent);
            lightsaber.SetId(i);
            lightsaber.SetName(datas[i].Name);
            lightsaber.SetLightsaberImg(datas[i].SwordHilt);
        }
    }

    public void SetCurrentLightsaber()
    {
        currentLightsaberImg.sprite = GameSystem.Instance.GetCurrentLightsaberData().SwordHilt;

        var duration = 0.3f;
        var ratiaScale = 1.2f;
        var objectScale = currentLightsaberImg.transform.localScale;
        currentLightsaberImg.transform
        .DOScale(objectScale * ratiaScale,duration)
        .SetEase(Ease.Linear)
        .SetLoops(-1,LoopType.Yoyo);
    }
}
