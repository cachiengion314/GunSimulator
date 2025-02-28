using UnityEngine;

public enum MapComponent
{
  None,
  Cup,
  Bottle,
  Remove,
  Box,
  WaitingCup
}

[ExecuteInEditMode]
public class LevelEditComponent : MonoBehaviour
{
  LevelEditor levelEditor;

  [Header("Block Setting")]
  public MapComponent component1;
  public MapComponent component2;

  [Header("Only use when has component cup")]
  [Range(1, 3)]
  public int CapacityCup;
  public ColorType CupColor;

  [Header("Only use when has component bottle")]
  public BottleDataEditor[] bottleDataEditors;

  [Header("Only use when has component box")]
  [Range(1, 3)]
  public int HealthBox;

  private Color _removeBlockColor;

  void Start()
  {
    _removeBlockColor = new Color(0.25f, 0.25f, 0.25f, 1);
  }

  void Update()
  {
    if (Application.isPlaying) return;

    if (component1 == MapComponent.None && component2 == MapComponent.None)
    {
      SetColor(Color.white);
    }
    else if (component1 == MapComponent.Cup || component2 == MapComponent.Cup)
    {
      SetColor(Color.red);
    }
    else if (component1 == MapComponent.Bottle || component2 == MapComponent.Bottle)
    {
      SetColor(Color.blue);
    }
    else if (component1 == MapComponent.Remove || component2 == MapComponent.Remove)
    {
      SetColor(_removeBlockColor);
    }
    else if (component1 == MapComponent.Box || component2 == MapComponent.Box)
    {
      SetColor(Color.yellow);
    }
  }

  public void SetColor(Color color)
  {
    if (GetComponent<SpriteRenderer>().color.Equals(color)) return;
    GetComponent<SpriteRenderer>().color = color;
  }

  public void InjectLevelEditor(LevelEditor levelEditor)
  {
    this.levelEditor = levelEditor;
  }
}

[SerializeField]
public struct Bottle
{

}
