using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class LevelDesignDatas
{
  public LevelDesignData[] datas;
}

[Serializable]
public class LevelDesignData
{
  public int Index;
  public int2 GridSize;
  public int Moves;
  public TrayData[] TrayDatas;
  public BottleDatas[] BottleDatas;
  public CupData[] CupDatas;
  public int[] GridTableCells;
  public int[] GridBoxes;
  public bool[] IsUnlockedBoosters;
  public int Difficulty;
}

[CreateAssetMenu(fileName = "LevelDesignObj", menuName = "ScriptableObjects/LevelDesignObj", order = 1)]
public class LevelDesignObj : ScriptableObject
{
  public int Index;
  public int2 GridSize;
  public int Moves;
  public TrayData[] TrayDatas;
  public BottleDatas[] BottleDatas;
  public CupData[] CupDatas;
  public int[] GridTableCells;
  public int[] GridBoxes;
  public bool[] IsUnlockedBoosters;
  public int Difficulty;
}