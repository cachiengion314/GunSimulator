using System;
using System.Collections;
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
    private Coroutine countdownCoroutine; // Lưu trữ coroutine để tránh chạy nhiều lần

    void UpdateTextCoudoutTime(ExplosionControl explosionControl, float explosionTime)
    {
        if (explosionControl == null || explosionControl.textTime == null) return;

        // Nếu đã có coroutine chạy, hủy trước khi chạy mới
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        // Bắt đầu coroutine mới
        countdownCoroutine = StartCoroutine(CountdownCoroutine(explosionControl, explosionTime));
    }

    IEnumerator CountdownCoroutine(ExplosionControl explosionControl, float explosionTime)
    {
        float remainingTime = explosionTime;

        while (remainingTime > 0)
        {
            int seconds = Mathf.FloorToInt(remainingTime); // Lấy phần giây nguyên
            int centiseconds = Mathf.FloorToInt((remainingTime - seconds) * 100); // Lấy hai chữ số của mili giây

            explosionControl.textTime.text = $"{seconds:D2}:{centiseconds:D2}"; // Định dạng XX:XX (giây:centiseconds)

            remainingTime -= Time.deltaTime;
            yield return null; // Chờ frame tiếp theo
        }

        // Khi countdown kết thúc
        explosionControl.textTime.text = "00:00";

    }

    void InvokeBombAnim(GameObject bombObj, float explosionTime)
    {
        if (bombObj == null) return;
        var explosionControl = bombObj.GetComponent<ExplosionControl>();
        if (explosionControl == null) return;

        // Gọi hàm update countdown text bằng Coroutine
        UpdateTextCoudoutTime(explosionControl, explosionTime);

        Sequence seq = DOTween.Sequence();
        int shakeSteps = Mathf.RoundToInt(explosionTime * 5); // Số lần rung
        float shakeTime = explosionTime; // Thời gian rung trước nổ
        float postExplosionShakeTime = 0.1f; // Dư chấn sau nổ

        float stepDuration = shakeTime / shakeSteps; // Chia đều thời gian rung để đảm bảo tổng thời gian đúng

        for (int i = 0; i < shakeSteps; i++)
        {
            float stepProgress = (float)i / shakeSteps;
            float shakeStrength = Mathf.Lerp(0.05f, 0.2f, stepProgress); // Tăng cường độ rung dần

            seq.Append(bombObj.transform.DOShakePosition(stepDuration, shakeStrength, 10 + i * 2, 90, false, true));
        }

        // *** Giai đoạn NỔ - Xảy ra ngay sau khi explosionTime kết thúc ***
        seq.AppendCallback(() =>
        {
            var muzzlePosition = bombObj.GetComponent<IdExplosion>().GetMuzzlePosition();
            EffectSystem.Instance.SpawnExplosionEfxAt(muzzlePosition, bombObj.GetComponent<ExplosionControl>().ExplosionType);

            flashlightController.ToggleFlashlight();
            SoundSystem.Instance.PlayExplosionSound();
        });

        // *** Dư chấn sau vụ nổ ***
        seq.Append(bombObj.transform.DOShakePosition(postExplosionShakeTime, 0.8f, 40, 90, false, true));

        seq.OnComplete(() =>
        {
            GameSystem.Instance.isBombing = false;
        });

        seq.SetUpdate(UpdateType.Normal, true);
    }

}