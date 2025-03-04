using MoreMountains.Feedbacks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
public class ExplosionControl : MonoBehaviour,
  ISpriteRenderer,
  IdExplosion,
  IFeedbackControl,
  IDoTweenControl
{
    [Header("Dependencies")]
    [SerializeField] SpriteRenderer[] bodyRenderers;
    public SpriteRenderer BodyRenderer
    {
        get
        {
            _bodyRendererIndex = math.max(0, _bodyRendererIndex);
            _bodyRendererIndex = math.min(bodyRenderers.Length, _bodyRendererIndex);
            return bodyRenderers[_bodyRendererIndex];
        }
    }
    public TMP_Text textTime;

    [Header("Datas")]
    Quaternion _initRotation;
    int _bodyRendererIndex;
    bool _isTweening;
    [SerializeField] int _initAmmo;
    public int InitAmmo { get { return _initAmmo; } }
    [SerializeField] int _currentAmmo;
    public int CurrentAmmo { get { return _currentAmmo; } }
    [SerializeField] int explosionType;
    public int ExplosionType { get { return explosionType; } }

    [SerializeField] float explosionTime;
    public float ExplosionTime { get { return explosionTime; } }

    public void ChangeBodyRendererBaseOn(int idx)
    {
        _bodyRendererIndex = idx;
        for (int i = 0; i < bodyRenderers.Length; ++i)
        {
            bodyRenderers[i].gameObject.SetActive(false);
            if (i == idx) bodyRenderers[i].gameObject.SetActive(true);
        }
        _initRotation = transform.rotation;
    }

    /// <summary>
    /// lượng đạn ban đầu
    /// </summary>
    /// <param name="ammo"></param>
    public void SetInitAmmo(int ammo)
    {
        _initAmmo = ammo;
        // SetCurrentAmmo(ammo);
    }

    /// <summary>
    /// hàm set lượng đạn hiện tại
    /// </summary>
    /// <param name="ammo"></param>
    public void SetCurrentAmmo(int ammo)
    {
        _currentAmmo = ammo;
    }

    /// <summary>
    /// lấy số lượng đạn hiện có
    /// </summary>
    /// <param name="ammo"></param>
    public int GetCurrentAmmo()
    {
        return _currentAmmo;
    }

    public float3 GetMuzzlePosition()
    {
        return BodyRenderer.transform.GetChild(0).position;
    }

    public int GetSortingOrder()
    {
        throw new System.NotImplementedException();
    }

    public void ResetSortingOrder()
    {
        throw new System.NotImplementedException();
    }

    public void SetInitSortingOrder(int sortingOrder)
    {
        throw new System.NotImplementedException();
    }

    public void SetSortingOrder(int sortingOrder)
    {
        throw new System.NotImplementedException();
    }

    public void InjectChannel(int channelId)
    {
        BodyRenderer.GetComponent<MMPositionShaker>().Channel = channelId;
    }

    public bool IsTweening()
    {
        return _isTweening;
    }

    public void ChangeTweeningTo(bool onOffValue)
    {
        _isTweening = onOffValue;
    }

    public float3 GetBulletBurstUpPosition()
    {
        return BodyRenderer.transform.GetChild(1).position;
    }

    public Quaternion GetInitRotation()
    {
        return _initRotation;
    }
    public void SetCurrentExplosionType(int type)
    {
        explosionType = type;
        // SetCurrentAmmo(ammo);
    }
}
