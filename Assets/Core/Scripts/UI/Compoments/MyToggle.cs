using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MyToggle : MonoBehaviour
{
    public UnityEvent onClickToggle;

    [SerializeField] Sprite On;
    [SerializeField] Sprite Off;
    Image btnImg;
    public bool IsOn;

    private void Awake()
    {
        btnImg = GetComponent<Image>();
    }

    public void OnClick()
    {
        SoundSystem.Instance.PlayButtonSfx();
        IsOn = !IsOn;
        ChangeState(IsOn);
        onClickToggle?.Invoke();
    }

    public void ChangeState(bool isOn)
    {
        IsOn = isOn;

        if (isOn) btnImg.sprite = On;
        else btnImg.sprite = Off;
    }
}
