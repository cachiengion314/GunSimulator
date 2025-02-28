using System.Collections.Generic;
using UnityEngine;

public partial class LevelEditor : MonoBehaviour
{
  private void SaveGridSizeFrom(ref LevelDesignObj levelDesignObj)
  {
    grid.GridSize = gridSize;
    grid.BakingGridWorld();
    levelDesignObj.GridSize = gridSize;
  }

  private void SaveMovesFrom(ref LevelDesignObj levelDesignObj)
  {
    levelDesignObj.Moves = moves;
  }

  private void SaveTrayDatasFrom(ref LevelDesignObj levelDesignObj)
  {
    levelDesignObj.TrayDatas = new TrayData[trayDataEditors.Length];

    for (int i = 0; i < trayDataEditors.Length; i++)
    {
      levelDesignObj.TrayDatas[i].Color = (int)trayDataEditors[i].Color;
    }
  }

  private void SaveBottleDatasFrom(ref LevelDesignObj levelDesignObj)
  {
    List<BottleDatas> _bottleDatas = new();

    for (int i = 0; i < grid.Grid.Length; i++)
    {
      if (i >= levelComponentsParent.transform.childCount) break;
      var component = levelComponentsParent.transform.GetChild(i).GetComponent<LevelEditComponent>();

      if (component.component1 != MapComponent.Bottle && component.component2 != MapComponent.Bottle) continue;

      var bottleData = new BottleData[component.bottleDataEditors.Length];
      for (int j = 0; j < bottleData.Length; j++)
      {
        bottleData[j].ColorIndex = (int)component.bottleDataEditors[j].Color;
        bottleData[j].BottleCapacity = component.bottleDataEditors[j].CapacityBottle;
      }

      var bottleDatas = new BottleDatas
      {
        bottleDatas = bottleData,
        PosIndex = i
      };

      _bottleDatas.Add(bottleDatas);
    }

    levelDesignObj.BottleDatas = _bottleDatas.ToArray();
  }

  private void SaveCupDatasFrom(ref LevelDesignObj levelDesignObj)
  {
    List<CupData> cupDatas = new();

    for (int i = 0; i < grid.Grid.Length; i++)
    {
      if (i >= levelComponentsParent.transform.childCount) break;
      var component = levelComponentsParent.transform.GetChild(i).GetComponent<LevelEditComponent>();

      if (component.component1 != MapComponent.Cup && component.component2 != MapComponent.Cup) continue;

      var cupData = new CupData
      {
        ColorIndex = (int)component.CupColor,
        CupCapacity = component.CapacityCup,
        PosIndex = i
      };

      cupDatas.Add(cupData);
    }

    levelDesignObj.CupDatas = cupDatas.ToArray();
  }

  private void SaveGridTableCellsFrom(ref LevelDesignObj levelDesignObj)
  {
    levelDesignObj.GridTableCells = new int[grid.Grid.Length];

    for (int i = 0; i < grid.Grid.Length; i++)
    {
      if (i >= levelComponentsParent.transform.childCount) break;

      var component = levelComponentsParent.transform.GetChild(i).GetComponent<LevelEditComponent>();

      if (component.component1 != MapComponent.Remove && component.component2 != MapComponent.Remove) continue;

      levelDesignObj.GridTableCells[i] = -1;
    }
  }

  private void SaveGridBoxesFrom(ref LevelDesignObj levelDesignObj)
  {
    levelDesignObj.GridBoxes = new int[grid.Grid.Length];

    for (int i = 0; i < grid.Grid.Length; i++)
    {
      if (i >= levelComponentsParent.transform.childCount) break;

      var component = levelComponentsParent.transform.GetChild(i).GetComponent<LevelEditComponent>();

      if (component.component1 != MapComponent.Box && component.component2 != MapComponent.Box) continue;

      levelDesignObj.GridBoxes[i] = component.HealthBox;
    }
  }

  private void SaveIsUnlockedBoosterFrom(ref LevelDesignObj levelDesignObj)
  {
    levelDesignObj.IsUnlockedBoosters = isUnlockedBoosters;
  }

  private void SaveDifficultyFrom(ref LevelDesignObj levelDesignObj)
  {
    levelDesignObj.Difficulty = (int)difficulty;
  }
}