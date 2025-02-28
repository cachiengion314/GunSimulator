using System;
using UnityEngine;

public class LobbyPageSelectSystem : MonoBehaviour
{
    [SerializeField] LobbyPageSelectCtrl[] lobbyPageSelectCtrls;
    [Header("Event")]
    public static Action<int> OnSelect;
    void Start()
    {
        Init();
    }
    void Init()
    {
        SelectAt(1);
    }
    public void SelectAt(int index)
    {
        OnSelect?.Invoke(index);
        for (int i = 0; i < lobbyPageSelectCtrls.Length; i++)
        {
            if (i == index)
                lobbyPageSelectCtrls[i].Selecting();
            else
                lobbyPageSelectCtrls[i].Unchecking();
        }
    }
}
