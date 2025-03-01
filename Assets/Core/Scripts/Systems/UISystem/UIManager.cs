using System.Collections.Generic;
using UnityEngine;





using System.Collections;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Unity.Mathematics;

public class UIManager : MonoBehaviour
{
  public static UIManager Instance { get; private set; }
  [SerializeField] GameObject[] uIHorizontalPrefabs;
  [SerializeField] GameObject[] uIVerticalPrefabs;
  [SerializeField] GameObject parentHorizontalCanva;
  [SerializeField] GameObject parentVerticalCanva;
  Dictionary<string, BaseUIRoot> uIRoots = new();
  Stack<string> stack = new();
  string currentName = "";
  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
  }

  private void Start()
  {
    InjectCamera();
    Init();
  }
  void InjectCamera() // điều kiện là spam manager có 2 gameobj thứ 
  {
    if (parentHorizontalCanva.TryGetComponent(out Canvas canvas))
    {
      canvas.renderMode = RenderMode.ScreenSpaceCamera;
      canvas.worldCamera = Camera.main;
    }
    if (parentVerticalCanva.TryGetComponent(out Canvas Vertical))
    {
      Vertical.renderMode = RenderMode.ScreenSpaceCamera;
      Vertical.worldCamera = Camera.main;
    }

  }

  void Init()
  {
    foreach (var uIPrefab in uIHorizontalPrefabs)
    {
      // if(!uIPrefab) continue; 
      BaseUIRoot ui = Instantiate(uIPrefab, parentHorizontalCanva.transform).GetComponent<BaseUIRoot>();
      if (ui == null) continue;
      uIRoots.Add(ui.name, ui);
      ui.DoOnCreate();
      ui.gameObject.SetActive(false);
    }
    // StartCoroutine(InstantiateModalAsset());


    foreach (var uIPrefab in uIVerticalPrefabs)
    {
      // if(!uIPrefab) continue; 
      BaseUIRoot ui = Instantiate(uIPrefab, parentVerticalCanva.transform).GetComponent<BaseUIRoot>();
      if (ui == null) continue;
      uIRoots.Add(ui.name, ui);
      ui.DoOnCreate();
      ui.gameObject.SetActive(false);
    }
  }

  public void Show(string name)
  {
    if (uIRoots.ContainsKey(name) && name != currentName)
    {
      // SoundSystem.Instance.PlayButtonSfx();
      GameSystem.Instance.SetGameState(GameState.Pause);
      if (currentName != "")
        uIRoots[name].DelayShow();
      else
        uIRoots[name].Show();

      HideCurrentName(name);
    }
    else
    {
      Debug.LogWarning("khong tim thay hoac da show :" + name);
    }
  }

  public void Hide()
  {
    if (uIRoots.ContainsKey(currentName))
    {
      // SoundSystem.Instance.PlayHidePopupSfx();
      uIRoots[currentName].Hide();
      ShowCurrentName();
      // if (currentName == "") 
      // GameSystem.Instance.SetGameState(GameState.Gameplay);
    }
  }

  void HideCurrentName(string name)
  {
    if (currentName != "")
    {
      stack.Push(currentName);
      uIRoots[currentName].Hide();
    }
    currentName = name;
  }

  void ShowCurrentName()
  {
    try
    {
      currentName = stack.Pop();
      uIRoots[currentName].DelayShow();
    }
    catch
    {
      currentName = "";
    }
  }

  public void HideAll()
  {
    if (uIRoots.ContainsKey(currentName))
    {
      SoundSystem.Instance.PlayHidePopupSfx();
      uIRoots[currentName].Hide();
      currentName = "";
      stack.Clear();
    }
  }

  public T GetUI<T>(string name) where T : BaseUIRoot
  {
    if (uIRoots.ContainsKey(name))
    {
      return uIRoots[name] as T;
    }

    return null;
  }


  //Test addrets
  [SerializeField] AssetReference[] assetRefsHorizontal;
  [SerializeField] AssetReference[] assetRefsVertical;


  public IEnumerator InstantiateModalAsset(Action _onCompleted = null)
  {
    for (int i = 0; i < assetRefsHorizontal.Length; i++)
    {
      AsyncOperationHandle<GameObject> handle = assetRefsHorizontal[i].InstantiateAsync(parentHorizontalCanva.transform);
      // Đảm bảo UI chưa xuất hiện ngay lập tức
      handle.Completed += (operation) =>
       {
         if (operation.Status == AsyncOperationStatus.Succeeded)
         {
           GameObject instantiatedObject = operation.Result;
           instantiatedObject.SetActive(false); // Tắt ngay khi tạo

           if (instantiatedObject.TryGetComponent(out BaseUIRoot ui))
           {
             uIRoots.Add(ui.name, ui);
             ui.DoOnCreate();
             ui.transform.localScale = Vector3.one;
           }
         }
       };

      yield return handle; // Chờ hoàn thành tải


      // yield return handle;
      // if (handle.Status == AsyncOperationStatus.Succeeded)
      // {

      //   if (handle.Result.TryGetComponent(out BaseUIRoot ui))
      //   {
      //     if (ui == null) continue;
      //     uIRoots.Add(ui.name, ui);
      //     ui.DoOnCreate();
      //     ui.transform.localScale = Vector3.one;
      //     // ui.gameObject.SetActive(false);
      //   }
      // }
    }

    for (int i = 0; i < assetRefsVertical.Length; i++)
    {
      AsyncOperationHandle<GameObject> handle = assetRefsVertical[i].InstantiateAsync(parentVerticalCanva.transform);
      // Đảm bảo UI chưa xuất hiện ngay lập tức
      handle.Completed += (operation) =>
       {
         if (operation.Status == AsyncOperationStatus.Succeeded)
         {
           GameObject instantiatedObject = operation.Result;
           instantiatedObject.SetActive(false); // Tắt ngay khi tạo

           if (instantiatedObject.TryGetComponent(out BaseUIRoot ui))
           {
             uIRoots.Add(ui.name, ui);
             ui.DoOnCreate();
             ui.transform.localScale = Vector3.one;
           }
         }
       };

      yield return handle; // Chờ hoàn thành tải

      // yield return handle;
      // if (handle.Status == AsyncOperationStatus.Succeeded)
      // {

      //   if (handle.Result.TryGetComponent(out BaseUIRoot ui))
      //   {
      //     if (ui == null) continue;
      //     uIRoots.Add(ui.name, ui);
      //     ui.DoOnCreate();
      //     ui.transform.localScale = Vector3.one;
      //     // ui.gameObject.SetActive(false);
      //   }
      // }
    }
  }
  //Test

}

