using System;
using HoangNam;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public partial class LevelEditor : MonoBehaviour
{
  public static LevelEditor Instance { get; private set; }

  [Header("External dependencies")]
  [SerializeField] LevelComponentParent levelComponentsParent;

  [Header("Prefabs")]
  [SerializeField] LevelDesignObj[] levelDesignObjs;
  public LevelDesignObj[] LevelDesignObjs { get { return levelDesignObjs; } }
  [SerializeField] Camera mainCam;
  [SerializeField] GridWorld grid;
  [SerializeField] GameObject cellTablePrefab;

  [Header("Level Datas")]
  [SerializeField] int2 gridSize;
  [SerializeField] int moves;

  [Header("Quest")]
  [SerializeField] TrayDataEditor[] trayDataEditors;

  [Header("Unlock Boosters")]
  [SerializeField] bool[] isUnlockedBoosters;

  [Header("Settings")]
  [SerializeField] LevelDifficulty difficulty;

  [Header("Level selection")]
  [Range(1, 200)]
  [SerializeField] int levelSelected;
  public bool IsSelectLevelEqualCurrent;

  private void Start()
  {
    if (Instance == null)
      Instance = this;
    else Destroy(gameObject);

    gameObject.SetActive(false);
    levelComponentsParent.gameObject.SetActive(false);

    if (IsSelectLevelEqualCurrent)
    {
      GameSystem.Instance.CurrentLevelIndex = levelSelected - 1;
    }
  }

  public LevelDesignObj GetCurrentLevelDesign()
  {
    return levelDesignObjs[levelSelected - 1];
  }


  [NaughtyAttributes.Button]
  void Clear()
  {
    gridSize = new int2(0, 0);
    moves = 0;
    isUnlockedBoosters = new bool[3];
    trayDataEditors = new TrayDataEditor[0];

    levelComponentsParent.Clear();
  }

  [NaughtyAttributes.Button]
  public void LoadLevelsFromDisk()
  {
    var _levelDesignsData = HoangNam.SaveSystem.Load<LevelDesignDatas>(
      KeyStr.NAME_LEVEL_DESIGN_DATAS
    );

    for (int i = 0; i < levelDesignObjs.Length; ++i)
    {
      var _levelDesignObj = levelDesignObjs[i];
      _levelDesignObj.Index = _levelDesignsData.datas[i].Index;
      _levelDesignObj.GridSize = _levelDesignsData.datas[i].GridSize;
      _levelDesignObj.Moves = _levelDesignsData.datas[i].Moves;
      _levelDesignObj.TrayDatas = _levelDesignsData.datas[i].TrayDatas;
      _levelDesignObj.BottleDatas = _levelDesignsData.datas[i].BottleDatas;
      _levelDesignObj.CupDatas = _levelDesignsData.datas[i].CupDatas;
      _levelDesignObj.GridTableCells = _levelDesignsData.datas[i].GridTableCells;
      _levelDesignObj.GridBoxes = _levelDesignsData.datas[i].GridBoxes;
      _levelDesignObj.IsUnlockedBoosters = _levelDesignsData.datas[i].IsUnlockedBoosters;
      _levelDesignObj.Difficulty = _levelDesignsData.datas[i].Difficulty;

      levelDesignObjs[i] = _levelDesignObj;
    }

    Debug.Log("LoadLevelsFromDisk successfully");
  }

  [NaughtyAttributes.Button]
  void LoadLevel()
  {
    Clear();

    var levelDesignObj = GetCurrentLevelDesign();

    if (levelDesignObj == null)
    {
      SaveLevel();
      return;
    }

    LoadGridSizeFrom(levelDesignObj);
    LoadMovesFrom(levelDesignObj);
    LoadTableCellsFrom(levelDesignObj); // load table cells in level editor scene
    LoadTrayDatasFrom(levelDesignObj);
    LoadBottleDatasFrom(levelDesignObj);
    LoadCupDatasFrom(levelDesignObj);
    LoadGridTableCellsFrom(levelDesignObj);
    LoadGridBoxesFrom(levelDesignObj);
    LoadIsUnlockedBoosterFrom(levelDesignObj);
    LoadDifficultyFrom(levelDesignObj);
    SetOrthographicSizeCamera();
  }

  [NaughtyAttributes.Button]
  void SaveLevel()
  {
    var levelDesignObj = GetCurrentLevelDesign();

    SaveGridSizeFrom(ref levelDesignObj);
    SaveMovesFrom(ref levelDesignObj);
    SaveTrayDatasFrom(ref levelDesignObj);
    SaveBottleDatasFrom(ref levelDesignObj);
    SaveCupDatasFrom(ref levelDesignObj);
    SaveGridTableCellsFrom(ref levelDesignObj);
    SaveGridBoxesFrom(ref levelDesignObj);
    SaveIsUnlockedBoosterFrom(ref levelDesignObj);
    SaveDifficultyFrom(ref levelDesignObj);

    // saving section
    var levelDesignDatas = new LevelDesignDatas
    {
      datas = new LevelDesignData[levelDesignObjs.Length]
    };
    // transfer data from scriptable obj to data json
    for (int i = 0; i < levelDesignObjs.Length; ++i)
    {
      levelDesignDatas.datas[i] = new LevelDesignData()
      {
        Index = i,
        GridSize = levelDesignObjs[i].GridSize,
        Moves = levelDesignObjs[i].Moves,
        TrayDatas = levelDesignObjs[i].TrayDatas,
        BottleDatas = levelDesignObjs[i].BottleDatas,
        CupDatas = levelDesignObjs[i].CupDatas,
        GridTableCells = levelDesignObjs[i].GridTableCells,
        GridBoxes = levelDesignObjs[i].GridBoxes,
        IsUnlockedBoosters = levelDesignObjs[i].IsUnlockedBoosters,
        Difficulty = levelDesignObjs[i].Difficulty,
      };

#if UNITY_EDITOR
      EditorUtility.SetDirty(levelDesignObjs[i]);
      AssetDatabase.SaveAssets();
#endif

    }
    HoangNam.SaveSystem.Save(levelDesignDatas, KeyStr.NAME_LEVEL_DESIGN_DATAS);
    Debug.Log("Save level successfully");
  }

  void SetOrthographicSizeCamera()
  {
    var size = grid.GridSize;

    var n = size.x;
    n = n > size.y ? n : size.y;

    mainCam.orthographicSize = 9 + (n - 3) * 3;
  }
}
