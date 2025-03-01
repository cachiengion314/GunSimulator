using Unity.Mathematics;
using UnityEngine;
using HoangNam;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    public float _countdown = 0;
    [SerializeField] private float _BulletSpeed;
    [SerializeField] GameObject[] _PrefabListGunPostil;
    [SerializeField] GameObject[] _PrefabListGunSMG;
    [SerializeField] GameObject[] _PrefabListShotGun;
    [SerializeField] GameObject[] _PrefabListRiffleGun;
    [SerializeField] GameObject[] _PrefabListSniperGun;
    BoxCollider2D _BoxCollider2D;
    [Header("---Burst---")]
    public bool _isbursting = false;
    public int countBurst = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }
    void Start()
    {
        var gunData = DataGunManager.Instance.GetGunDataClass(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick);
       
        CreatPlayer(GameSystem.Instance.IdShopMode);
    }

    void Update()
    {
        UpdateCoundownBulle();
    }

    void OnDetect(float2 pos, Collider2D[] _Collider2Ds)
    {
        switch (GameSystem.Instance.IdShopMode)
        {
            case 0:
                GunModeTap(pos, _Collider2Ds[0]);
                break;
            case 1:
                ExplosionMode(pos, _Collider2Ds[0]);
                break;
            case 2:
                TaserGunMode(pos, _Collider2Ds[0]);
                break;
            case 3:
                LightSaberMode(pos, _Collider2Ds[0]);
                break;
        }


    }
    void OnHoldDetect(float2 pos, Collider2D[] _Collider2Ds)
    {
        // Mặc định chuyển về chế độ Single khi chạm
        switch (GameSystem.Instance.IdShopMode)
        {
            case 0:
                GunModeHold(pos, _Collider2Ds);
                break;
        }
    }
    void CreatPlayer(int _idShop) // id shop 0 = Gun .1 = Explision .2 = TaserGun . 3 = LightSaber
    {
        switch (_idShop)
        {
            case 0:
                CreatGunTarget();
                break;
            case 1:
                CreatExpplosionTarget();
                break;
            case 2:
                CreatTaserGunTarget();

                break;
            case 3:
                CreatLightSaberTarget();
                break;
        }
    }


    void CreatGunTarget()
    {
        int TypePick = GameSystem.Instance.IdTypePick;
        int GunPick = GameSystem.Instance.IdGunPick;
        GameObject GunTarget = null;

        switch (TypePick)
        {
            case 0:
                GunTarget = Instantiate(_PrefabListGunPostil[GunPick], this.transform);
                break;
            case 1:
                GunTarget = Instantiate(_PrefabListGunSMG[GunPick], this.transform);
                break;
            case 2:
                GunTarget = Instantiate(_PrefabListShotGun[GunPick], this.transform);
                break;
            case 3:
                GunTarget = Instantiate(_PrefabListRiffleGun[GunPick], this.transform);
                break;
            case 4:
                GunTarget = Instantiate(_PrefabListSniperGun[GunPick], this.transform);
                break;
        }

        if (GunTarget != null)
        {
            GunTarget.transform.GetChild(1).gameObject.SetActive(false); // Tắt FX
            _BoxCollider2D = GunTarget.GetComponent<BoxCollider2D>();
            UnityEngine.Debug.Log("_BoxCollider2D " + _BoxCollider2D);
        }

    }
    void CreatExpplosionTarget()
    {
        // int TypePick = GameSystem.Instance._idTypePick;
        // int GunPick = GameSystem.Instance._idGunPick;
        // GameObject GunTarget = null;

        // switch (TypePick)
        // {
        //     case 0:
        //         GunTarget = Instantiate(_PrefabListGunPostil[GunPick], this.transform);
        //         break;
        //     case 1:
        //         GunTarget = Instantiate(_PrefabListGunSMG[GunPick], this.transform);
        //         break;
        //     case 2:
        //         GunTarget = Instantiate(_PrefabListShotGun[GunPick], this.transform);
        //         break;
        //     case 3:
        //         GunTarget = Instantiate(_PrefabListRiffleGun[GunPick], this.transform);
        //         break;
        //     case 4:
        //         GunTarget = Instantiate(_PrefabListSniperGun[GunPick], this.transform);
        //         break;
        // }

        // if (GunTarget != null)
        // {
        //     GunTarget.transform.GetChild(1).gameObject.SetActive(false); // Tắt FX
        //     _BoxCollider2D = GunTarget.GetComponent<BoxCollider2D>();
        //     UnityEngine.Debug.Log("_BoxCollider2D " + _BoxCollider2D);
        // }

    }
    void CreatTaserGunTarget()
    {
        // int TypePick = GameSystem.Instance._idTypePick;
        // int GunPick = GameSystem.Instance._idGunPick;
        // GameObject GunTarget = null;

        // switch (TypePick)
        // {
        //     case 0:
        //         GunTarget = Instantiate(_PrefabListGunPostil[GunPick], this.transform);
        //         break;
        //     case 1:
        //         GunTarget = Instantiate(_PrefabListGunSMG[GunPick], this.transform);
        //         break;
        //     case 2:
        //         GunTarget = Instantiate(_PrefabListShotGun[GunPick], this.transform);
        //         break;
        //     case 3:
        //         GunTarget = Instantiate(_PrefabListRiffleGun[GunPick], this.transform);
        //         break;
        //     case 4:
        //         GunTarget = Instantiate(_PrefabListSniperGun[GunPick], this.transform);
        //         break;
        // }

        // if (GunTarget != null)
        // {
        //     GunTarget.transform.GetChild(1).gameObject.SetActive(false); // Tắt FX
        //     _BoxCollider2D = GunTarget.GetComponent<BoxCollider2D>();
        //     UnityEngine.Debug.Log("_BoxCollider2D " + _BoxCollider2D);
        // }

    }
    void CreatLightSaberTarget()
    {
        // int TypePick = GameSystem.Instance._idTypePick;
        // int GunPick = GameSystem.Instance._idGunPick;
        // GameObject GunTarget = null;

        // switch (TypePick)
        // {
        //     case 0:
        //         GunTarget = Instantiate(_PrefabListGunPostil[GunPick], this.transform);
        //         break;
        //     case 1:
        //         GunTarget = Instantiate(_PrefabListGunSMG[GunPick], this.transform);
        //         break;
        //     case 2:
        //         GunTarget = Instantiate(_PrefabListShotGun[GunPick], this.transform);
        //         break;
        //     case 3:
        //         GunTarget = Instantiate(_PrefabListRiffleGun[GunPick], this.transform);
        //         break;
        //     case 4:
        //         GunTarget = Instantiate(_PrefabListSniperGun[GunPick], this.transform);
        //         break;
        // }

        // if (GunTarget != null)
        // {
        //     GunTarget.transform.GetChild(1).gameObject.SetActive(false); // Tắt FX
        //     _BoxCollider2D = GunTarget.GetComponent<BoxCollider2D>();
        //     UnityEngine.Debug.Log("_BoxCollider2D " + _BoxCollider2D);
        // }

    }



    public void Fire(int _idTypePick, int _idGunPick, int vaule) // Gọi từ UI để bắn
    {
        var gunData = DataGunManager.Instance.GetGunDataClass(_idTypePick, _idGunPick);
        if (_countdown > 0)
        {
            return;
        }
        if (gunData._currentValue <= 0)
        {
            UnityEngine.Debug.Log("hết đạn nạp đi ");
            // show popup BuyBullet
            GameSystem.Instance.SetGameState(GameState.Pause);
            UIManager.Instance.Show(KeyStr.NAME_BuyBullet_MODAL);
            return;
        }
        // Gửi tín hiệu cập nhật đạn
        DataGunManager.Instance.UpdateGunCurrenValue(_idTypePick, _idGunPick, vaule); // trừ đạn
        UiIngameRoot.Instance.UpdateTextCurrentBullet(); // update lại text đạn
        _countdown = _BulletSpeed; // đặt lại thời gian chờ để bắn đạn
        SoundSystem.Instance.PlayPistolSfx();
        AnimAtk();
    }
    public void AnimAtk()
    {
        GameObject Gun = this.transform.GetChild(0).gameObject; // parent FX
        GameObject FxATK = Gun.transform.GetChild(1).gameObject; // Gameobjcet FX
        // Animator animator = FxATK.GetComponent<Animator>();
        // animator.speed = 1 / _BulletSpeed; // 1
        // animator.Play(0);
        FxATK.SetActive(true);
    }

    public void HideAnim(float2 pos1, float2 pos2)
    {
        if (GameSystem.Instance.IdFireModes == 2)
        {
            return;
        }
        UiIngameRoot.Instance.TypeFireModes[0].gameObject.SetActive(true);
        UiIngameRoot.Instance.TypeFireModes[1].gameObject.SetActive(false);
    }
    void UpdateCoundownBulle()
    {
        if (_countdown > 0)
        {
            _countdown -= Time.deltaTime; // Giảm thời gian chờ
        }

    }


    void GunModeTap(float2 pos, Collider2D _Collider2D)
    {
        // Mặc định chuyển về chế độ Single khi chạm
        if (GameSystem.Instance.IdFireModes == 2)
        {
            if (_BoxCollider2D == _Collider2D)
            {
                if (_isbursting == true) // đang bắn thì dừng
                {
                    return;
                }
                Fire(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick, 3);
            }
            return;
        }
        GameSystem.Instance.IdFireModes = 0;


        if (_BoxCollider2D == _Collider2D)
        {
            Fire(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick, 1);
        }
    }
    void GunModeHold(float2 pos, Collider2D[] _Collider2D)
    {
        if (GameSystem.Instance.IdFireModes == 2)
        {
            if (_isbursting == true) // đang bắn thì dừng
            {
                return;
            }
            foreach (Collider2D _collider2d in _Collider2D)
            {
                if (_BoxCollider2D == _collider2d)
                {
                    Fire(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick, 3);
                    _isbursting = true;
                }
            }
            return;
        }
        // Chuyển sang chế độ Auto
        GameSystem.Instance.IdFireModes = 1;
        UnityEngine.Debug.Log("Chuyển sang chế độ Auto");
        UiIngameRoot.Instance.TypeFireModes[0].gameObject.SetActive(false);
        UiIngameRoot.Instance.TypeFireModes[1].gameObject.SetActive(true);

        foreach (Collider2D _collider2d in _Collider2D)
        {
            if (_BoxCollider2D == _collider2d)
            {
                Fire(GameSystem.Instance.IdTypePick, GameSystem.Instance.IdGunPick, 1);
            }
        }
    }
    void ExplosionMode(float2 pos, Collider2D _Collider2D)
    {

    }
    void TaserGunMode(float2 pos, Collider2D _Collider2D)
    {

    }
    void LightSaberMode(float2 pos, Collider2D _Collider2D)
    {

    }
    // IEnumerator OffFX()
    // {

    // }

}

