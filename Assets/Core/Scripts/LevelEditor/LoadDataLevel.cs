using System;
using DG.Tweening;
using UnityEngine;

public partial class LevelEditor : MonoBehaviour
{
  private void LoadGridSizeFrom(LevelDesignObj levelDesignObj)
  {
    gridSize = levelDesignObj.GridSize;

    grid.GridSize = levelDesignObj.GridSize;
    grid.BakingGridWorld();
  }

  private void LoadMovesFrom(LevelDesignObj levelDesignObj)
  {
    moves = levelDesignObj.Moves;
  }

  private void LoadTableCellsFrom(LevelDesignObj levelDesignObj, Action onCompleted = null)
  {
    for (int i = 0; i < grid.Grid.Length; i++)
    {
      var cellTable = Instantiate(cellTablePrefab, levelComponentsParent.transform);
      cellTable.transform.position = grid.ConvertIndexToWorldPos(i);
    }
  }

  private void LoadTrayDatasFrom(LevelDesignObj levelDesignObj)
  {
    var trayDatas = levelDesignObj.TrayDatas;
    trayDataEditors = new TrayDataEditor[trayDatas.Length];

    for (int i = 0; i < trayDataEditors.Length; i++)
    {
      trayDataEditors[i].Color = (ColorType)trayDatas[i].Color;
    }
  }

  private void LoadBottleDatasFrom(LevelDesignObj levelDesignObj)
  {
    var bottleDatas = levelDesignObj.BottleDatas;

    for (int i = 0; i < bottleDatas.Length; i++)
    {
      var bottleData = bottleDatas[i];
      var component = levelComponentsParent.transform.GetChild(bottleData.PosIndex).GetComponent<LevelEditComponent>();

      component.component1 = MapComponent.Bottle;

      component.bottleDataEditors = new BottleDataEditor[bottleData.bottleDatas.Length];
      for (int j = 0; j < component.bottleDataEditors.Length; j++)
      {
        component.bottleDataEditors[j].Color = (ColorType)bottleData.bottleDatas[j].ColorIndex;
        component.bottleDataEditors[j].CapacityBottle = bottleData.bottleDatas[j].BottleCapacity;
      }
    }
  }

  private void LoadCupDatasFrom(LevelDesignObj levelDesignObj)
  {
    var cupDatas = levelDesignObj.CupDatas;

    for (int i = 0; i < cupDatas.Length; i++)
    {
      var cupData = cupDatas[i];
      var component = levelComponentsParent.transform.GetChild(cupData.PosIndex).GetComponent<LevelEditComponent>();

      component.component1 = MapComponent.Cup;
      component.CupColor = (ColorType)cupData.ColorIndex;
      component.CapacityCup = cupData.CupCapacity;
    }
  }

  private void LoadGridTableCellsFrom(LevelDesignObj levelDesignObj)
  {
    var gridTableCells = levelDesignObj.GridTableCells;

    for (int i = 0; i < gridTableCells.Length; i++)
    {
      var tableCell = gridTableCells[i];
      var component = levelComponentsParent.transform.GetChild(i).GetComponent<LevelEditComponent>();
      if (tableCell == 0) continue;

      component.component1 = MapComponent.Remove;
    }
  }

  private void LoadGridBoxesFrom(LevelDesignObj levelDesignObj)
  {
    var gridBoxes = levelDesignObj.GridBoxes;

    for (int i = 0; i < gridBoxes.Length; i++)
    {
      var box = gridBoxes[i];
      var component = levelComponentsParent.transform.GetChild(i).GetComponent<LevelEditComponent>();
      if (box == 0) continue;

      component.component1 = MapComponent.Box;
      component.HealthBox = box;
    }
  }

  private void LoadIsUnlockedBoosterFrom(LevelDesignObj levelDesignObj)
  {
    isUnlockedBoosters = levelDesignObj.IsUnlockedBoosters;
  }

  private void LoadDifficultyFrom(LevelDesignObj levelDesignObj)
  {
    difficulty = (LevelDifficulty)levelDesignObj.Difficulty;
  }
}