using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public partial class ItemSystem
{
    [Header("Explosion dependencies")]
    [SerializeField] ExplosionControl GranadePref;
    [SerializeField] ExplosionControl BombPref;
    [SerializeField] Transform explosionParent;
    public Transform ExplosionParent { get { return explosionParent; } }

    [Header("Datas")]
    public Action OnExploSion;
    public Action OnOutOfBomb;

    public GameObject GetCurrentExplosion()
    {
        var gun = explosionParent.GetChild(0);
        if (gun == null) return null;
        return gun.gameObject;
    }
    public void SpawnGaranade(int _idGun = 0)
    {
        var obj = Instantiate(GranadePref, explosionParent);

        obj.GetComponent<ExplosionControl>().ChangeBodyRendererBaseOn(_idGun);

        obj.transform.localScale = defaultLocalScale;
        obj.transform.position = new Vector3(0, 0, 0);
    }
    public void SpawnBomb(int _idGun = 0)
    {
        var obj = Instantiate(BombPref, explosionParent);

        obj.GetComponent<ExplosionControl>().ChangeBodyRendererBaseOn(_idGun);

        obj.transform.localScale = defaultLocalScale;
        obj.transform.position = new Vector3(0, 0, 0);
    }

    public void SpawnExplosionBy(int gunType, int idGun) // id type 0 = Garanade , id type 1 = bomb
    {
        switch (gunType)
        {
            case 0:
                SpawnGaranade(idGun);
                break;
            case 1:
                SpawnBomb(idGun);
                break;
        }

        var _dataExplosionTarget = DataExplosionManager.Instance.GetExplosionDataClassPick();
        int intInitAmmo = _dataExplosionTarget._currentStartValue;

        int intCurrentAmmo = _dataExplosionTarget._currentValue;

        var _currentExplosion = GetCurrentExplosion();
        if (_currentExplosion == null) return;

        var _explosionControl = _currentExplosion.GetComponent<ExplosionControl>();
        _explosionControl.SetInitAmmo(intInitAmmo);
        _explosionControl.SetCurrentAmmo(intCurrentAmmo);
        int typeExplosion = _dataExplosionTarget._idTypeExplosion;

        _explosionControl.SetCurrentExplosionType(typeExplosion);

    }

    public void GranadeModeInvoke()
    {
        var currenExplosion = GetCurrentExplosion();
        if (currenExplosion == null) return;

        if (GameSystem.Instance.isBombing == true)
        {
            return;
        }
        if (currenExplosion.GetComponent<ExplosionControl>().CurrentAmmo <= 0)
        {

            return;
        }
        GameSystem.Instance.isBombing = true;


        InvokeBombAnim(
         currenExplosion,
         GameSystem.Instance.explosionTime

       );

    }

    public void BombModeInvoke()
    {
        var currentExplosion = GetCurrentExplosion();
        if (currentExplosion == null) return;

        if (GameSystem.Instance.isBombing == true)
        {
            return;
        }
        if (currentExplosion.GetComponent<ExplosionControl>().CurrentAmmo <= 0)
        {
            OnOutOfBomb?.Invoke();
            return;
        }
        GameSystem.Instance.isBombing = true;



        InvokeBombAnim(
          currentExplosion,
         GameSystem.Instance.explosionTime
        );

    }
    void UpdateTextCoudoutTime()
    {
        var currentExplosion = GetCurrentExplosion();
        if (currentExplosion == null) return;

        float explosionTime = GameSystem.Instance.explosionTime;
        var explosionControl = currentExplosion.GetComponent<ExplosionControl>();
        float remainingTime = explosionTime;

        DOTween.To(() => remainingTime, x => remainingTime = x, 0, explosionTime)
            .OnUpdate(() =>
            {
                int minutes = Mathf.FloorToInt(remainingTime / 60);
                int seconds = Mathf.FloorToInt(remainingTime % 60);
                explosionControl.textTime.text = $"{minutes:D2}:{seconds:D2}";
            })
            .OnComplete(() =>
            {
                explosionControl.textTime.text = "00:00";
                Debug.Log("Bom!");
            });
    }

   

    void InvokeBombAnim(GameObject bombObj, float explosionTime)
    {
        if (bombObj == null) return;

        UpdateTextCoudoutTime();

        Sequence seq = DOTween.Sequence();
        int shakeSteps = Mathf.RoundToInt(explosionTime * 5); // Số lần rung (càng cao càng dồn dập)
        float totalShakeTime = explosionTime * 0.9f; // Dành 90% thời gian cho hiệu ứng rung trước khi nổ
        float postExplosionShakeTime = explosionTime * 0.1f; // Dành 10% thời gian cho hiệu ứng rung mạnh sau khi nổ

        for (int i = 0; i < shakeSteps; i++)
        {
            float stepProgress = (float)i / shakeSteps; // Tỷ lệ hoàn thành (0 -> 1)
            float stepDuration = Mathf.Lerp(totalShakeTime / shakeSteps, (totalShakeTime / shakeSteps) * 0.3f, stepProgress); // Giảm thời gian rung dần

            float shakeStrength = Mathf.Lerp(0.05f, 0.2f, stepProgress); // Cường độ rung tăng dần

            seq.Append(bombObj.transform.DOShakePosition(stepDuration, shakeStrength, 10 + i * 2, 90, false, true));
        }

        // *** Giai đoạn NỔ ***
        seq.AppendCallback(() =>
        {
            Debug.Log("💥 BOM NỔ!!!");

            // Hiệu ứng khói bốc lên
            var muzzlePosition = bombObj.GetComponent<IdExplosion>().GetMuzzlePosition();
            EffectSystem.Instance.SpawnExplosionEfxAt(muzzlePosition, bombObj.GetComponent<ExplosionControl>().ExplosionType);

            // Flashlight bật để tạo hiệu ứng sáng
            flashlightController.ToggleFlashlight();

            // Âm thanh vụ nổ
            SoundSystem.Instance.PlayExplosionSound();
        });

        // *** Dư chấn sau vụ nổ ***
        seq.Append(bombObj.transform.DOShakePosition(postExplosionShakeTime, 0.8f, 40, 90, false, true));

        seq.OnComplete(() =>
        {
            GameSystem.Instance.isBombing = false;
        });

        // seq.SetUpdate(UpdateType.Normal, true); // Đảm bảo tween chạy theo thời gian thực
    }
}