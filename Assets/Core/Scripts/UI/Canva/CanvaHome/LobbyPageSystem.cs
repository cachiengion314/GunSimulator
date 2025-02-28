using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HoangNam;
using UnityEngine;

public class LobbyPageSystem : MonoBehaviour
{
    [SerializeField] GameObject[] pages;
    [SerializeField] float duration;
    float temp = 0;
    void Start()
    {
        // LobbyPageSelectSystem.OnSelect += SelectAt;
        SelectAt(1);
    }

    // private void OnDestroy()
    // {
    //     LobbyPageSelectSystem.OnSelect -= SelectAt;
    // }

    public void SelectAt(int index)
    {
        Camera mainCamera = Camera.main;
        Vector2 posCamera = mainCamera.transform.position;
        float witdhCamera = mainCamera.orthographicSize * 2f * mainCamera.aspect;
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < pages.Length; i++)
        {
            Vector2 pos = posCamera;
            if (i < index)
                pos.x = pos.x - witdhCamera * (index - i);
            else if (i > index)
                pos.x = pos.x + witdhCamera * (i - index);
            seq.Join(
                pages[i].transform.DOMove(pos, temp)
            );
        }
        seq.OnComplete(() => temp = duration);
    }
    public void BtnSetting()
    {
        UIManager.Instance.Show(KeyStr.NAME_SETTING_1_MODAL);
        
    }
    public void BtnTestShop2()
    {
        UIManager.Instance.Show(KeyStr.NAME_SHOP_MODAL);
        
    }
     public void BtnTestWin()
    {
        UIManager.Instance.Show(KeyStr.NAME_WIN_LEVEL_MODAL);
        
    }
    public void SFXPlayButtonSfx()
    {
        SoundSystem.Instance.PlayButtonSfx();
    }
}
